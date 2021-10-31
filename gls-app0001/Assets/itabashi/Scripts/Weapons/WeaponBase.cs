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
    private MeleeWeaponTrail m_weaponTrail;

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

    public bool weaponTrailEnabled
    {
        set
        {
            if (m_weaponTrail) { m_weaponTrail.enabled = value; }
        }

        get => m_weaponTrail ? m_weaponTrail.enabled : false;
    }


    private void OnTriggerEnter(Collider other)
    {
        var takeDamageObject = other.GetComponent<TakeDamageObject>();

        if(HitTest(takeDamageObject))
        {
            Vector3 hitPosition = other.ClosestPointOnBounds(this.transform.position);

            OnDamageTheOpponent(takeDamageObject, m_baseAttackDamageData, hitPosition);

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

    protected abstract void OnDamageTheOpponent(TakeDamageObject takeDamageObject, DamageData baseDamageData, Vector3 hitPosition);
}
