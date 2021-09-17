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

        AddChangeComp(owner.GetComponent<ChaseTarget>(), true, false);

        ChangeComps(EnableChangeType.Start);
    }

    public override void OnUpdate()
    {
        Debug.Log("ChaseTarget");
    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }
}
