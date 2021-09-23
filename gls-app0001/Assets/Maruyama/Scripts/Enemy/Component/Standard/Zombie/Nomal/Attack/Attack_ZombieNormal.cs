using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Attack_ZombieNormal : AttackBase
{
    Stator_ZombieNormal m_stator;
    TargetMgr m_targetMgr;
    EyeSearchRange m_eyeRange;

    [SerializeField]
    EnemyAttackTriggerAction m_hitBox = null;

    void Start()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetMgr>();
        m_eyeRange = GetComponent<EyeSearchRange>();

        m_hitBox.AddEnterAction(SendDamage);
    }

    void Update()
    {
        if (IsAttackStartRange())
        {
            m_stator.GetTransitionMember().attackTrigger.Fire();
        }
    }

    /// <summary>
    /// �U�����J�n���鋗�����ǂ���
    /// </summary>
    /// <returns>�J�n����Ȃ�true</returns>
    bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        var target = m_targetMgr.GetNowTarget();
        if (target)
        {
            return m_eyeRange.IsInEyeRange(target.gameObject, range);
        }
        else
        {
            return false;
        }
        
    }

    /// <summary>
    /// �U���̃_���[�W�͈͂ɂ��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    bool IsAttackDamageRange()
    {
        float range = GetBaseParam().damageRange;
        var target = m_targetMgr.GetNowTarget();
        if (target)
        {
            return m_eyeRange.IsInEyeRange(target.gameObject, range);
        }
        else
        {
            return false;
        }
        
    }

    override public void Attack(){
        Debug.Log("Attack");

        m_hitBox.AttackStart();
    }

    public override void AttackHitEnd()
    {
        m_hitBox.AttackEnd();
    }


    /// <summary>
    /// ����Ƀ_���[�W��^����B
    /// </summary>
    private void SendDamage(Collider other)
    {
        Debug.Log("Attack_Hit");

        if(other.gameObject == this.gameObject) {
            return;
        }

        //var target = m_targetMgr.GetNowTarget();

        var damage = other.GetComponent<AttributeObject.TakeDamageObject>();
        if (damage != null)
        {
            var data = new AttributeObject.DamageData((int)GetBaseParam().power);
            damage.TakeDamage(data);
        }
    }

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();
    }
}
