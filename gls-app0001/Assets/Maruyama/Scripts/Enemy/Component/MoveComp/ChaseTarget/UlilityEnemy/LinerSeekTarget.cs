using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

using FoundType = FoundObject.FoundType;

/// <summary>
/// 直線的なターゲット追従
/// </summary>
public class LinerSeekTarget : NodeBase<EnemyBase>
{
    float m_maxSpeed = 3.0f;
    float m_turningPower = 1.0f; //旋回する力

    ChaseTarget m_chaseTarget;
    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityMgr;
    ThrongManager m_throngMgr;
    EnemyRotationCtrl m_rotationCtrl;
    StatusManagerBase m_statusManager;

    public LinerSeekTarget(EnemyBase owner)
        : this(owner,3.0f, 1.0f)
    { }

    public LinerSeekTarget(EnemyBase owner, float maxSpeed, float turningPower)
        : base(owner)
    {
        m_maxSpeed = maxSpeed;
        m_turningPower = turningPower;

        m_chaseTarget = owner.GetComponent<ChaseTarget>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityMgr = owner.GetComponent<EnemyVelocityMgr>();
        m_throngMgr = owner.GetComponent<ThrongManager>();
        m_rotationCtrl = owner.GetComponent<EnemyRotationCtrl>();
        m_statusManager = owner.GetComponent<StatusManagerBase>();
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        UpdateMove();
    }

    public override void OnExit()
    {

    }


    void UpdateMove()
    {
        Debug.Log("LinerTargret");

        FoundObject target = m_targetManager.GetNowTarget();
        var position = m_targetManager.GetNowTargetPosition();
        if(position != null)
        {
            Move((Vector3)position);
        }
        else
        {
            m_chaseTarget.TargetLost();
        }
    }

    //ターゲットを追従する処理
    void LinerTarget(FoundObject target)
    {
        Move(target.transform.position);
    }

    //見失った場所を探す。
    void LinerLostPosition()
    {
        var lostPosition = m_targetManager.GetLostPosition();
        if (lostPosition == null)
        {
            m_chaseTarget.TargetLost();
            return;
        }

        Move((Vector3)lostPosition);

        Debug.Log("見失った―追う");
    }

    void Move(Vector3 targetPosition)
    {
        Vector3 toVec = targetPosition - GetOwner().transform.position;
        float maxSpeed = m_maxSpeed * m_statusManager.GetBuffParametor().SpeedBuffMultiply;
        //Vector3 force = CalcuVelocity.CalucSeekVec(m_velocityMgr.velocity, toVec, maxSpeed);
        var type = (FoundType)m_targetManager.GetNowTargetType();
        var force = type switch {
            FoundType.Smell => CalcuVelocity.CalucArriveVec(m_velocityMgr.velocity, toVec, maxSpeed),
            _ => CalcuVelocity.CalucSeekVec(m_velocityMgr.velocity, toVec, maxSpeed),
        };

        m_velocityMgr.AddForce(force * m_turningPower);

        m_rotationCtrl.SetDirect(m_velocityMgr.velocity);
    }

    //アクセッサ-----------------------------------------------------------------------------

    public void SetMaxSpeed(float speed){
        m_maxSpeed = speed;
    }
}
