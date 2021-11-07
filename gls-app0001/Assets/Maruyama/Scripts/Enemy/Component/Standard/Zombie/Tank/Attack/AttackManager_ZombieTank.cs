﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MaruUtility;

public class AttackManager_ZombieTank : AttackNodeManagerBase
{
    [Serializable]
    struct Parametor {
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

    enum AttackType
    {
         None,
         Near,
         Tackle,   //タックル
         WaitSee,  //様子見
    }

    [SerializeField]
    Parametor m_param = new Parametor(2.0f, 10.0f, 1000.0f);

    TargetManager m_targetMgr;
    Stator_ZombieTank m_stator;
    AnimatorCtrl_ZombieTank m_animatorCtrl;
    EyeSearchRange m_eye;

    AttackType m_type = AttackType.None;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_animatorCtrl = GetComponent<AnimatorCtrl_ZombieTank>();
        m_eye = GetComponent<EyeSearchRange>();
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

    void NearAttackStart()
    {
        m_animatorCtrl.NearAttackTriggerFire();
        m_type = AttackType.Near;
    }

    void TackleAttackStart()
    {
        m_animatorCtrl.TackleTriggerFire();
        m_type = AttackType.Tackle;
    }

    void WaitSeeStart()
    {
        m_type = AttackType.WaitSee;
    }

    public override void EndAnimationEvent()
    {
        Debug.Log("EndAnimation");
        m_stator.GetTransitionMember().chaseTrigger.Fire();
        m_type = AttackType.None;
    }
}
