using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Attack_ZombieTank : AttackBase
{
    enum AttackType
    {
         Charge,  //攻撃前の溜め
         Tackle   //タックル
    }

    TargetManager m_targetMgr;
    Stator_ZombieTank m_stator;
    EnemyVelocityMgr m_velocityManager;
    EnemyRotationCtrl m_rotationCtrl;

    AttackType m_attackType = AttackType.Charge;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_stator = GetComponent<Stator_ZombieTank>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
    }

    void Update()
    {
        RotationCtrl();
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

        m_velocityManager?.AddForce(toVec.normalized * GetBaseParam().moveSpeed);

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

    void RotationCtrl()
    {
        if(m_attackType == AttackType.Charge)
        {
            Debug.Log("Rotation");

            var target = m_targetMgr.GetNowTarget();

            var direction = target.transform.position - transform.position;

            transform.forward = direction.normalized;
        }

        //var direction = m_attackType switch
        //{
        //    AttackType.Charge => target.transform.position - transform.position,
        //    AttackType.Tackle => m_velocityManager.velocity,
        //    _ => Vector3.zero,
        //};

        //transform.forward = direction.normalized;
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
