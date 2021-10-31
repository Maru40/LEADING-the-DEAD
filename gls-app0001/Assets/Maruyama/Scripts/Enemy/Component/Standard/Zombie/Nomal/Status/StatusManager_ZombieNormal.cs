using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class StatusManager_ZombieNormal : StatusManagerBase , I_Stun
{
    Stator_ZombieNormal m_stator = null;
    AngerManager m_angerManager;

    DamagedManager_ZombieNormal m_damageManager;
    DamageParticleManager m_damageParticleManager;

    [SerializeField]
    GameObject m_fireDamageParticle = null;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_angerManager = GetComponent<AngerManager>();
        m_damageParticleManager = GetComponent<DamageParticleManager>();

        m_damageManager = new DamagedManager_ZombieNormal(gameObject);
    }

    void Start()
    {
        SetDamageParticle();
    }

    private void SetDamageParticle()
    {
        m_damageParticleManager.SetCreateParticels(AttributeObject.DamageType.Fire, m_fireDamageParticle);
    }

    public override void Damage(AttributeObject.DamageData data)
    {
        m_damageManager.Damaged(data);
    }

    //インターフェースの実装----------------------------------------------------------------

    void I_Stun.StartStun()
    {
        m_stator.GetTransitionMember().stunTrigger.Fire();
    }

    void I_Stun.EndStun()
    {
        if (m_angerManager.IsAnger())
        {
            m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        }
        else
        {
            m_stator.GetTransitionMember().angerTirgger.Fire();
        }
    }

    //アクセッサ----------------------------------------------------------------------------
}
