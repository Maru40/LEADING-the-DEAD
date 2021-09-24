using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Attack_ZombieNormal : AttackBase
{
    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;
    EyeSearchRange m_eyeRange;

    [SerializeField]
    EnemyAttackTriggerAction m_hitBox = null;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
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
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        FoundObject target = m_targetMgr.GetNowTarget();
        if (target)
        {
            //return UtilityCalculation.IsRange(gameObject, target.gameObject, range);
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
    /// 相手にダメージを与える。
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
