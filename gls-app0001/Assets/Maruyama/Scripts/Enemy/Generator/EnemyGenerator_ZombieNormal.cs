using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// 生成時にセットするパラメータ群
/// </summary>
[Serializable]
public class CreateSetParametor_ZombieNormal
{
    public RespawnManagerParametor respawnParametor = new RespawnManagerParametor(true, 0.0f);
    public ChaseTargetParametor chaseParametor = new ChaseTargetParametor(0.75f, 3.0f, 3.0f, 10.0f, 3.0f);
    public RandomPlowlingMove.Parametor randomPlowlingParametor = new RandomPlowlingMove.Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);
    public StatusManager_ZombieNormal.Status status = new StatusManager_ZombieNormal.Status(1.0f, 3.0f);
    public AttackParametorBase attackParametor = new AttackParametorBase(10.0f, 2.5f);

    public CreateSetParametor_ZombieNormal()
    {

    }

    public CreateSetParametor_ZombieNormal(float allInit)
    {
        respawnParametor = new RespawnManagerParametor(true, allInit);
        chaseParametor = new ChaseTargetParametor(allInit, allInit, allInit, allInit, allInit);
        randomPlowlingParametor = new RandomPlowlingMove.Parametor(allInit, allInit, allInit, allInit, allInit, allInit);
        status = new StatusManager_ZombieNormal.Status(allInit, allInit);
        attackParametor = new AttackParametorBase(allInit, allInit);
    }
}

public class EnemyGenerator_ZombieNormal : EnemyGenerator
{
    [SerializeField]
    CreateSetParametor_ZombieNormal m_createSetParam = new CreateSetParametor_ZombieNormal();

    [SerializeField]
    CreateSetParametor_ZombieNormal m_respawnStatusUpParam= new CreateSetParametor_ZombieNormal(0.0f);

    protected override void Start()
    {
        base.Start();
    }

    protected override void CreateObjectAdjust(GameObject obj)
    {
        var respawn = obj.GetComponent<EnemyRespawnManager>();
        if (respawn)
        {
            respawn.SetParametor(m_createSetParam.respawnParametor);
        }

        var chase = obj.GetComponent<ChaseTarget>();
        if (chase)
        {
            chase.SetParametor(m_createSetParam.chaseParametor);
        }

        var randomPlowling = obj.GetComponent<RandomPlowlingMove>();
        if (randomPlowling)
        {
            randomPlowling.SetParametor(m_createSetParam.randomPlowlingParametor);
        }

        var statusMgr = obj.GetComponent<StatusManager_ZombieNormal>();
        if (statusMgr)
        {
            statusMgr.SetStatus(m_createSetParam.status);
        }

        var attack = obj.GetComponent<Attack_ZombieNormal>();
        if (attack)
        {
            attack.SetBaseParam(m_createSetParam.attackParametor);
        }

        var statusUp = obj.GetComponent<RespawnStatusUp_ZonbieNormal>();
        if (statusUp)
        {
            statusUp.SetParametor(m_respawnStatusUpParam);
        }
    }
}
