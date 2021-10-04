using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Attack_ZombieTank : AttackBase
{
    float m_moveForce = 10.0f;

    TargetManager m_targetMgr;
    Stator_ZombieTank m_stator;
    EnemyVelocityMgr m_velocityManager;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
    }

    void Update()
    {
        
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        FoundObject target = m_targetMgr.GetNowTarget();
        if (target) {
            return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else {
            return false;
        }
    }

    public override void AttackStart()
    {
        m_stator.GetTransitionMember().attackTrigger.Fire();
    }

    public void AddMoveForce()
    {
        var target = m_targetMgr.GetNowTarget();
        var toVec = target.gameObject.transform.position - transform.position;

        m_velocityManager?.AddForce(toVec.normalized * m_moveForce);
    }

    public override void Attack()
    {

    }

    public override void AttackHitEnd()
    {

    }

    public override void EndAnimationEvent()
    {

    }
}
