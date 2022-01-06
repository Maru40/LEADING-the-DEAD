using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_Stun : EnemyStateNodeBase<EnemyBase>
{
    private EnemyStunManager m_stunMgr;
    private EnemyVelocityManager m_velocityMgr;
    private ThrongManager m_throngMgr;

    public EnState_Stun(EnemyBase owner)
        : base(owner)
    {
        m_stunMgr = owner.GetComponent<EnemyStunManager>();
        m_velocityMgr = owner.GetComponent<EnemyVelocityManager>();
        m_throngMgr = owner.GetComponent<ThrongManager>();
    }

    protected override void ReserveChangeComponents()
    {
        AddChangeComp(m_throngMgr, false, true);
    }

    public override void OnStart()
    {
        base.OnStart();

        m_stunMgr.StartStun();

        m_velocityMgr.ResetVelocity();
        m_velocityMgr.ResetForce();
    }

    public override void OnUpdate()
    {

    }
}
