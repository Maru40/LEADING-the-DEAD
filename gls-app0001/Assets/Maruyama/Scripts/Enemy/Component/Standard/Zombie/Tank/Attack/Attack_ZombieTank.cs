using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MaruUtility;

public class Attack_ZombieTank : AttackNodeManagerBase
{
    [Serializable]
    struct Parametor {
        public float nearRange;

        public float normalSpeed;
        public float tackleSpeed;

        public Parametor(float nearRange, float normalSpeed, float tackleSpeed)
        { 
            this.nearRange = nearRange;
            this.normalSpeed = normalSpeed;
            this.tackleSpeed = tackleSpeed;
        }
    }

    enum AttackType
    {
         Charge,  //攻撃前の溜め
         Tackle   //タックル
    }

    [SerializeField]
    Parametor m_param = new Parametor(2.0f, 10.0f, 1000.0f);

    TargetManager m_targetMgr;
    Stator_ZombieTank m_stator;
    AnimatorCtrl_ZombieTank m_animatorCtrl;
    EyeSearchRange m_eye;
    TankTackle m_tankTackle;

    AttackType m_attackType = AttackType.Charge;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_animatorCtrl = GetComponent<AnimatorCtrl_ZombieTank>();
        m_eye = GetComponent<EyeSearchRange>();
        m_tankTackle = GetComponent<TankTackle>();
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
        m_stator.GetTransitionMember().attackTrigger.Fire();

        FoundObject target = m_targetMgr.GetNowTarget();
        if (Calculation.IsRange(gameObject, target.gameObject, m_param.nearRange)) {
            NearAttack();
        }
        else {
            TackleAttack();
        }
    }

    void NearAttack()
    {
        m_animatorCtrl.NearAttackTriggerFire();
    }

    void TackleAttack()
    {
        m_animatorCtrl.TackleTriggerFire();
    }

    public override void EndAnimationEvent()
    {
        Debug.Log("EndAnimation");
        m_stator.GetTransitionMember().chaseTrigger.Fire();

        m_attackType = AttackType.Charge;
    }
}
