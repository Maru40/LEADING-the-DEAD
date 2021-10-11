using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// 怒り状態の管理
/// </summary>
public class AngerManager : MonoBehaviour
{
    [Serializable]
    public struct RiseParametor
    {
        float attack;
        float speed;

        public RiseParametor(float attack,float speed)
        {
            this.attack = attack;
            this.speed = speed;
        }
    }

    [SerializeField]
    RiseParametor m_riseParam;  //ステータス上昇のパラメータ

    [SerializeField]
    float m_time = 99.0f;

    bool m_isAnger = false;

    WaitTimer m_waitTimer;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    public void StartAnger()
    {
        SetIsAnger(true);

        m_waitTimer.AddWaitTimer(GetType(), m_time, EndAnger);
    }

    void EndAnger()
    {
        SetIsAnger(false);
    }

    //アクセッサ--------------------------------------------------------

    public bool IsAnger()
    {
        return m_isAnger;
    }
    public void SetIsAnger(bool isAnger)
    {
        m_isAnger = isAnger;
    }

    public RiseParametor GetRiseParametor()
    {
        return m_riseParam;
    }
}