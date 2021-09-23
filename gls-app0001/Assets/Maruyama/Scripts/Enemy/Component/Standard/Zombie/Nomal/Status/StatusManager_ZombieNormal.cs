using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class StatusManager_ZombieNormal : StatusManagerBase
{
    [Serializable]
    public struct Status
    {
        public float hp;
        public float damageIntervalTime;  //�_���[�W���󂯂���̖��G����

        public Status(float hp, float damageIntervalTime)
        {
            this.hp = hp;
            this.damageIntervalTime = damageIntervalTime;
        }
    }

    [SerializeField]
    Status m_status = new Status(1.0f, 3.0f);

    WaitTimer m_waitTimer = null;
    RespawnRandom_OutRangeOfTarget m_respawn = null;

    void Start()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_respawn = GetComponent<RespawnRandom_OutRangeOfTarget>();
    }

    public void Damage(AttributeObject.DamageData data)
    {
        if(m_waitTimer == null) {
            m_waitTimer = GetComponent<WaitTimer>();
        }

        if (m_waitTimer.IsWait(GetType())) {
            return;
        }

        m_status.hp -= data.damageValue;

        if (m_status.hp <= 0)
        {
            m_status.hp = 0;
            m_respawn?.Respawn();
        }

        //�_���[�W�C���^�[�o���J�n
        float time = m_status.damageIntervalTime;
        m_waitTimer.AddWaitTimer(GetType(), time);
    }


    //�A�N�Z�b�T------------------------------------------------------------

    public void SetStatus(Status status)
    {
        m_status = status;
    }
    public Status GetStatus()
    {
        return m_status;
    }
}
