﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

/// <summary>
/// 怒り状態の管理
/// </summary>
public class AngerManager : MonoBehaviour
{
    [Serializable]
    public struct RiseParametor
    {
        public float attackPower;
        public float speed;

        public RiseParametor(float attackPower, float speed)
        {
            this.attackPower = attackPower;
            this.speed = speed;
        }

        public static RiseParametor operator +(RiseParametor right, RiseParametor left)
        {
            var param = new RiseParametor();
            param.attackPower = right.attackPower + left.attackPower;
            param.speed = right.speed + left.speed;

            return param;
        }

        public static RiseParametor operator -(RiseParametor right, RiseParametor left)
        {
            var param = new RiseParametor();
            param.attackPower = right.attackPower - left.attackPower;
            param.speed = right.speed - left.speed;

            return param;
        }
    }

    [SerializeField]
    RiseParametor m_riseParam = new RiseParametor(1.2f,1.2f);  //ステータス上昇のパラメータ

    [SerializeField]
    float m_time = 99.0f;

    [SerializeField]
    bool m_isAngerTimer = false;  //怒り状態をタイマー性にするかどうか

    bool m_isAnger = false;
    ReactiveProperty<bool> m_isReactiveAnger = new ReactiveProperty<bool>(false);

    WaitTimer m_waitTimer;
    StatusManagerBase m_statusManager;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_statusManager = GetComponent<StatusManagerBase>();
    }

    private void Start()
    {
        m_isReactiveAnger.Skip(1).Subscribe(x => { UpdateBuffParametor(); }).AddTo(this);
    }

    public void StartAnger()
    {
        SetIsAnger(true);

        if (m_isAngerTimer) {
            m_waitTimer.AddWaitTimer(GetType(), m_time, EndAnger);
        }
    }

    void EndAnger()
    {
        SetIsAnger(false);
    }

    void UpdateBuffParametor()
    {
        var param = m_statusManager.GetBuffParametor();

        if (m_isAnger) {
            param.angerParam = param.angerParam + m_riseParam;
        }
        else {
            param.angerParam = param.angerParam - m_riseParam;
        }

        m_statusManager.SetBuffParametor(param);
    }

    //アクセッサ--------------------------------------------------------

    public bool IsAnger()
    {
        return m_isAnger;
    }
    public void SetIsAnger(bool isAnger)
    {
        m_isAnger = isAnger;
        m_isReactiveAnger.Value = isAnger;
    }

    public RiseParametor GetRiseParametor()
    {
        return m_riseParam;
    }
}