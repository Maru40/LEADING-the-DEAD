using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Attack : EnState_AttackBase
{
    public StateNode_ZombieNormal_Attack(EnemyBase owner)
        : base(owner)
    { }

    protected override void PlayStartAnimation()
    {
        //攻撃アニメーション再生
        //var animatorCtrl = GetOwner().GetComponent<AnimatorCtrl_ZombieNormal>();
        //animatorCtrl.AttackTriggerFire();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<Attack_ZombieNormal>(), true, false);
    }
}
