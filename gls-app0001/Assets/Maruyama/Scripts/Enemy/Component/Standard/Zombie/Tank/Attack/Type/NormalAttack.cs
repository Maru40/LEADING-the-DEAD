using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class NormalAttack : AttackNodeBase
{
    //Stator_ZombieTank m_stator;
    AttackNodeManagerBase m_attackManager;
    TargetManager m_targetMgr;
    EnemyVelocityMgr m_velocityMgr;
    EyeSearchRange m_eyeRange;
    StatusManagerBase m_statusManager;
    ThrongManager m_throngManager;

    [SerializeField]
    float m_moveSpeed = 3.0f;

    [SerializeField]
    EnemyAttackTriggerAction m_hitBox = null;

    bool m_isTargetChase = true;  //攻撃の途中まではターゲットを追うようにするため。

    void Awake()
    {
        //m_stator = GetComponent<Stator_ZombieTank>();
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_targetMgr = GetComponent<TargetManager>();
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_statusManager = GetComponent<StatusManagerBase>();
        m_throngManager = GetComponent<ThrongManager>();

        m_hitBox.AddEnterAction(SendDamage);
    }

    void Update()
    {
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

        float moveSpeed = m_moveSpeed * m_statusManager.GetBuffParametor().angerParam.speed;
        var toVec = target.transform.position - transform.position;
        toVec.y = 0.0f;  //(yのベクトルを殺す。)

        m_velocityMgr.velocity = toVec.normalized * moveSpeed;
        ThrongAdjust(moveSpeed);

        //向きの調整
        transform.forward = toVec.normalized;
        //m_rotationCtrl.SetDirect(m_velocityMgr.velocity);
    }

    void ThrongAdjust(float moveSpeed)
    {
        if(m_throngManager == null) {
            return;
        }

        var avoidVec = m_throngManager.CalcuSumAvoidVector();
        if (avoidVec != Vector3.zero) //回避が必要なら
        {
            var velocity = m_velocityMgr.velocity;
            Vector3 avoidForce = CalcuVelocity.CalucSeekVec(velocity, avoidVec, velocity.magnitude);
            m_velocityMgr.AddForce(avoidForce);
        }
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
        m_isTargetChase = true;

        enabled = true;
    }

    override public void AttackHitStart()
    {
        Debug.Log("AttackHit");
        m_isTargetChase = false;
        m_hitBox.AttackStart();

        m_velocityMgr.StartDeseleration();
    }

    public override void AttackHitEnd()
    {
        Debug.Log("AttackHitEnd");
        m_hitBox.AttackEnd();
    }

    /// <summary>
    /// 相手にダメージを与える。
    /// </summary>
    private void SendDamage(Collider other)
    {
        if (other.gameObject == this.gameObject)
        {
            return;
        }

        var damage = other.GetComponent<AttributeObject.TakeDamageObject>();
        if (damage != null)
        {
            var power = GetBaseParam().damageData.damageValue * m_statusManager.GetBuffParametor().angerParam.attackPower;
            var data = new AttributeObject.DamageData(power);
            damage.TakeDamage(data);
        }
    }

    public override void EndAnimationEvent()
    {
        //m_stator.GetTransitionMember().chaseTrigger.Fire();
        m_attackManager.EndAnimationEvent();

        m_velocityMgr.SetIsDeseleration(false);
        m_velocityMgr.ResetForce();
        m_velocityMgr.ResetVelocity();

        enabled = false;
    }
}
