using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class StatusManager_ZombieNormal : StatusManagerBase
{
    [Serializable]
    struct Status_ZombieNormal
    {
        public float hp;
        public float damageIntervalTime;  //ƒ_ƒ[ƒW‚ğó‚¯‚½Œã‚Ì–³“GŠÔ

        public Status_ZombieNormal(float hp, float damageIntervalTime)
        {
            this.hp = hp;
            this.damageIntervalTime = damageIntervalTime;
        }
    }

    [SerializeField]
    Status_ZombieNormal m_status = new Status_ZombieNormal(1.0f, 3.0f);

    WaitTimer m_waitTimer = null;

    void Start()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    public void Damage(AttributeObject.DamageData data)
    {
        if (m_waitTimer.IsWait(GetType())) {
            return;
        }

        m_status.hp -= data.damageValue;

        if(m_status.hp <= 0)
        {
            m_status.hp = 0;
            Debug.Log("€–S");
            gameObject.SetActive(false);
        }

        float time = m_status.damageIntervalTime;
        m_waitTimer.AddWaitTimer(GetType(), time);
    }

    
}
