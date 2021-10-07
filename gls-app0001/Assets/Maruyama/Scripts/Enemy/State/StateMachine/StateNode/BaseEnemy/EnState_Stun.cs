using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_Stun : EnemyStateNodeBase<EnemyBase>
{
    EnemyStunManager m_stunMgr;
    EnemyVelocityMgr m_velocityMgr;

    public EnState_Stun(EnemyBase owner)
        : base(owner)
    {
        m_stunMgr = owner.GetComponent<EnemyStunManager>();
        m_velocityMgr = owner.GetComponent<EnemyVelocityMgr>();
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();

        m_stunMgr.StartStun();

        m_velocityMgr.ResetVelocity();
    }

    public override void OnUpdate()
    {

    }
}
