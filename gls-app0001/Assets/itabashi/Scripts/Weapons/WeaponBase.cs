using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using AttributeObject;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private Collider m_weaponCollider;

    [SerializeField]
    private DamageData m_baseAttackDamageData;

    public DamageData baseAttackDamageData => m_baseAttackDamageData;

    [SerializeField]
    private UnityEvent<TakeDamageObject, DamageData> m_opponentDamageEvent;

    private List<TakeDamageObject> m_opponentObjects = new List<TakeDamageObject>();

    public bool attackColliderEnabled
    {
        set => m_weaponCollider.enabled = value;
        get => m_weaponCollider.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        var takeDamageObject = other.GetComponent<TakeDamageObject>();

        if(HitTest(takeDamageObject))
        {
            OnDamageTheOpponent(takeDamageObject, m_baseAttackDamageData);

            m_opponentDamageEvent?.Invoke(takeDamageObject, m_baseAttackDamageData);
        }
    }

    private bool HitTest(TakeDamageObject takeDamageObject)
    {
        if(!takeDamageObject || !takeDamageObject.enabled)
        {
            return false;
        }

        foreach(var damageObject in m_opponentObjects)
        {
            if (damageObject == takeDamageObject)
            {
                return false;
            }
        }

        m_opponentObjects.Add(takeDamageObject);

        return true;
    }

    public void HitClear()
    {
        m_opponentObjects.Clear();
    }

    protected abstract void OnDamageTheOpponent(TakeDamageObject takeDamageObject, DamageData baseDamageData);
}
