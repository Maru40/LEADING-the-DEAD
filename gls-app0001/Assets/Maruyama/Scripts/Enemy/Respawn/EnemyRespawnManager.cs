using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using System;

[Serializable]
public struct RespawnManagerParametor
{
    public bool isAbsoluteRespawn;  //強制リスポーン
    public bool isRespawn;
    public float time;

    public RespawnManagerParametor(bool isRespawn, float time)
    {
        this.isAbsoluteRespawn = false;
        this.isRespawn = isRespawn;
        this.time = time;
    }
}

/// <summary>
/// ターゲットの範囲外にリスポーンする処理
/// </summary>
public class EnemyRespawnManager : EnemyRespawnBase
{
    [SerializeField]
    RespawnManagerParametor m_param = new RespawnManagerParametor(true, 0.0f);

    [SerializeField]
    EnemyGenerator m_generator = null;

    StatusManagerBase m_statusManager;
    StatorBase m_stator;
    WaitTimer m_waitTimer;
    EnemyRespawnStatusUpBase m_statusUp;
    DropObjecptManager m_dropManager;
    TargetManager m_targetManger;
    AngerManager m_angerManager;

    void Awake()
    {
        //StartTargetNullCheck();
        StartGeneratorNullCheck();

        m_statusManager = GetComponent<StatusManagerBase>();
        m_stator = GetComponent<StatorBase>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_statusUp = GetComponent<EnemyRespawnStatusUpBase>();
        m_dropManager = GetComponent<DropObjecptManager>();
        m_targetManger = GetComponent<TargetManager>();
        m_angerManager = GetComponent<AngerManager>();
    }

    //リスポーン準備
    public void RespawnReserve()
    {
        //リスポーンするなら準備をする。
        if (IsRespawn())
        {
            //使いまわすため、削除せずにリスポーンポイントに設定する。
            gameObject.transform.position = new Vector3(0.0f, -100.0f, 0.0f);

            m_waitTimer.AddWaitTimer(GetType(), m_param.time, Respawn);
        }
        else
        {
            m_generator.AddDeathCount();
            gameObject.SetActive(false);
        }
    }

    void Respawn()
    {
        //if(m_target == null || m_generator == null) {
        if (m_generator == null) { 
            //StartTargetNullCheck();
            StartGeneratorNullCheck();
        }

        var respawnPosition = CalcuRespawnRandomPosition();
        transform.position = respawnPosition;

        //DeathCount();  //死亡時カウント
        DropDistribution();  //ドロップアイテム再配布
        m_statusUp?.Respawn();  //死亡時にステータスUP
        m_angerManager.SetIsAnger(false);  //怒りのoff
        m_statusManager.Respawn();
        m_stator.Reset();  //ステートのリセット
    }

    /// <summary>
    /// リスポーンする場所の計算
    /// </summary>
    /// <returns>リスポーンする場所</returns>
    Vector3 CalcuRespawnRandomPosition()
    {
        return m_generator.CalcuRandomPosition();
    }

    /// <summary>
    /// ドロップアイテムの再配布
    /// </summary>
    void DropDistribution()
    {
        if (m_dropManager.GetNumData() != 0)  //ドロップデータが存在したら
        {
            var datas = m_dropManager.GetDatas();
            m_generator.DropDistribution(datas);  //配布
            m_dropManager.RemoveDatas(datas);
        }
    }

    /// <summary>
    /// 死亡数カウント
    /// </summary>
    void DeathCount()
    {
        var target = m_targetManger.GetNowTarget();
        if (target)
        {
            //ターゲットがplayerなら
            var playerComp = target.GetComponent<Player.PlayerStatusManager>();
            if (playerComp)
            {
                m_generator.AddDeathCount();
            }
        }
    }

    /// <summary>
    /// リスポーンするかどうかの判断
    /// </summary>
    /// <returns></returns>
    bool IsRespawn()
    {
        if (m_param.isAbsoluteRespawn) { //強制リスポーン
            return true;
        }

        if (!m_param.isRespawn) {  //リスポーンさせないならfalse
            return false;
        }

        if (CalcuCamera.IsInCamera(transform.position, Camera.main)) {  //カメラの内なら
            return false;
        }

        var targetType = m_targetManger.GetNowTargetType();
        if(targetType == FoundObject.FoundType.Player) {  //ターゲットがPlayerなら
            return false;
        }

        return true;  //どの条件にも当てはまらないならリスポーンする。
    }

    //アクセッサ-------------------------------------------------------

    public void SetRespawnTime(float time)
    {
        m_param.time = time;
    }
    public float GetRespawnTime(float time)
    {
        return m_param.time;
    }

    public void SetIsRespawn(bool isRespawn)
    {
        m_param.isRespawn = isRespawn;
    }
    public bool GetIsRespawn()
    {
        return m_param.isRespawn;
    }

    public void SetParametor(RespawnManagerParametor parametor)
    {
        m_param = parametor;
    }

    //将来的に消す
    public void AddParametor(RespawnManagerParametor parametor)
    {
        m_param.time += parametor.time;
    }

    public void SetGenerator(EnemyGenerator generator)
    {
        m_generator = generator;
    }

    //StartNullCheck----------------------------------------------------

    void StartTargetNullCheck()
    {
        //if(m_target == null) {
        //    m_target = GameObject.Find("Player");
        //}
    }

    void StartGeneratorNullCheck()
    {
        return;

        if(m_generator != null) { //null出ないなら処理をしない
            return;
        }

        var generators = GameObject.FindObjectsOfType<EnemyGenerator>();

        foreach (var generator in generators)
        {
            if(generator.IsEqualCreateObject(gameObject))
            {
                m_generator = generator;
                return;
            }
        }
    }
}
