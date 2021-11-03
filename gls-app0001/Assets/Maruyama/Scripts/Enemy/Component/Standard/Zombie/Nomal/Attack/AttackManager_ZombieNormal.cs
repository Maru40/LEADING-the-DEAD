using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class AttackManager_ZombieNormal : AttackNodeManagerBase
{
    public enum AttackType
    {
        Normal,
    }

    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;
    AnimatorManager_ZombieNormal m_animatorManager;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
    }

    /// <summary>
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    override public bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        FoundObject target = m_targetMgr.GetNowTarget();
        if (target)
        {
            return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else
        {
            return false;
        }
    }

    public override void AttackStart()
    {
        m_stator.GetTransitionMember().attackTrigger.Fire();

        m_animatorManager.ChangeNormalAttackAnimation();
    }

    override public void Attack()
    {

    }

    public override void AttackHitEnd()
    {

    }

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();
    }
}
