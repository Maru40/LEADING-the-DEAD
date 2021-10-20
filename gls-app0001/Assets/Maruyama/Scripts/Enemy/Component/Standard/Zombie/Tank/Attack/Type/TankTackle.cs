using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class TankTackle : AttackNodeBase
{
    enum State 
    {
        None,
        Charge,
        Tackle,
    }

    State m_state = State.None;
    Vector3 m_tackleDirect = new Vector3();

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    EnemyRotationCtrl m_rotationCtrl;
    Stator_ZombieTank m_stator;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_stator = GetComponent<Stator_ZombieTank>();
    }

    private void Update()
    {
        TackleMove();

        Rotation();
    }

    Vector3 CalcuToTargetVec()
    {
        var target = m_targetManager.GetNowTarget();
        var toVec = target.gameObject.transform.position - transform.position;
        return toVec;
    }

    void TackleMove()
    {
        if(m_state != State.Tackle) {  //Tackleでなかったら処理をしない
            return;
        }

        Debug.Log("Tackle");

        var velocity = m_velocityManager.velocity;

        var force = CalcuVelocity.CalucSeekVec(velocity, m_tackleDirect, 15.0f);
        m_velocityManager.AddForce(force);

        m_rotationCtrl.SetDirect(m_tackleDirect);
    }

    void Rotation()
    {
        if(m_state != State.Charge) {
            return;
        }

        var toVec = CalcuToTargetVec();
        m_rotationCtrl.SetDirect(toVec);
    }

    public override void AttackStart()
    {
        m_state = State.Charge;
        enabled = true;
    }

    public override void AttackHitStart()
    {
        m_state = State.Tackle;

        //タックルする方向を決める
        var target = m_targetManager.GetNowTarget();
        var toVec = target.gameObject.transform.position - transform.position;
        toVec.y = 0;
        m_tackleDirect = toVec;
    }

    public override void AttackHitEnd()
    {
        m_velocityManager.StartDeseleration();
        m_state = State.None;
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        FoundObject target = m_targetManager.GetNowTarget();
        if (target) {
            return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else {
            return false;
        }
    }

    public override void EndAnimationEvent()
    {
        Debug.Log("EndAnimation");
        m_stator.GetTransitionMember().chaseTrigger.Fire();

        m_state = State.Charge;

        enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //攻撃状態なら
        if (m_state == State.Tackle) {
            var damage = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();
            damage?.TakeDamage(GetBaseParam().damageData);
        }
    }
}
