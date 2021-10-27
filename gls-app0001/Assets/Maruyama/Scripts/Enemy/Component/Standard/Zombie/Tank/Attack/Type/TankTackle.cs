﻿using System.Collections;
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
    float m_turningPower = 2.0f;
    [SerializeField]
    float m_subPursuitTargetForward = 0.7f; //ターゲットとのフォワードの差が0.7fより小さければ、予測タックルにする。

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

        Vector3 force = CalcuTackleForce();
        //var force = 

        m_velocityManager.AddForce(force);

        if (IsTackleEnd()) //ターゲットに限りなく近づいたら
        {
            AttackHitEnd();
        }
    }

    Vector3 CalcuTackleForce()
    {
        var target = m_targetManager.GetNowTarget();

        var velocity = m_velocityManager.velocity;
        var toVec = CalcuToTargetVec();

        var relativeHeading = Vector3.Dot(transform.forward, target.transform.forward);

        //前方にいて、かつ、自分と相手のフォワードの差が開いていなかったら、通常Seek
        //Dotは差が開いている程、値が小さくなる。
        if (Vector3.Dot(toVec, transform.forward) > 0 &&
            Mathf.Abs(relativeHeading) > 0.7f)
        {
            //return Vector3.zero;
            return CalcuVelocity.CalucSeekVec(velocity, toVec, m_tackleSpeed);
        }
        else
        {//フォワードの差が開いていたら、予測Seek
            return CalcuVelocity.CalcuPursuitForce(velocity, toVec, m_tackleSpeed, gameObject, target.GetComponent<Rigidbody>(), 2.0f);
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
