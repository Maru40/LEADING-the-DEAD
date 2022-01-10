using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieTank_None : EnemyStateNodeBase<EnemyBase>
{
    public StateNode_ZombieTank_None(EnemyBase owner)
        :base(owner)
    { }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<RandomPlowlingMove>(), false, true);
    }

    public override void OnUpdate()
    {

    }
}
