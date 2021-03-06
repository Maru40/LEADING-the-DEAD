using System.Collections;
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
        public float attackAnimeSpeed;

        public RiseParametor(float attackPower, float speed, float attackAnimeSpeed)
        {
            this.attackPower = attackPower;
            this.speed = speed;
            this.attackAnimeSpeed = attackAnimeSpeed;
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
    private RiseParametor m_riseParam = new RiseParametor(1.2f,1.2f, 1.2f);  //ステータス上昇のパラメータ

    [SerializeField]
    private float m_time = 99.0f;

    [SerializeField]
    private bool m_isAngerTimer = false;  //怒り状態をタイマー性にするかどうか

    private readonly ReactiveProperty<bool> m_isReactiveAnger = new ReactiveProperty<bool>(false);

    private WaitTimer m_waitTimer;
    private StatusManagerBase m_statusManager;

    private void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_statusManager = GetComponent<StatusManagerBase>();
    }

    private void Start()
    {
        m_isReactiveAnger.Skip(1)
            .Subscribe(x => { UpdateBuffParametor(); })
            .AddTo(this);
    }

    public void StartAnger()
    {
        SetIsAnger(true);

        if (m_isAngerTimer) {  //怒りに時間制限を設ける時。
            m_waitTimer.AddWaitTimer(GetType(), m_time, EndAnger);
        }
    }

    private void EndAnger()
    {
        SetIsAnger(false);
    }

    /// <summary>
    /// バフデータをStatusManagerに送る
    /// </summary>
    private void UpdateBuffParametor()
    {
        var param = m_statusManager.GetBuffParametor();

        if (m_isReactiveAnger.Value) {
            param.angerParam = param.angerParam + m_riseParam;
        }
        else {
            param.angerParam = param.angerParam - m_riseParam;
        }

        m_statusManager.SetBuffParametor(param);
    }

    //アクセッサ--------------------------------------------------------

    public void SetIsAnger(bool isAnger)
    {
        m_isReactiveAnger.Value = isAnger;
    }
    public bool IsAnger()
    {
        return m_isReactiveAnger.Value;
    }
    public IObservable<bool> isAngerObservable => m_isReactiveAnger;

    public void SetRiseParametor(RiseParametor parametor)
    {
        m_riseParam = parametor;
    }
    public RiseParametor GetRiseParametor()
    {
        return m_riseParam;
    }
}