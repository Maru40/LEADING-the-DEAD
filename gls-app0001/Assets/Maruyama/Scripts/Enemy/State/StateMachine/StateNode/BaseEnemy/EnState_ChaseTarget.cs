using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class EnState_ChaseTarget : EnemyStateNodeBase<EnemyBase>
{
    AttackBase m_attackComp;
    Stator_ZombieNormal m_stator;

    public EnState_ChaseTarget(EnemyBase owner)
        : base(owner)
    { }

    public override void OnStart()
    {
        var owner = GetOwner();

        m_attackComp = owner.GetComponent<AttackBase>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();

        var chaseTarget = owner.GetComponent<ChaseTarget>();

        AddChangeComp(chaseTarget, true, false);

        ChangeComps(EnableChangeType.Start);

        //èWícçsìÆê›íË
        var throngMgr = owner.GetComponent<ThrongManager>();
        if (chaseTarget && throngMgr)
        {
            float range = chaseTarget.GetInThrongRange();
            throngMgr.SetInThrongRange(range);
        }
    }

    public override void OnUpdate()
    {
        if (m_attackComp.IsAttackStartRange())
        {
            m_stator.GetTransitionMember().attackTrigger.Fire();
        }

        //Debug.Log("ChaseTarget");
    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }
}
