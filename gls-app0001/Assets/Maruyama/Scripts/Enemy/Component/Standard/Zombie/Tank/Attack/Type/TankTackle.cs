using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using MaruUtility;

public class TankTackle : AttackNodeBase
{
    public enum State
    {
        None,
        Charge,
        Tackle,
        TackleLast,
    }

    [SerializeField]
    float m_tackleSpeed = 15.0f;

    [SerializeField]
    float m_tackleLastRange = 2.5f;

    [SerializeField]
    float m_eyeDeg = 40.0f;
    float m_eyeRad = 0.0f;

    [SerializeField]
    float m_turningPower = 3.0f;

    /// <summary>
    /// //ターゲットとのフォワードの差がこの数字よりより小さければ、予測タックルにする。
    /// </summary>
    [SerializeField]
    float m_subPursuitTargetForward = 0.3f;

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    ReactiveProperty<State> m_state = new ReactiveProperty<State>(State.None);

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    Stator_ZombieTank m_stator;
    EyeSearchRange m_eye;
    AnimatorManager_ZombieTank m_animatorManager;

    TaskList<State> m_taskList = new TaskList<State>();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_eye = GetComponent<EyeSearchRange>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieTank>();

        m_eyeRad = m_eyeDeg * Mathf.Deg2Rad;
    }

    private void Update()
    {
        Rotation();

        TackleMove();
    }

    Vector3 CalcuToTargetVec()
    {
        var target = m_targetManager.GetNowTarget();
        if(target == null) {
            return Vector3.zero;
        }

        var toVec = target.gameObject.transform.position - transform.position;
        return toVec;
    }

    void TackleMove()
    {
        if (m_state.Value != State.Tackle) {  //Tackleでなかったら処理をしない
            return;
        }

        Vector3 force = CalcuTackleForce();

        m_velocityManager.AddForce(force);
        Debug.Log(m_velocityManager.velocity.magnitude);
        //m_animatorManager.TackleSpeed = m_velocityManager.velocity.magnitude / m_tackleSpeed;

        if (IsTackleEnd()) //ターゲットに限りなく近づいたら
        {
            TackleLastStart();
        }
    }

    Vector3 CalcuTackleForce()
    {
        var target = m_targetManager.GetNowTarget();
        if(target == null) { 
            return Vector3.zero; 
        }

        var velocity = m_velocityManager.velocity;
        var toVec = CalcuToTargetVec();

        var relativeHeading = Vector3.Dot(transform.forward, target.transform.forward);

        //前方にいて、かつ、自分と相手のフォワードの差が開いていなかったら、通常Seek
        //Dotは差が開いている程、値が小さくなる。
        if (IsFront(toVec) && IsMinSubForward(relativeHeading, m_subPursuitTargetForward))
        {
            return CalcuVelocity.CalucSeekVec(velocity, toVec, m_tackleSpeed);
        }
        else
        {   //フォワードの差が開いていたら、予測Seek
            Debug.Log("予測Seek");
            return CalcuVelocity.CalcuPursuitForce(
                velocity, toVec, m_tackleSpeed, gameObject, target.GetComponent<Rigidbody>(), m_turningPower);
        }
    }

    void Rotation()
    {
        var forward = m_state.Value switch {
            State.Charge => CalcuToTargetVec().normalized,
            State.Tackle => m_velocityManager.velocity,
            _ => Vector3.zero
        };

        if (forward == Vector3.zero) {
            return;
        }

        transform.forward = forward;
    }

    public override void AttackStart()
    {
        m_state.Value = State.Charge;
        
        enabled = true;
    }

    public  void TackleStart()
    {
        m_state.Value = State.Tackle;
    }

    private void TackleLastStart()
    {
        m_velocityManager.StartDeseleration();
        m_state.Value = State.TackleLast;
    }

    public override bool IsAttackStartRange()
    {
        return IsTargetRange(GetBaseParam().startRange);
    }

    private bool IsTargetRange(float range)
    {
        FoundObject target = m_targetManager.GetNowTarget();
        if (target) {
            return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// ターゲットが正面にいるかどうか
    /// </summary>
    /// <param name="toTargetVec">ターゲットの方向</param>
    /// <returns>正面ならtrue</returns>
    bool IsFront(Vector3 toTargetVec)
    {
        return Vector3.Dot(toTargetVec, transform.forward) > 0 ? true : false;
    }

    /// <summary>
    /// 自分と相手のフォワードの差が開いていなかったら
    /// </summary>
    /// <param name="relativeHeading">自分と相手のフォワードのdot</param>
    /// <returns>差が開いていなかったらtrue</returns>
    bool IsMinSubForward(float relativeHeading, float sub)
    {
        return Mathf.Abs(relativeHeading) > sub ? true : false;
    }

    /// <summary>
    /// タックルの終了条件
    /// </summary>
    /// <returns></returns>
    bool IsTackleEnd()
    {
        var speedRate = m_velocityManager.velocity.magnitude / m_tackleSpeed;
        var range = m_tackleLastRange * speedRate;
        if (IsTargetRange(range)) {
            return true;
        }

        if (!IsTackleEyeRad()) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// タックルの追従する角度
    /// </summary>
    /// <returns></returns>
    public bool IsTackleEyeRad()
    {
        var target = m_targetManager.GetNowTarget();
        if(target == null) {
            return false;
        }

        var eyeParam = m_eye.GetParam();
        eyeParam.rad = m_eyeRad;
        if (m_eye.IsInEyeRange(m_targetManager.GetNowTarget().gameObject, eyeParam))
        {
            return true;
        }

        return false;
    }

    public override void EndAnimationEvent()
    {
        Debug.Log("EndAnimation");
        m_stator.GetTransitionMember().chaseTrigger.Fire();

        m_state.Value = State.None;

        m_velocityManager.SetIsDeseleration(false);
        enabled = false;
    }

    /// <summary>
    /// 壁に突っかかりを回避
    /// </summary>
    /// <param name="collision">コリジョン</param>
    void TakcleWallAvoid(Collision collision)
    {
        foreach (var layerString in m_rayObstacleLayerStrings)
        {
            int obstacleLayer = LayerMask.NameToLayer(layerString);
            if (collision.gameObject.layer == obstacleLayer) {  //障害物と当たったら
                //TackleLastStart();  //タックルを終了
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_state.Value == State.Tackle) {  //タックル状態で
            TakcleWallAvoid(collision);  //壁の突っかかり回避
        }

        //攻撃状態なら
        if (m_state.Value == State.Tackle || m_state.Value == State.TackleLast) {
            var damage = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();
            damage?.TakeDamage(GetBaseParam().damageData);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (m_state.Value == State.Tackle) {  //タックル状態で
            TakcleWallAvoid(collision);  //壁の突っかかり回避
        }
    }

    //アクセッサ----------------------------------------

    public ReactiveProperty<State> state
    {
        get { return m_state; }
    }

}
