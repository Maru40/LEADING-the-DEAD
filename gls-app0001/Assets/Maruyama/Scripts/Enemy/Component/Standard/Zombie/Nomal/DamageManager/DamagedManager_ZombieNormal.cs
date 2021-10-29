using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedManager_ZombieNormal : DamagedManagerBase
{

    StatusManager_ZombieNormal m_statusManager = null;
    EnemyRespawnManager m_respawn = null;
    DropObjecptManager m_dropManager = null;
    I_Stun m_stun = null;

    WaitTimer m_waitTimer = null;

    public DamagedManager_ZombieNormal(GameObject owner)
        :base(owner)
    {
        m_statusManager = owner.GetComponent<StatusManager_ZombieNormal>();
        m_respawn = owner.GetComponent<EnemyRespawnManager>();
        m_dropManager = owner.GetComponent<DropObjecptManager>();
        m_stun = owner.GetComponent<I_Stun>();

        m_waitTimer = owner.GetComponent<WaitTimer>();
    }

    public override void Damaged(DamageData data)
    {
        if (m_waitTimer.IsWait(GetType())) {
            return;
        }

        var status = m_statusManager.GetStatus();

        Damage(data, ref status);  //ダメージを受ける処理
        Death(ref status);         //死亡判定処理

        m_statusManager.SetStatus(status);
    }

    /// <summary>
    /// ダメージ計算処理
    /// </summary>
    /// <param name="data">ダメージデータ</param>
    /// <param name="status">現在のステータスの参照</param>
    void Damage(DamageData data, ref StatusManagerBase.Status status)
    {
        if (data.isStunAttack) { //スタン状態になる攻撃なら
            Stun(data.obj);
        }
        else {
            //ダメージを受ける
            status.hp -= data.damageValue;
        }

        StartDamageInterval(ref status); //ダメージインターバルの開始
    }

    /// <summary>
    /// ダメージインターバルの開始
    /// </summary>
    void StartDamageInterval(ref StatusManagerBase.Status status)
    {
        float time = status.damageIntervalTime;
        m_waitTimer.AddWaitTimer(GetType(), time);
    }

    /// <summary>
    /// 死亡時の処理
    /// </summary>
    /// <param name="status">現在のステータスの参照</param>
    void Death(ref StatusManagerBase.Status status)
    {
        if (IsDeath(in status))
        {
            status.hp = 0;
            m_respawn?.RespawnReserve();
        }
    }

    /// <summary>
    /// 死亡したかどうか
    /// </summary>
    /// <param name="status">現在のステータスの参照</param>
    /// <returns>死亡状態ならtrue</returns>
    bool IsDeath(in StatusManagerBase.Status status)
    {
        return status.hp <= 0 ? true : false;
    }

    /// <summary>
    /// スタン時の処理
    /// </summary>
    /// <param name="other">スタンを与えてきた相手</param>
    void Stun(GameObject other)
    {
        m_stun.StartStun();
        m_dropManager.Drop(other);
    }

}
