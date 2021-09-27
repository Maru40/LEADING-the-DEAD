using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator_ZombieNormal : EnemyGenerator
{
    [SerializeField]
    RespawnManagerParametor m_respawnParametor = new RespawnManagerParametor(true, 0.0f);

    [SerializeField]
    ChaseTargetParametor m_chaseParametor = new ChaseTargetParametor(0.75f, 3.0f, 3.0f, 10.0f, 3.0f);

    [SerializeField]
    RandomPlowlingMove.Parametor m_randomPlowlingParametor = new RandomPlowlingMove.Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);

    [SerializeField]
    StatusManager_ZombieNormal.Status m_status = new StatusManager_ZombieNormal.Status(1.0f, 3.0f);

    [SerializeField]
    AttackParametorBase m_attackParametor = new AttackParametorBase(10.0f, 1.0f, 3.0f);

    protected override void Start()
    {
        base.Start();
    }

    protected override void CreateObjectAdjust(GameObject obj)
    {
        var respawn = obj.GetComponent<EnemyRespawnManager>();
        if (respawn)
        {
            respawn.SetParametor(m_respawnParametor);
        }

        var chase = obj.GetComponent<ChaseTarget>();
        if (chase)
        {
            chase.SetPamametor(m_chaseParametor);
        }

        var randomPlowling = obj.GetComponent<RandomPlowlingMove>();
        if (randomPlowling)
        {
            randomPlowling.SetParametor(m_randomPlowlingParametor);
        }

        var statusMgr = obj.GetComponent<StatusManager_ZombieNormal>();
        if (statusMgr)
        {
            statusMgr.SetStatus(m_status);
        }

        var attack = obj.GetComponent<Attack_ZombieNormal>();
        if (attack)
        {
            attack.SetBaseParam(m_attackParametor);
        }
    }
}
