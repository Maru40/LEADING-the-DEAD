using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeObject;

public class BarricadeDurability : MonoBehaviour
{
    /// <summary>
    /// �o���P�[�h�̑ϋv�x
    /// </summary>
    [SerializeField]
    private float m_durability = 100;

    public void TakeDamage(AttributeObject.DamageData damageData)
    {
        Debug.Log($"{damageData.damageValue}�_���[�W�󂯂܂���");
        m_durability -= damageData.damageValue;

        m_durability = Mathf.Max(m_durability, 0);

        if (m_durability > 0)
        {
            return;
        }

        Debug.Log("�j�󂳂�܂���");

        Destroy(gameObject);
    }
}
