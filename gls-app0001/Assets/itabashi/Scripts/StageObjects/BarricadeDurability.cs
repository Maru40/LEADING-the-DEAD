using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeObject;
using UnityEngine.Events;

public class BarricadeDurability : MonoBehaviour
{
    /// <summary>
    /// バリケードの耐久度
    /// </summary>
    [SerializeField]
    private float m_durability = 100;

    [SerializeField]
    private float m_maxDurability = 100;

    [SerializeField]
    private Gauge m_gauge;

    [SerializeField]
    private UnityEvent m_breakEvent;

    public void TakeDamage(AttributeObject.DamageData damageData)
    {
        Debug.Log($"{damageData.damageValue}ダメージ受けました");
        m_durability -= damageData.damageValue;

        m_durability = Mathf.Clamp(m_durability, 0.0f, m_maxDurability);

        m_gauge.fillAmount = m_durability / m_maxDurability;

        if (m_durability > 0)
        {
            return;
        }

        m_breakEvent?.Invoke();

        Debug.Log("破壊されました");

        Destroy(gameObject);
    }

    public void Recovery(float recoveryValue)
    {
        m_durability = Mathf.Clamp(m_durability + recoveryValue, 0.0f, m_maxDurability);

        m_gauge.fillAmount = m_durability / m_maxDurability;
    }
}
