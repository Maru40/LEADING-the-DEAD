using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Anger : EnState_Anger
{
    private AngerManager m_angerManager;
    private AnimatorCtrl_ZombieNormal m_animatorCtrl;

    public StateNode_ZombieNormal_Anger(EnemyBase owner)
        : base(owner)
    {
        m_angerManager = owner.GetComponent<AngerManager>();
        //m_animatorCtrl = owner.GetComponent<AnimatorCtrl_ZombieNormal>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<ChaseTarget>(), false, true);
        AddChangeComp(owner.GetComponent<ThrongManager>(), false, true);
    }

    public override void OnStart()
    {
        base.OnStart();

        m_angerManager.StartAnger();
        //m_animatorCtrl.StartAnger();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
