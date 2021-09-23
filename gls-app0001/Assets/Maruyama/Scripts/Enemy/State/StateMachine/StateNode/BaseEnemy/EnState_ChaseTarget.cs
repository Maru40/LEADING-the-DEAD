using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class EnState_ChaseTarget : EnemyStateNodeBase<EnemyBase>
{

    public EnState_ChaseTarget(EnemyBase owner)
        : base(owner)
    { }

    public override void OnStart()
    {
        var owner = GetOwner();
        var chaseTarget = owner.GetComponent<ChaseTarget>();

        AddChangeComp(chaseTarget, true, false);

        ChangeComps(EnableChangeType.Start);

        //èWícçsìÆê›íË
        var throngMgr = owner.GetComponent<ThrongMgr>();
        if (chaseTarget && throngMgr)
        {
            float range = chaseTarget.GetInThrongRange();
            throngMgr.SetInThrongRange(range);
        }
    }

    public override void OnUpdate()
    {
        //Debug.Log("ChaseTarget");
    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }
}
