using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnStatusUp_ZonbieNormal : EnemyRespawnStatusUpBase
{
    [SerializeField]
    CreateSetParametor_ZombieNormal m_param = new CreateSetParametor_ZombieNormal(0.0f);

    protected override void StatusUp()
    {
        var respawn = GetComponent<EnemyRespawnManager>();
        if (respawn)
        {
            respawn.AddParametor(m_param.respawnParametor);
        }

        var chase = GetComponent<ChaseTarget>();
        if (chase)
        {
            chase.AddParametor(m_param.chaseParametor);
        }

        var randomPlowling = GetComponent<RandomPlowlingMove>();
        if (randomPlowling)
        {
            randomPlowling.AddParametor(m_param.randomPlowlingParametor);
        }

        var statusMgr = GetComponent<StatusManager_ZombieNormal>();
        if (statusMgr)
        {
            statusMgr.AddStatus(m_param.status);
        }

        var attack = GetComponent<Attack_ZombieNormal>();
        if (attack)
        {
            attack.AddBaseParam(m_param.attackParametor);
        }
    }

    public override void Respawn()
    {
        base.Respawn();

        StatusUp();
    }

    public void SetParametor(CreateSetParametor_ZombieNormal param)
    {
        m_param = param;
    }
    public CreateSetParametor_ZombieNormal GetParametor()
    {
        return m_param;
    }
}
