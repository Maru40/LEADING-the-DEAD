using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UtilCalcu = UtilityCalculation;

public class BreadSeekTarget : NodeBase<EnemyBase>
{ 
    float m_maxSpeed = 3.0f;
    float m_nearRange = 3.0f;
    float m_lostSeekTime = 10.0f;
    Vector3 m_targetPosition = new Vector3();

    //コンポ―ネント系----------------------------------

    ChaseTarget m_chaseTarget;
    WaitTimer m_waitTimer;
    Rigidbody m_rigid;
    TargetMgr m_targetMgr;
    BreadCrumb m_bread;

    public BreadSeekTarget(EnemyBase owner, float nearRange, float maxSpeed, float lostSeekTime)
        : base(owner)
    {
        m_nearRange = nearRange;
        m_maxSpeed = maxSpeed;
        m_lostSeekTime = lostSeekTime;
    }

    public override void OnStart()
    {
        var owner = GetOwner();

        m_chaseTarget = owner.GetComponent<ChaseTarget>();
        //WaitTimerで一定時間見失ったら待機状態に移行することにする。
        m_waitTimer = owner.GetComponent<WaitTimer>();
        m_waitTimer.AddWaitTimer(GetType(), m_lostSeekTime, m_chaseTarget.TargetLost);

        m_rigid = owner.GetComponent<Rigidbody>();

        m_targetMgr = owner.GetComponent<TargetMgr>();
        var target = m_targetMgr.GetNowTarget();

        m_bread = target.GetComponent<BreadCrumb>();

        if (m_bread){
            m_targetPosition = m_bread.GetNewPosition();
        }
    }

    public override void OnUpdate()
    {
        if (!m_bread){
            m_chaseTarget.TargetLost();
        }

        UpdateMove();
    }

    public override void OnExit()
    {
        m_waitTimer.AbsoluteEndTimer(GetType(), false);
    }

    void UpdateMove()
    {
        var toVec = m_targetPosition - GetOwner().transform.position;
        var force = UtilityVelocity.CalucSeekVec(m_rigid.velocity, toVec, m_maxSpeed);
        m_rigid.AddForce(force);

        //目的地に到達したら
        if (UtilCalcu.IsArrivalPosition(m_nearRange, GetOwner().transform.position, m_targetPosition)) {
            NextRoute();
        }
    }

    void NextRoute()
    {
        var newPosition = m_bread.GetNextPosition(m_targetPosition);

        if(newPosition != null){
            m_targetPosition = (Vector3)newPosition;
        }
        else{
            m_targetPosition = m_bread.GetNewPosition();
        }
    }


    //アクセッサ------------------------------------------------------

    public void SetMaxSpeed(float maxSpeed)
    {
        m_maxSpeed = maxSpeed;
    }

    public void SetNearRange(float nearRange){
        m_nearRange = nearRange;
    }

    public void SetLostSeekTime(float seekTime){
        m_lostSeekTime = seekTime;
    }
}
