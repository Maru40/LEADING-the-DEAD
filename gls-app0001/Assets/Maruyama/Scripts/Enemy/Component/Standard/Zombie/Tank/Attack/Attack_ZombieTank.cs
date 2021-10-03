using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Attack_ZombieTank : AttackBase
{
    TargetManager m_targetMgr;
    Stator_ZombieTank m_stator;

    void Start()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
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
