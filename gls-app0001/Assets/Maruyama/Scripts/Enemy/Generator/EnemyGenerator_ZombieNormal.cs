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
    [Header("リスポーン管理")]
    public RespawnManagerParametor respawnParametor = new RespawnManagerParametor(true, 0.0f);
    [Header("追従")]
    public ChaseTargetParametor chaseParametor = new ChaseTargetParametor(0.75f, 3.0f, 3.0f, 10.0f, 3.0f);
    [Header("徘徊時")]
    public RandomPlowlingMove.Parametor randomPlowlingParametor = new RandomPlowlingMove.Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);
    [Header("ステータス")]
    public StatusManager_ZombieNormal.Status status = new StatusManager_ZombieNormal.Status(1.0f, 3.0f);
    [Header("攻撃力など")]
    public AttackParametorBase attackParametor = new AttackParametorBase(new AttributeObject.DamageData(2.0f), 2.5f);

    public CreateSetParametor_ZombieNormal()
    {

    }

    public CreateSetParametor_ZombieNormal(float allInit)
    {
        respawnParametor = new RespawnManagerParametor(true, allInit);
        chaseParametor = new ChaseTargetParametor(allInit, allInit, allInit, allInit, allInit);
        randomPlowlingParametor = new RandomPlowlingMove.Parametor(allInit, allInit, allInit, allInit, allInit, allInit);
        status = new StatusManager_ZombieNormal.Status(allInit, allInit);
        attackParametor = new AttackParametorBase(new AttributeObject.DamageData(allInit), allInit);
    }
}

public class EnemyGenerator_ZombieNormal : EnemyGenerator
{
    [Header("生成時にセットするパラメータ"),SerializeField]
    CreateSetParametor_ZombieNormal m_createSetParam = new CreateSetParametor_ZombieNormal();

    [Header("リスポーン時にバフを掛けるパラメータ"), SerializeField]
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

        var attack = obj.GetComponent<AttackManager_ZombieNormal>();
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
