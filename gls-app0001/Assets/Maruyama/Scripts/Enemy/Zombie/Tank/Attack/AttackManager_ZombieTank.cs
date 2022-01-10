using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MaruUtility;

public class AttackManager_ZombieTank : AttackNodeManagerBase
{
    [Serializable]
    private struct Parametor {
        public float nearRange;

        public float normalSpeed;
        public float tackleSpeed;
        public float waitSeeProbability;  //様子見確率

        public Parametor(float nearRange, float normalSpeed, float tackleSpeed, float waitSeeProbability = 30.0f)
        { 
            this.nearRange = nearRange;
            this.normalSpeed = normalSpeed;
            this.tackleSpeed = tackleSpeed;

            this.waitSeeProbability = waitSeeProbability;
        }
    }

    public enum AttackType
    {
         None,
         Near,
         Tackle,   //タックル
         WaitSee,  //様子見
    }

    [SerializeField]
    private Parametor m_param = new Parametor(2.0f, 10.0f, 1000.0f);

    private TargetManager m_targetMgr;
    private Stator_ZombieTank m_stator;
    private AnimatorManager_ZombieTank m_animatorManager;
    private EnemyVelocityManager m_velocityManager;
    private EyeSearchRange m_eye;

    private AttackType m_type = AttackType.None;

    private void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieTank>();
        m_eye = GetComponent<EyeSearchRange>();
        m_velocityManager = GetComponent<EnemyVelocityManager>();
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        FoundObject target = m_targetMgr.GetNowTarget();
        if (target) {
            return m_eye.IsInEyeRange(target.gameObject, range);
            //return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else {
            return false;
        }
    }

    public override void AttackStart()
    {
        if(m_stator.GetNowStateType() == ZombieTankState.Attack) {  //攻撃状態なら処理を飛ばす。
            return;
        }

        //確率で様子見
        if (MyRandom.RandomProbability(m_param.waitSeeProbability))
        {
            m_stator.GetTransitionMember().waitSeeTrigger.Fire();
            return;
        }

        m_stator.GetTransitionMember().attackTrigger.Fire();

        FoundObject target = m_targetMgr.GetNowTarget();
        if (Calculation.IsRange(gameObject, target.gameObject, m_param.nearRange)) {
            NearAttackStart();
        }
        else {
            TackleAttackStart();
        }
    }

    private void NearAttackStart()
    {
        m_animatorManager.CrossFadeNormalAttack();
        m_type = AttackType.Near;
    }

    private void TackleAttackStart()
    {
        m_animatorManager.CrossFadeShout();
        m_velocityManager.ResetAll();
        m_type = AttackType.Tackle;
    }

    void WaitSeeStart()
    {
        m_type = AttackType.WaitSee;
    }

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();
        m_type = AttackType.None;
    }
}
