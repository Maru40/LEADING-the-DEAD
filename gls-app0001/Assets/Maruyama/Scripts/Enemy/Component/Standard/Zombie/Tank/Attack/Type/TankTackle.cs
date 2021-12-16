using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using MaruUtility;
using AttributeObject;

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
    private float m_tackleSpeed = 15.0f;

    [SerializeField]
    private float m_tackleLastRange = 2.5f;

    [SerializeField]
    private float m_eyeDeg = 40.0f;
    private float m_eyeRad = 0.0f;

    [SerializeField]
    private float m_turningPower = 3.0f;

    /// <summary>
    /// //ターゲットとのフォワードの差がこの数字よりより小さければ、予測タックルにする。
    /// </summary>
    [SerializeField]
    private float m_subPursuitTargetForward = 0.3f;

    [Header("タックル終了後の待機時間"),SerializeField]
    private float m_waitTime = 1.0f; //タックル終了後の待機時間

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    private string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    private bool m_isAttack = false;

    private ReactiveProperty<State> m_state = new ReactiveProperty<State>(State.None);

    private TargetManager m_targetManager;
    private EnemyVelocityMgr m_velocityManager;
    private Stator_ZombieTank m_stator;
    private EyeSearchRange m_eye;
    private AnimatorManager_ZombieTank m_animatorManager;
    private WaitTimer m_waitTimer;

    private TaskList<State> m_taskList = new TaskList<State>();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_eye = GetComponent<EyeSearchRange>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieTank>();
        m_waitTimer = GetComponent<WaitTimer>();

        m_eyeRad = m_eyeDeg * Mathf.Deg2Rad;
    }

    private void Update()
    {
        Rotation();

        TackleMove();
    }

    private Vector3 CalcuToTargetVec()
    {
        var target = m_targetManager.GetNowTarget();
        if(target == null) {
            return Vector3.zero;
        }

        var toVec = target.gameObject.transform.position - transform.position;
        return toVec;
    }

    private void TackleMove()
    {
        if (m_state.Value != State.Tackle) {  //Tackleでなかったら処理をしない
            return;
        }

        Vector3 force = CalcuTackleForce();

        m_velocityManager.AddForce(force);

        if (IsTackleEnd()) //ターゲットに限りなく近づいたら
        {
            TackleLastStart();
        }
    }

    private Vector3 CalcuTackleForce()
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

    private void Rotation()
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
    private bool IsFront(Vector3 toTargetVec)
    {
        return Vector3.Dot(toTargetVec, transform.forward) > 0 ? true : false;
    }

    /// <summary>
    /// 自分と相手のフォワードの差が開いていなかったら
    /// </summary>
    /// <param name="relativeHeading">自分と相手のフォワードのdot</param>
    /// <returns>差が開いていなかったらtrue</returns>
    private bool IsMinSubForward(float relativeHeading, float sub)
    {
        return Mathf.Abs(relativeHeading) > sub ? true : false;
    }

    /// <summary>
    /// タックルの終了条件
    /// </summary>
    /// <returns></returns>
    private bool IsTackleEnd()
    {
        var speedRate = m_velocityManager.velocity.magnitude / m_tackleSpeed;
        var range = m_tackleLastRange * speedRate;
        if (IsTargetRange(range)) {
            return true;
        }

        if (!IsTackleEyeRange()) {  //視界から外れたらタックル終了
            return true;
        }

        return false;
    }

    /// <summary>
    /// タックルの追従する角度
    /// </summary>
    /// <returns></returns>
    public bool IsTackleEyeRange()
    {
        var target = m_targetManager.GetNowTarget();
        if(target == null) {
            return false;
        }

        //const float eyeRange = 50.0f;  //タックル中の視界の広さ
        //var eyeParam = m_eye.GetParam();
        //eyeParam.rad = m_eyeRad;
        //eyeParam.range = eyeRange;
        //if (m_eye.IsInEyeRange(m_targetManager.GetNowTarget().gameObject, eyeParam))
        //{
        //    return true;
        //}

        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        if (UtilityMath.IsFront(transform.forward , toTargetVec)){  //正面にいるなら
            return true;
        }

        return false;
    }

    public override void EndAnimationEvent()
    {
        Debug.Log("EndAnimation");
        //m_stator.GetTransitionMember().chaseTrigger.Fire();

        m_state.Value = State.None;

        m_velocityManager.SetIsDeseleration(false);
        m_velocityManager.ResetAll();
        enabled = false;

        m_waitTimer.AddWaitTimer(GetType(), m_waitTime, () => m_stator.GetTransitionMember().chaseTrigger.Fire());
    }

    /// <summary>
    /// 壁の突っかかりを回避
    /// </summary>
    /// <param name="collision">コリジョン</param>
    private void TackleWallAvoid(Collision collision)
    {
        foreach (var layerString in m_rayObstacleLayerStrings)
        {
            int obstacleLayer = LayerMask.NameToLayer(layerString);
            if (collision.gameObject.layer == obstacleLayer) {  //障害物と当たったら
                TackleLastStart();  //タックルを終了
            }
        }
    }

    //タックル中に壁に突っかかっているかどうか
    private bool IsTackleWallAvoid(Collision collision)
    {
        if(m_state.Value != State.Tackle) { //ステートがTackleでない場合は処理をしない。
            return false;
        }

        //対象が破壊できる壁ならfalse
        var typeDamage = collision.gameObject.GetComponent<TypeDamageDestroy>();
        if (typeDamage)
        {
            foreach (DamageType type in typeDamage.GetTakeDamageTypes())
            {
                if (type == DamageType.ObjectBreak)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(IsTackleWallAvoid(collision)) {  //タックル状態で
            TackleWallAvoid(collision);  //壁の突っかかり回避
        }

        //攻撃状態なら
        if (m_state.Value == State.Tackle || m_state.Value == State.TackleLast) {
            var damage = collision.gameObject.GetComponent<TakeDamageObject>();
            damage?.TakeDamage(GetBaseParam().damageData);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsTackleWallAvoid(collision)) {  //タックル状態で
            TackleWallAvoid(collision);  //壁の突っかかり回避
        }
    }

    //アクセッサ----------------------------------------

    public ReactiveProperty<State> state
    {
        get { return m_state; }
    }

}
