using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeObject;
using UnityEngine.Events;
using UniRx;
using System;

public class BarricadeDurability : MonoBehaviour
{
    /// <summary>
    /// バリケードの耐久度
    /// </summary>
    [SerializeField]
    private FloatReactiveProperty m_durability = new FloatReactiveProperty(100.0f);

    public float durability
    {
        private set => m_durability.Value = Mathf.Clamp(value, 0.0f, m_maxDurability);

        get => m_durability.Value;
    }

    public IObservable<float> OnChangedDurability => m_durability;

    [SerializeField]
    private float m_maxDurability = 100;

    [SerializeField]
    private Gauge m_gauge;

    [SerializeField]
    private UnityEvent m_breakEvent;

    private void Awake()
    {
        OnChangedDurability
            .Subscribe(durability => m_gauge.fillAmount = durability / m_maxDurability)
            .AddTo(this);

        OnChangedDurability
            .Where(durability => durability == 0.0f)
            .Subscribe(_ =>
            {
                m_breakEvent?.Invoke();
                Debug.Log("破壊されました");
                Destroy(gameObject);
            })
            .AddTo(this);
    }

    public void TakeDamage(AttributeObject.DamageData damageData)
    {
        durability -= damageData.damageValue;
    }

    public void Recovery(float recoveryValue)
    {
        durability += recoveryValue;
    }
}
