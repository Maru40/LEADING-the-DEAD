using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Attack_ZombieNormal : AttackBase
{
    Stator_ZombieNormal m_stator;
    TargetMgr m_targetMgr;
    EyeSearchRange m_eyeRange;

    void Start()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetMgr>();
        m_eyeRange = GetComponent<EyeSearchRange>();
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
        var target = m_targetMgr.GetNowTarget();

        return m_eyeRange.IsInEyeRange(target, range);
    }

    /// <summary>
    /// 攻撃のダメージ範囲にいるかどうか
    /// </summary>
    /// <returns></returns>
    bool IsAttackDamageRange()
    {
        float range = GetBaseParam().damageRange;
        var target = m_targetMgr.GetNowTarget();

        return m_eyeRange.IsInEyeRange(target, range);
    }

    Vector3 CalcuToTargetVec()
    {
        var target = m_targetMgr.GetNowTarget();
        var toVec = target.transform.position - transform.position;
        return toVec;
    }

    override public void Attack(){
        Debug.Log("Attack");

        if (IsAttackDamageRange()){
            Debug.Log("Attack_Hit");

            var target = m_targetMgr.GetNowTarget();

            var damage = target.GetComponent<I_TakeDamage>();
            if (damage != null) 
            {
                var data = new DamageData((int)GetBaseParam().power);
                damage.TakeDamage(data);
            }
        }
    }
}
