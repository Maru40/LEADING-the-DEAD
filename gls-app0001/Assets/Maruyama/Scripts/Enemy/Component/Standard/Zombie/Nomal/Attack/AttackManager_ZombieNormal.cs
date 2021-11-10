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
    EyeSearchRange m_eye;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
        m_eye = GetComponent<EyeSearchRange>();
    }

    /// <summary>
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    override public bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        //FoundObject target = m_targetMgr.GetNowTarget();
        var position = m_targetMgr.GetNowTargetPosition();
        if (position != null)
        {
            return m_eye.IsInEyeRange((Vector3)position, range);
            //return Calculation.IsRange(gameObject, (Vector3)position, range);
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

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();
    }
}
