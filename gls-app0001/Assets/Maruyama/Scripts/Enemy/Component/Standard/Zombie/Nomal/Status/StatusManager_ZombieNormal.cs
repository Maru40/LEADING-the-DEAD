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
    I_Stun m_stun;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_respawn = GetComponent<EnemyRespawnManager>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_animator = GetComponent<AnimatorCtrl_ZombieNormal>();
        m_dropManager = GetComponent<DropObjecptManager>();
        m_stun = GetComponent<I_Stun>();
    }

    public override void Damage(AttributeObject.DamageData data)
    {
        if (m_waitTimer.IsWait(GetType())) {
            return;
        }

        if (data.isStunAttack)  //スタン状態になる攻撃なら
        {
            m_stun.StartStun();
        }
        else
        {
            //普通にダメージを受ける
            m_status.hp -= data.damageValue;
        }
        
        if (m_status.hp <= 0)
        {
            m_status.hp = 0;
            m_respawn?.RespawnReserve();
        }

        //ダメージインターバル開始
        float time = m_status.damageIntervalTime;
        m_waitTimer.AddWaitTimer(GetType(), time);
    }

    //インターフェースの実装----------------------------------------------------------------

    void I_Stun.StartStun()
    {
        m_stator.GetTransitionMember().stunTrigger.Fire();
        m_dropManager.Drop();

        //アニメーションの切替
        m_animator.StartStun();
    }

    void I_Stun.EndStun()
    {
        //m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        m_stator.GetTransitionMember().angerTirgger.Fire();

        //アニメーションの切替
        m_animator.EndStun();
    }

    //アクセッサ----------------------------------------------------------------------------
}
