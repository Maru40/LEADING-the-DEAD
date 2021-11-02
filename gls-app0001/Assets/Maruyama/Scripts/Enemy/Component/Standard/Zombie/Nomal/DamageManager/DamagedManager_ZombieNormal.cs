using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DyingTypeEnum = StatusManager_ZombieNormal.DyingTypeEnum;

public class DamagedManager_ZombieNormal : DamagedManagerBase
{

    StatusManager_ZombieNormal m_statusManager = null;
    AnimatorManager_ZombieNormal m_animatorManager = null;
    Stator_ZombieNormal m_stator = null;
    EnemyRespawnManager m_respawn = null;
    DropObjecptManager m_dropManager = null;
    DamageParticleManager m_particleManager = null;
    I_Stun m_stun = null;

    WaitTimer m_waitTimer = null;

    public DamagedManager_ZombieNormal(GameObject owner)
        :base(owner)
    {
        m_statusManager = owner.GetComponent<StatusManager_ZombieNormal>();
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_respawn = owner.GetComponent<EnemyRespawnManager>();
        m_dropManager = owner.GetComponent<DropObjecptManager>();
        m_particleManager = owner.GetComponent<DamageParticleManager>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
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
        Death(data ,ref status);         //死亡判定処理

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

            CreateDamageEffect(data);
        }

        m_animatorManager.HitStop(data);  //ヒットストップ
        StartDamageInterval(ref status); //ダメージインターバルの開始
    }

    void CreateDamageEffect(DamageData data)
    {
        //将来的にこのif文いらない？
        if(data.type == DamageType.Fire)
        {
            m_particleManager.StartDamage(data.type);
        }
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
    void Death(DamageData data ,ref StatusManagerBase.Status status)
    {
        if (IsDeath(in status))
        {
            status.hp = 0;

            var dyingMode = data.type switch  //瀕死状態のTypeを決定
            {
                DamageType.None => DyingTypeEnum.None,
                DamageType.Fire => DyingTypeEnum.Fire,
                _ => DyingTypeEnum.None
            };
            m_statusManager.ChangeDyingMode(dyingMode);  //瀕死状態のTypeを変更

            m_stator.GetTransitionMember().dyingTrigger.Fire();  //瀕死状態に変更

            //m_respawn?.RespawnReserve();
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
