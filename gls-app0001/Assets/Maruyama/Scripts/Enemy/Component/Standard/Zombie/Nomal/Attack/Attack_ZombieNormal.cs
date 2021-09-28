using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

public class Attack_ZombieNormal : AttackBase
{
    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;
    EnemyVelocityMgr m_velocityMgr;
    EnemyRotationCtrl m_rotationCtrl;
    EyeSearchRange m_eyeRange;
    ThrongManager m_throngManager;

    [SerializeField]
    EnemyAttackTriggerAction m_hitBox = null;

    bool m_isTargetChase = true;  //攻撃の途中まではターゲットを追うようにするため。

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_throngManager = GetComponent<ThrongManager>();

        m_hitBox.AddEnterAction(SendDamage);
    }

    void Update()
    {
        Debug.Log(m_isTargetChase);

        if (m_isTargetChase) 
        {
            TargetChase();
        }
    }

    /// <summary>
    /// 攻撃の途中までは敵を追従するため。
    /// </summary>
    void TargetChase()
    {
        var target = m_targetMgr.GetNowTarget();
        if (target == null) {
            return;
        }

        var velocity = m_velocityMgr.velocity;
        var toVec = target.transform.position - transform.position;
        //var throngVec = m_throngManager.CalcuThrongVector();
        var avoidVec = m_throngManager.CalcuSumAvoidVector();
        toVec += avoidVec;
        toVec.y = 0.0f;  //(yのベクトルを殺す。)
        //var force = CalcuVelocity.CalucSeekVec(velocity, toVec, GetBaseParam().moveSpeed);

        m_velocityMgr.velocity = toVec.normalized * GetBaseParam().moveSpeed;
        //m_velocityMgr.AddForce(force);
        m_rotationCtrl.SetDirect(m_velocityMgr.velocity);
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
            //return m_eyeRange.IsInEyeRange(target.gameObject, range);
        }
        else
        {
            return false;
        }
    }

    override public void Attack(){
        //Debug.Log("Attack");

        m_isTargetChase = false;
        m_hitBox.AttackStart();
    }

    public override void AttackHitEnd()
    {
        m_hitBox.AttackEnd();
    }


    /// <summary>
    /// 相手にダメージを与える。
    /// </summary>
    private void SendDamage(Collider other)
    {
        //Debug.Log("Attack_Hit");

        if(other.gameObject == this.gameObject) {
            return;
        }

        var damage = other.GetComponent<AttributeObject.TakeDamageObject>();
        if (damage != null)
        {
            var data = new AttributeObject.DamageData((int)GetBaseParam().power);
            damage.TakeDamage(data);
        }
    }

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();
        m_isTargetChase = true;
    }
}
