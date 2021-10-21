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

    ReactiveProperty<State> m_state = new ReactiveProperty<State>(State.None);
    Vector3 m_tackleDirect = new Vector3();

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    Stator_ZombieTank m_stator;
    EyeSearchRange m_eye;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_eye = GetComponent<EyeSearchRange>();

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
        var toVec = target.gameObject.transform.position - transform.position;
        return toVec;
    }

    void TackleMove()
    {
        if (m_state.Value != State.Tackle) {  //Tackleでなかったら処理をしない
            return;
        }

        var velocity = m_velocityManager.velocity;
        var toVec = CalcuToTargetVec();

        var force = CalcuVelocity.CalucSeekVec(velocity, toVec, m_tackleSpeed);
        m_velocityManager.AddForce(force);

        if (IsTackleEnd()) //ターゲットに限りなく近づいたら
        {
            AttackHitEnd();
        }
    }

    void Rotation()
    {
        if(m_velocityManager.velocity == Vector3.zero) {
            return;
        }

        if (m_state.Value == State.Charge || m_state.Value == State.Tackle) {
            transform.forward = m_velocityManager.velocity;
        }
    }

    public override void AttackStart()
    {
        //m_velocityManager.ResetVelocity();
        //m_velocityManager.ResetForce();

        m_state.Value = State.Charge;
        enabled = true;
    }

    public override void AttackHitStart()
    {
        m_state.Value = State.Tackle;

        //タックルする方向を決める
        var target = m_targetManager.GetNowTarget();
        var toVec = target.gameObject.transform.position - transform.position;
        toVec.y = 0;
        m_tackleDirect = toVec;
    }

    public override void AttackHitEnd()
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
    /// タックルの終了条件
    /// </summary>
    /// <returns></returns>
    bool IsTackleEnd()
    {
        if (IsTargetRange(m_tackleLastRange)) {
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

    private void OnCollisionEnter(Collision collision)
    {
        //攻撃状態なら
        if (m_state.Value == State.Tackle || m_state.Value == State.TackleLast) {
            var damage = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();
            damage?.TakeDamage(GetBaseParam().damageData);
        }
    }


    //アクセッサ----------------------------------------

    public ReactiveProperty<State> state
    {
        get { return m_state; }
    }

}
