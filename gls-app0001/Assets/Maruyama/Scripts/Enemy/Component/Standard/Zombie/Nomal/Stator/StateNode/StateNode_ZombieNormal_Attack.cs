using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Attack : EnState_AttackBase
{
    TargetManager m_targetManager;
    Stator_ZombieNormal m_stator;

    public StateNode_ZombieNormal_Attack(EnemyBase owner)
        : base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
    }

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
        AddChangeComp(owner.GetComponent<AttackManager_ZombieNormal>(), true, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!m_targetManager.HasTarget())
        {
            m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        }
    }
}
