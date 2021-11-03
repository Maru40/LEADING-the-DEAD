using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class AttackManager_ZombieNormal : AttackNodeManagerBase
{
    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;
    EnemyVelocityMgr m_velocityMgr;
    EnemyRotationCtrl m_rotationCtrl;
    EyeSearchRange m_eyeRange;
    ThrongManager m_throngManager;
    StatusManagerBase m_statusManager;
    AnimatorManager_ZombieNormal m_animatorManager;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_throngManager = GetComponent<ThrongManager>();
        m_statusManager = GetComponent<StatusManagerBase>();
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
