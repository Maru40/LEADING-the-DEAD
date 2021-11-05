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
    EnemyRotationCtrl m_rotationCtrl;
    StatusManagerBase m_statusManager;
    EyeSearchRange m_eye;

    public BreadSeekTarget(EnemyBase owner, float nearRange, float maxSpeed, float turningPower, float lostSeekTime)
        : base(owner)
    {
        m_nearRange = nearRange;
        m_maxSpeed = maxSpeed;
        m_turningPower = turningPower;
        m_lostSeekTime = lostSeekTime;

        m_statusManager = owner.GetComponent<StatusManagerBase>();
        m_eye = owner.GetComponent<EyeSearchRange>();
        m_chaseTarget = owner.GetComponent<ChaseTarget>();
        m_waitTimer = owner.GetComponent<WaitTimer>();
        m_rigid = owner.GetComponent<Rigidbody>();
        m_velocityMgr = owner.GetComponent<EnemyVelocityMgr>();
        m_targetMgr = owner.GetComponent<TargetManager>();
    }

    public override void OnStart()
    {
        var owner = GetOwner();

        //WaitTimerで一定時間見失ったら待機状態に移行することにする。
        m_waitTimer.AddWaitTimer(GetType(), m_lostSeekTime, m_chaseTarget.TargetLost);

        var target = m_targetMgr.GetNowTarget();

        m_bread = target?.GetComponent<BreadCrumb>();

        if (m_bread){
            //初期ポジションのセット
            //var position = m_bread.GetNewBackPosition(1); //最新の一個前を取得
            var position = CalcuTargetPosition(m_bread);
            if (position != null){
                m_targetPosition = (Vector3)position;
                //Debug.Log("Player" + target.transform.position);
                //Debug.Log("Target" + m_targetPosition);
            }
            else{  //もしなかったら最新を取得
                //Debug.Log("ターゲットがNull");
                m_targetPosition = m_bread.GetNewPosition();
            }
        }

        m_throngMgr = owner.GetComponent<ThrongManager>();
        m_rotationCtrl = owner.GetComponent<EnemyRotationCtrl>();
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
        var maxSpeed = m_maxSpeed * m_statusManager.GetBuffParametor().SpeedBuffMultiply;
        Vector3 force = CalcuVelocity.CalucSeekVec(m_velocityMgr.velocity, toVec, maxSpeed);
        m_velocityMgr.AddForce(force * m_turningPower);

        m_rotationCtrl.SetDirect(m_velocityMgr.velocity);
        //m_throngMgr.AvoidNearThrong(m_velocityMgr, toVec, m_maxSpeed, m_turningPower);

        //目的地に到達したら
        if (Calculation.IsArrivalPosition(m_nearRange, GetOwner().transform.position, m_targetPosition, true)) {
            NextRoute();
        }
    }

    void NextRoute()
    {
        var newPosition = m_bread.GetNextPosition(m_targetPosition);

        if(newPosition != null){
            m_targetPosition = (Vector3)newPosition;
            //Debug.Log("NextPosition" + m_targetPosition);
        }
        else{
            m_targetPosition = m_bread.GetNewPosition();
        }
    }

    //Rayの及ばない場所の取得
    Vector3? CalcuTargetPosition(BreadCrumb bread)
    {
        var positions = bread.GetCopyPositions();

        //最新のポジションから参照
        for(int i = positions.Count - 1; i > 0; i--)
        {
            //視界内のポジションを取得
            if (m_eye.IsInEyeRange(positions[i]))
            {
                return positions[i];
            }
        }

        return null;
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
