using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombiNormal_Death : EnemyStateNodeBase<EnemyBase>
{
    EnemyRespawnManager m_respawnManager;

    public StateNode_ZombiNormal_Death(EnemyBase owner)
        :base(owner)
    {
        m_respawnManager = owner.GetComponent<EnemyRespawnManager>();
    }

    protected override void ReserveChangeComponents()
    {
        //特になし
    }

    public override void OnStart()
    {
        base.OnStart();

        m_respawnManager.RespawnReserve();
    }

    public override void OnUpdate()
    {

    }
}
