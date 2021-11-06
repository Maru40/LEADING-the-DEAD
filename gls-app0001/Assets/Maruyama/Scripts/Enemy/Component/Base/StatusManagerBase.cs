using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;
using MaruUtility;

public abstract class StatusManagerBase : MonoBehaviour
{
    [Serializable]
    public struct Status
    {
        public float hp;
        public float damageIntervalTime;  //ダメージを受けた後の無敵時間
        public readonly ReactiveProperty<bool> isHitStopReactive;
        public bool isStun;

        public Status(float hp, float damageIntervalTime)
        {
            this.hp = hp;
            this.damageIntervalTime = damageIntervalTime;
            this.isHitStopReactive = new ReactiveProperty<bool>();
            this.isStun = false;
        }
    }

    private BuffManager m_buffManager = new BuffManager();

    [SerializeField]
    protected Status m_status = new Status(1.0f, 3.0f);

    /// <summary>
    /// ヒットストップ時にUpdateを止めるコンポーネント群
    /// </summary>
    [SerializeField]
    protected List<Behaviour> m_hitStopBehaviours = new List<Behaviour>();
    /// <summary>
    /// ヒットストップ前のコンポーネントの状態を記録
    /// </summary>
    private List<ChangeBehaviourEnablePair> m_beforeHitStopBehaviourEnablePairs = new List<ChangeBehaviourEnablePair>();

    protected virtual void Start()
    {
        //ヒットストップ変更時に動きを止めるコンポーネント群
        m_status.isHitStopReactive.Skip(1)
            .Where(isHitStop => isHitStop == true)
            .Subscribe(isHitStop => {
                SetBeforeHitStopBehaviourEnable(); //ヒットストップ前の状態を記録
                UtilityBehaviour.ChangeBehaviourEnable(m_hitStopBehaviours, false); }) //登録した全てのコンポーネントをfalseに
            .AddTo(this);

        //ヒットストップ前の状態にコンポーネントを戻す
        m_status.isHitStopReactive.Skip(1)
            .Where(isHitStop => isHitStop == false)
            .Subscribe(isHitStop => UtilityBehaviour.ChangeBehaviourEnable(m_beforeHitStopBehaviourEnablePairs))
            .AddTo(this);
    }

    private void SetBeforeHitStopBehaviourEnable()  //ヒットストップ前のコンポーネントの状態を記録
    {
        m_beforeHitStopBehaviourEnablePairs.Clear();

        foreach(var behaviour in m_hitStopBehaviours)
        {
            m_beforeHitStopBehaviourEnablePairs.Add(new ChangeBehaviourEnablePair(behaviour, behaviour.enabled));
        }
    }

    //仮想関数----------------------------------------------------------

    public abstract void Damage(AttributeObject.DamageData data);

    //アクセッサ--------------------------------------------------------

    public void SetBuffParametor(BuffParametor parametor)
    {
        m_buffManager.SetParametor(parametor);
    }
    public BuffParametor GetBuffParametor()
    {
        return m_buffManager.GetParametor();
    }

    public void SetStatus(Status status)
    {
        m_status = status;
    }
    public Status GetStatus()
    {
        return m_status;
    }

    public void AddStatus(Status status)
    {
        m_status.hp += status.hp;
        m_status.damageIntervalTime += status.damageIntervalTime;
    }

    /// <summary>
    /// ヒットストップ状態かどうか
    /// </summary>
    public bool IsHitStop
    {
        get => m_status.isHitStopReactive.Value;
        set => m_status.isHitStopReactive.Value = value;
    }

    /// <summary>
    /// スタンしているかどうか
    /// </summary>
    public bool IsStun
    {
        get => m_status.isStun;
        protected set => m_status.isStun = value;
    }
}
