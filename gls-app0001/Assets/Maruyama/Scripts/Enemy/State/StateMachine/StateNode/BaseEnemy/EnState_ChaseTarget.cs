﻿using System.Collections;
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

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ChaseTarget>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        var owner = GetOwner();

        m_attackComp = owner.GetComponent<AttackBase>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();

        var chaseTarget = owner.GetComponent<ChaseTarget>();

        //集団行動設定
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
        base.OnExit();
    }
}
