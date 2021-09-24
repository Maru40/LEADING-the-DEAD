using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class BreadSeekTarget : NodeBase<EnemyBase>
{ 
    float m_nearRange = 3.0f;
    float m_maxSpeed = 3.0f;
    float m_turningPower = 1.0f; //旋回する力
    float m_lostSeekTime = 10.0f;
    Vector3 m_targetPosition = new Vector3();

    //コンポ―ネント系----------------------------------

    ChaseTarget m_chaseTarget;
    WaitTimer m_waitTimer;
    Rigidbody m_rigid;
    EnemyVelocityMgr m_velocityMgr;
    TargetManager m_targetMgr;
    BreadCrumb m_bread;
    ThrongManager m_throngMgr;

    public BreadSeekTarget(EnemyBase owner, float nearRange, float maxSpeed, float turningPower, float lostSeekTime)
        : base(owner)
    {
        m_nearRange = nearRange;
        m_maxSpeed = maxSpeed;
        m_turningPower = turningPower;
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
        m_velocityMgr = owner.GetComponent<EnemyVelocityMgr>();

        m_targetMgr = owner.GetComponent<TargetManager>();
        var target = m_targetMgr.GetNowTarget();

        m_bread = target?.GetComponent<BreadCrumb>();

        if (m_bread){
            //初期ポジションのセット
            var position = m_bread.GetNewBackPosition(1); //最新の一個前を取得
            if(position != null){
                m_targetPosition = (Vector3)position;
            }
            else{  //もしなかったら最新を取得
                m_targetPosition = m_bread.GetNewPosition();
            }
        }

        m_throngMgr = owner.GetComponent<ThrongManager>();
    }

    public override void OnUpdate()
    {
        Debug.Log("BreadSeek");
        if (!m_bread){
            return;
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
        m_throngMgr.AvoidNearThrong(m_velocityMgr, toVec, m_maxSpeed, m_turningPower);

        //var force = UtilityVelocity.CalucSeekVec(m_rigid.velocity, toVec, m_maxSpeed);
        //m_rigid.AddForce(force);

        //目的地に到達したら
        if (Calculation.IsArrivalPosition(m_nearRange, GetOwner().transform.position, m_targetPosition)) {
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
