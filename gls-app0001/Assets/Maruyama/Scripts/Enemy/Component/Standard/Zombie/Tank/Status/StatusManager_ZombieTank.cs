using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager_ZombieTank : StatusManagerBase
{
    WaitTimer m_waitTimer;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    public override void Damage(DamageData data)
    {
        //if (m_waitTimer.IsWait(GetType())) {
        //    return;
        //}

        //m_status.hp -= data.damageValue;

        //if (m_status.hp <= 0) 
        //{
        //    m_status.hp = 0;
        //    gameObject.SetActive(false);
        //}

        ////ダメージインターバル開始
        //float time = m_status.damageIntervalTime;
        //m_waitTimer.AddWaitTimer(GetType(), time);
    }

    public void EatDamage() {
        gameObject.SetActive(false);
    }
}
