using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieTank_Attack : EnState_AttackBase
{
    public StateNode_ZombieTank_Attack(EnemyBase owner)
        : base(owner)
    { }

    protected override void PlayStartAnimation()
    {
        //攻撃アニメーション再生
        //var animatorCtrl = GetOwner().GetComponent<AnimatorCtrl_ZombieTank>();
        //animatorCtrl.AttackTriggerFire();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<Attack_ZombieTank>(), true, false);
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, true);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        Debug.Log("Attack");
    }
}
