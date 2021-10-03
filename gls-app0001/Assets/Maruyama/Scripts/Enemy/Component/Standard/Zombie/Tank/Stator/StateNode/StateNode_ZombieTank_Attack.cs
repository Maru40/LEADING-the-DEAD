﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieTank_Attack : EnState_AttackBase
{
    public StateNode_ZombieTank_Attack(EnemyBase owner)
        : base(owner)
    { }

    protected override void PlayStartAnimation()
    {
        
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<Attack_ZombieTank>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();
    }
}
