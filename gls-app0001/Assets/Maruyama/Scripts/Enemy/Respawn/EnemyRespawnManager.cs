using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct RespawnManagerParametor
{
    public bool isRespawn;
    public float time;

    public RespawnManagerParametor(bool isRespawn, float time)
    {
        this.isRespawn = isRespawn;
        this.time = time;
    }
}

/// <summary>
/// ターゲットの範囲外にリスポーンする処理
/// </summary>
public class EnemyRespawnManager : EnemyRespawnBase
{
    //[SerializeField]
    //GameObject m_target = null;  //現在使っていない。

    [SerializeField]
    RespawnManagerParametor m_param = new RespawnManagerParametor(true, 0.0f);

    //[SerializeField]
    //bool m_isRespawn = true;

    //[SerializeField]
    //float m_time = 10.0f;

    [SerializeField]
    EnemyGenerator m_generator = null;

    StatorBase m_stator;
    WaitTimer m_waitTimer;

    void Awake()
    {
        //StartTargetNullCheck();
        StartGeneratorNullCheck();

        m_stator = GetComponent<StatorBase>();
        m_waitTimer = GetComponent<WaitTimer>();
    }

    //リスポーン準備
    public void RespawnReserve()
    {
        //リスポーンするなら準備をする。
        if (m_param.isRespawn)
        {
            //使いまわすため、削除せずにリスポーンポイントに設定する。
            gameObject.transform.position = new Vector3(0.0f, -100.0f, 0.0f);

            m_waitTimer.AddWaitTimer(GetType(), m_param.time, Respawn);
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

    //StartNullCheck----------------------------------------------------

    void StartTargetNullCheck()
    {
        //if(m_target == null) {
        //    m_target = GameObject.Find("Player");
        //}
    }

    void StartGeneratorNullCheck()
    {
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
