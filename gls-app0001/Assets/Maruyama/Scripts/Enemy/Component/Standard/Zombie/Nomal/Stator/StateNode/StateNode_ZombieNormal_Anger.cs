using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Anger : EnState_Anger
{
    StatusManager_ZombieNormal m_stator;

    public StateNode_ZombieNormal_Anger(EnemyBase owner)
        : base(owner)
    {
        m_stator = owner.GetComponent<StatusManager_ZombieNormal>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_stator.SetIsAnger(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
