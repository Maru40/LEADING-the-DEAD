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

    [SerializeField]
    private UnityEvent<bool> m_changingChangeEvent = new UnityEvent<bool>();

    private bool m_isBreak = false;

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
                //Destroy(gameObject);
                m_isBreak = true;
            })
            .AddTo(this);

        OnChangedDurability
            .Where(durability => durability == m_maxDurability)
            .Subscribe(_ => m_changingChangeEvent?.Invoke(false))
            .AddTo(this);

        OnChangedDurability
            .Where(durability => durability != m_maxDurability)
            .Subscribe(_ => m_changingChangeEvent?.Invoke(true))
            .AddTo(this);
    }

    public void TakeDamage(AttributeObject.DamageData damageData)
    {
        if (!m_isBreak)
        {
            durability -= damageData.damageValue;
        }
    }

    public void Recovery(float recoveryValue)
    {
        if (!m_isBreak)
        {
            durability += recoveryValue;
        }
    }

    private void Update()
    {
        BreakCommand();
    }

    // コマンドで強制破壊用
    private void BreakCommand()
    {
        if(m_isBreak)
        {
            return;
        }

        if (UnityEngine.InputSystem.Keyboard.current.bKey.isPressed && UnityEngine.InputSystem.Keyboard.current.kKey.isPressed)
        {
            TakeDamage(new DamageData(100000000));
        }
    }
}
