using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private Collider m_weaponCollider;

    [SerializeField]
    private AttributeObject.DamageData m_baseAttackDamageData;

    public AttributeObject.DamageData baseAttackDamageData => m_baseAttackDamageData;

    public bool attackColliderEnabled
    {
        set => m_weaponCollider.enabled = value;
        get => m_weaponCollider.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        var takeDamageObject = other.GetComponent<AttributeObject.TakeDamageObject>();

        if(takeDamageObject && takeDamageObject.enabled)
        {
            OnDamageTheOpponent(takeDamageObject, m_baseAttackDamageData);
        }
        takeDamageObject?.TakeDamage(m_baseAttackDamageData);
    }

    protected abstract void OnDamageTheOpponent(AttributeObject.TakeDamageObject takeDamageObject, AttributeObject.DamageData baseDamageData);
}
