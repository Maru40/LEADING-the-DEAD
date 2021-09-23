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
        public float damageIntervalTime;  //ダメージを受けた後の無敵時間

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
            Debug.Log("死亡");
        }

        float time = m_status.damageIntervalTime;
        m_waitTimer.AddWaitTimer(GetType(), time);
    }

    
}
