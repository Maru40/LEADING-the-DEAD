using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SignalGunCtrl : ItemUserBase
{
    [SerializeField]
    GameObject m_bomb = null;

    [SerializeField,Min(0.0f)]
    private float m_coolTime = 1.0f;

    private float m_nowCountTime = 1.0f;

    private ReactiveProperty<float> m_coolTimePercentage = new ReactiveProperty<float>();

    public System.IObservable<float> coolTimePercentageOnChanged => m_coolTimePercentage;

    private void Awake()
    {
        m_nowCountTime = m_coolTime;

        this.UpdateAsObservable()
            .Where(_ => m_nowCountTime < m_coolTime)
            .Subscribe(_ => { m_nowCountTime += Time.deltaTime;m_coolTimePercentage.Value = m_nowCountTime / m_coolTime; })
            .AddTo(this);
    }

    private void Start()
    {
    }

    protected override void OnUse()
    {
        if(m_nowCountTime < m_coolTime || !isUse)
        {
            return;
        }

        m_nowCountTime = 0.0f;

        //信号弾の生成
        Instantiate(m_bomb, transform.position, Quaternion.identity);
    }
}
