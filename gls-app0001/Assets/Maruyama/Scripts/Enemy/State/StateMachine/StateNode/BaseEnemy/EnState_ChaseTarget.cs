using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_ChaseTarget : EnemyStateNodeBase<EnemyBase>
{

    public EnState_ChaseTarget(EnemyBase owner)
        : base(owner)
    { }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        Debug.Log("ChaseTarget");
    }

    public override void OnExit()
    {

    }
}
