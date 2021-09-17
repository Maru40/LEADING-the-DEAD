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
    Rigidbody m_rigid;

    public LinerSeekTarget(EnemyBase owner)
        : this(owner,3.0f)
    { }

    public LinerSeekTarget(EnemyBase owner, float maxSpeed)
        : base(owner)
    {
        m_maxSpeed = maxSpeed;

        m_targetMgr = owner.GetComponent<TargetMgr>();
        m_rigid = owner.GetComponent<Rigidbody>();
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

        Vector3 force = UtilityVelocity.CalucSeekVec(m_rigid.velocity, toVec, m_maxSpeed);

        m_rigid.AddForce(force);
    }


    //アクセッサ-----------------------------------------------------------------------------

    public void SetMaxSpeed(float speed){
        m_maxSpeed = speed;
    }
}
