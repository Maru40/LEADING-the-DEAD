using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直線的なターゲット追従
/// </summary>
public class LinerSeekTarget : NodeBase<EnemyBase>
{
    float m_maxSpeed = 3.0f;

    TargetMgr m_targetMgr;
    EnemyVelocityMgr m_velocityMgr;
    ThrongMgr m_throngMgr;

    public LinerSeekTarget(EnemyBase owner)
        : this(owner,3.0f)
    { }

    public LinerSeekTarget(EnemyBase owner, float maxSpeed)
        : base(owner)
    {
        m_maxSpeed = maxSpeed;

        m_targetMgr = owner.GetComponent<TargetMgr>();
        m_velocityMgr = owner.GetComponent<EnemyVelocityMgr>();
        m_throngMgr = owner.GetComponent<ThrongMgr>();
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
        GameObject target = m_targetMgr.GetNowTarget();
        Vector3 toVec = target.transform.position - GetOwner().transform.position;
        m_throngMgr.AvoidNearThrong(m_velocityMgr, toVec, m_maxSpeed);
        //toVec += m_throngMgr.CalcuThrongVector();

        //Vector3 force = UtilityVelocity.CalucSeekVec(m_rigid.velocity, toVec, m_maxSpeed);

        //m_rigid.AddForce(force);

        //test
        //m_throngMgr.AvoidNearThrong(m_rigid, toVec, m_maxSpeed);

        //var newVector = m_throngMgr.CalcuThrongVector();
        //if(newVector != Vector3.zero)
        //{
        //    force = UtilityVelocity.CalucSeekVec(m_rigid.velocity, m_throngMgr.CalcuThrongVector(), m_maxSpeed);
        //    m_rigid.AddForce(force);
        //}
    }


    //アクセッサ-----------------------------------------------------------------------------

    public void SetMaxSpeed(float speed){
        m_maxSpeed = speed;
    }
}
