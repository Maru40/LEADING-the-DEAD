using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTriggerAction : TriggerAction
{
    WaitTimer m_waitTimer;
    Collider m_hitCollider;

    private void Start()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_hitCollider = GetComponent<Collider>();
    }


    /// <summary>
    /// 攻撃開始
    /// </summary>
    /// <param name="hitTime">ヒット時間</param>
    public void AttackStart()
    {
        //m_waitTimer.AddWaitTimer(GetType(), hitTime, AttackEnd);

        m_hitCollider.enabled = true;
    }

    public void AttackEnd()
    {
        m_hitCollider.enabled = false;
    }
}
