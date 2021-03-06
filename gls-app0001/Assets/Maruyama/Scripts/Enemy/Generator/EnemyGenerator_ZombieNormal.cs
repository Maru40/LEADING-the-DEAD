using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using AttributeObject;

using MaruUtility;
using MaruUtility.UtilityDictionary;

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
    [Header("それぞれのステートのパラメータ")]
    public Stator_ZombieNormal.Parametor statorParametor = new Stator_ZombieNormal.Parametor();
    [Header("通常攻撃の攻撃力パラメータ")]
    public Ex_Dictionary<AnimatorManager_ZombieNormal.NormalAttackHitColliderType, DamageData> normalAttackHitBoxDictionary = 
        new Ex_Dictionary<AnimatorManager_ZombieNormal.NormalAttackHitColliderType, DamageData>();
    [Header("怒り状態のバフパラメータ")]
    public AngerManager.RiseParametor angerBuffParametor = new AngerManager.RiseParametor(1.05f, 1.02f, 1.5f);
    [Header("ターゲットのバフパラメータ")]
    public TargetManager.BuffParametor targetBuffParametor = new TargetManager.BuffParametor(3.0f);
    [Header("目線のパラメータ")]
    public EyeSearchRangeParam eyeSarchRangeParam = new EyeSearchRangeParam(7.0f, 3.0f, 0.7f);
    [Header("群衆パラメータ")]
    public ThrongManagerParametor throngParam = new ThrongManagerParametor(1.0f, 1.0f, 3.0f);
    [Header("食べる力")]
    public EatParametor eatParam = new EatParametor(1.0f);
    [Header("オーラパラメータ")]
    public AuraManager.Parametor auraParam = new AuraManager.Parametor();

    public CreateSetParametor_ZombieNormal()
    { }

    public CreateSetParametor_ZombieNormal(float allInit)
    {
        respawnParametor = new RespawnManagerParametor(true, allInit);
        chaseParametor = new ChaseTargetParametor(allInit, allInit, allInit, allInit, allInit);
        randomPlowlingParametor = new RandomPlowlingMove.Parametor(allInit, allInit, allInit, allInit, allInit, allInit);
        status = new StatusManager_ZombieNormal.Status(allInit, allInit);
    }
}

/// <summary>
/// ランダムパラメータにする
/// </summary>
[Serializable]
public class CreateSetRandomParametor_ZombieNormal
{
    [Header("追従")]
    public RandomRange<ChaseTargetParametor> cahseParametor = new RandomRange<ChaseTargetParametor>();
    [Header("徘徊時")]
    public RandomRange<RandomPlowlingMove.Parametor> randomPlowlingParametor = new RandomRange<RandomPlowlingMove.Parametor>();
    [Header("攻撃開始距離")]
    public RandomRange<AttackParametorBase> attackManagerParametor = new RandomRange<AttackParametorBase>();
    [Header("通常攻撃のパラメータ")]
    public RandomRange<NormalAttack.Parametor> normalAttackParametor = new RandomRange<NormalAttack.Parametor>(); 
}

public class EnemyGenerator_ZombieNormal : RandomPositionEnemyGenerator
{
    [Header("生成時にセットするパラメータ"),SerializeField]
    private CreateSetParametor_ZombieNormal m_createSetParam = new CreateSetParametor_ZombieNormal();

    [Header("リスポーン時にバフを掛けるパラメータ"), SerializeField]
    private CreateSetParametor_ZombieNormal m_respawnStatusUpParam = new CreateSetParametor_ZombieNormal(0.0f);

    [Header("ランダムに設定したいパラメータの設定"), SerializeField]
    private CreateSetRandomParametor_ZombieNormal m_randomSetParam = new CreateSetRandomParametor_ZombieNormal();

    protected override void Start()
    {
        base.Start();
    }

    protected override void CreateObjectAdjust(GameObject obj)
    {
        base.CreateObjectAdjust(obj);

        CreateSetRandomParametor();

        CreateSetParametor(obj);  //パラメータの生成時に設定
    }

    private void CreateSetParametor(GameObject obj)
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

        //通常攻撃力
        var animatorManager = obj.GetComponent<AnimatorManager_ZombieNormal>();
        if (animatorManager)
        {
            m_createSetParam.normalAttackHitBoxDictionary.InsertInspectorData();
            foreach (var data in m_createSetParam.normalAttackHitBoxDictionary)
            {
                Debug.Log(data.Key + ": " + data.Value);
                animatorManager.SetNormalAttackDamageData(data.Key, data.Value);
            }

            animatorManager.EatParam = m_createSetParam.eatParam;
        }

        //怒りバフ
        var angerManager = obj.GetComponent<AngerManager>();
        if (angerManager)
        {
            angerManager.SetRiseParametor(m_createSetParam.angerBuffParametor);
        }

        //ターゲットバフ
        var targetManager = obj.GetComponent<TargetManager>();
        if (targetManager)
        {
            targetManager.buffParametor = m_createSetParam.targetBuffParametor;
        }

        //目線パラメータ
        var eyeSearchRange = obj.GetComponent<EyeSearchRange>();
        if (eyeSearchRange)
        {
            eyeSearchRange.SetParam(m_createSetParam.eyeSarchRangeParam);
        }

        var statusUp = obj.GetComponent<RespawnStatusUp_ZonbieNormal>();
        if (statusUp)
        {
            statusUp.SetParametor(m_respawnStatusUpParam);
        }

        //集団行動
        var throngManager = obj.GetComponent<ThrongManager>();
        if (throngManager)
        {
            throngManager.SetParametor(m_createSetParam.throngParam);
        }

        //ステートの設定
        var stator = obj.GetComponent<Stator_ZombieNormal>();
        if (stator)
        {
            stator.parametor = m_createSetParam.statorParametor;
        }

        //オーラ設定
        var auraManager = obj.GetComponent<AuraManager>();
        if (auraManager)
        {
            auraManager.parametor = m_createSetParam.auraParam;
        }
    }

    private void CreateSetRandomParametor()
    {
        m_createSetParam.chaseParametor.Random(m_randomSetParam.cahseParametor);
        m_createSetParam.randomPlowlingParametor.Random(m_randomSetParam.randomPlowlingParametor);
    }
}
