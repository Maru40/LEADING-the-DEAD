using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = Stator_ZombieChild.StateType;

public class DamageManager_ZombieChild : DamagedManagerBase
{
    private Stator_ZombieChild m_stator;
    private StatusManager_ZombieChild m_statusManager;

    public DamageManager_ZombieChild(GameObject owner)
        :base(owner)
    {
        m_stator = owner.GetComponent<Stator_ZombieChild>();
        m_statusManager = owner.GetComponent<StatusManager_ZombieChild>();
    }

    public override void Damaged(DamageData data)
    {
        var status = m_statusManager.GetStatus();

        //ダメージを受ける
        status.hp -= data.damageValue;
        if (status.IsDeath()) //死亡したら
        {
            Dyning();
        }

        m_statusManager.SetStatus(status);
    }

    private void Dyning()
    {
        m_stator.ChangeState(StateType.Dying, (int)StateType.Dying);
    }
}
