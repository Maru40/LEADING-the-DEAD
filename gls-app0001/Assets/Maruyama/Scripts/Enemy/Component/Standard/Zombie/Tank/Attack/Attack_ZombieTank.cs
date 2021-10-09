using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MaruUtility;

public class Attack_ZombieTank : AttackBase
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
    EnemyVelocityMgr m_velocityManager;
    EnemyRotationCtrl m_rotationCtrl;
    AnimatorCtrl_ZombieTank m_animatorCtrl;

    AttackType m_attackType = AttackType.Charge;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_animatorCtrl = GetComponent<AnimatorCtrl_ZombieTank>();
    }

    void Update()
    {
        MoveProcess();

        RotationCtrl();
    }

    void MoveProcess()
    {
        //if(m_attackType == AttackType.Tackle)
        //{
        //    var target = m_targetMgr.GetNowTarget();
        //    var toVec = target.gameObject.transform.position - transform.position;
        //    var force = CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, GetBaseParam().moveSpeed);

        //    m_velocityManager.AddForce(force);
        //}
    }

    void RotationCtrl()
    {
        if (m_attackType == AttackType.Charge)
        {
            var target = m_targetMgr.GetNowTarget();
            var direction = target.transform.position - transform.position;
            transform.forward = direction.normalized;
        }
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
        m_velocityManager.ResetVelocity();

        m_animatorCtrl.TackleTriggerFire();
    }

    public void AddMoveForce()
    {
        var target = m_targetMgr.GetNowTarget();
        var toVec = target.gameObject.transform.position - transform.position;

        m_velocityManager?.AddForce(toVec.normalized * m_param.tackleSpeed);

        m_attackType = AttackType.Tackle;
    }

    public override void Attack()
    {
        
    }

    public override void AttackHitEnd()
    {

    }

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();

        m_attackType = AttackType.Charge;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //攻撃状態なら
        if(m_attackType == AttackType.Tackle)
        {
            var damage = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();
            damage?.TakeDamage(new AttributeObject.DamageData(GetBaseParam().power));
        }
    }
}
