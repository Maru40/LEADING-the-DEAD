using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直線的なターゲット追従
/// </summary>
public class LinerSeekTarget : UtilityEnemyBase
{
    float m_maxSpeed = 3.0f;

    TargetMgr m_targetMgr;
    Rigidbody m_rigid;

    public LinerSeekTarget(GameObject owner)
        : this(owner,3.0f)
    { }

    public LinerSeekTarget(GameObject owner, float maxSpeed)
        : base(owner)
    {
        m_maxSpeed = maxSpeed;

        m_targetMgr = owner.GetComponent<TargetMgr>();
        m_rigid = owner.GetComponent<Rigidbody>();
    }

    public void Move()
    {
        GameObject target = m_targetMgr.GetNowTarget();
        Vector3 toVec = target.transform.position - GetOwner().transform.position;

        Vector3 force = UtilityVelocity.CalucSeekVec(m_rigid.velocity, toVec, m_maxSpeed);

        m_rigid.AddForce(force);
    }
}
