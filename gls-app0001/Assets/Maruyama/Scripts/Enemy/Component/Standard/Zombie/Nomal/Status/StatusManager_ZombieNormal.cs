using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class StatusManager_ZombieNormal : StatusManagerBase , I_Stun
{
    WaitTimer m_waitTimer = null;
    EnemyRespawnManager m_respawn = null;
    Stator_ZombieNormal m_stator = null;
    AnimatorCtrl_ZombieNormal m_animator = null;
    DropObjecptManager m_dropManager = null;
    AngerManager m_angerManager;
    I_Stun m_stun;

    DamagedManager_ZombieNormal m_damageManager;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_respawn = GetComponent<EnemyRespawnManager>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_animator = GetComponent<AnimatorCtrl_ZombieNormal>();
        m_dropManager = GetComponent<DropObjecptManager>();
        m_angerManager = GetComponent<AngerManager>();
        m_stun = GetComponent<I_Stun>();

        m_damageManager = new DamagedManager_ZombieNormal(gameObject);
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
