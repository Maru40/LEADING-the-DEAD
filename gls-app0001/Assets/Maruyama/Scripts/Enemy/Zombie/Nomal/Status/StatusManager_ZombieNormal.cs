using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

public class StatusManager_ZombieNormal : StatusManagerBase , I_Stun
{
    //瀕死状態
    public enum DyingTypeEnum
    {
        None,
        Fire,
        Cutting,
    }

    private Stator_ZombieNormal m_stator = null;
    private AngerManager m_angerManager;

    private DamagedManager_ZombieNormal m_damageManager;
    private DamageParticleManager m_damageParticleManager;

    [SerializeField]
    private GameObject m_fireDamageParticle = null;  //炎ダメージ時のparticleのデータ管理
    [SerializeField]
    private GameObject m_cuttingDamageParticle = null;  //切られた時のparticle

    private DyingTypeEnum m_dyingType = DyingTypeEnum.None;  //瀕死状態

    private void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_angerManager = GetComponent<AngerManager>();
        m_damageParticleManager = GetComponent<DamageParticleManager>();

        m_damageManager = new DamagedManager_ZombieNormal(gameObject);
    }

    override protected void Start()
    {
        base.Start();

        SetDamageParticle();
    }

    private void SetDamageParticle()
    {
        m_damageParticleManager.AddCreateParticel(AttributeObject.DamageType.Fire, m_fireDamageParticle);
        m_damageParticleManager.AddCreateParticel(AttributeObject.DamageType.Cutting, m_cuttingDamageParticle);
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
        IsStun = false;

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

    public DyingTypeEnum DyingType => m_dyingType;
    public void ChangeDyingMode(DyingTypeEnum mode)
    {
        m_dyingType = mode;
    }
}
