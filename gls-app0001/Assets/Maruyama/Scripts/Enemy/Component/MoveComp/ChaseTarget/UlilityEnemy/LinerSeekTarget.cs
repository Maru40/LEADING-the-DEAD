using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����I�ȃ^�[�Q�b�g�Ǐ]
/// </summary>
public class LinerSeekTarget : NodeBase<EnemyBase>
{
    float m_maxSpeed = 3.0f;
    float m_turningPower = 1.0f; //���񂷂��

    ChaseTarget m_chaseTarget;
    TargetMgr m_targetMgr;
    EnemyVelocityMgr m_velocityMgr;
    ThrongMgr m_throngMgr;

    public LinerSeekTarget(EnemyBase owner)
        : this(owner,3.0f, 1.0f)
    { }

    public LinerSeekTarget(EnemyBase owner, float maxSpeed, float turningPower)
        : base(owner)
    {
        m_maxSpeed = maxSpeed;
        m_turningPower = turningPower;

        m_chaseTarget = owner.GetComponent<ChaseTarget>();
        m_targetMgr = owner.GetComponent<TargetMgr>();
        m_velocityMgr = owner.GetComponent<EnemyVelocityMgr>();
        m_throngMgr = owner.GetComponent<ThrongMgr>();
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        UpdateMove();
    }

    public override void OnExit()
    {

    }


    void UpdateMove()
    {
        Debug.Log("LinerTargret");

        GameObject target = m_targetMgr.GetNowTarget();
        if (target) {
            Vector3 toVec = target.transform.position - GetOwner().transform.position;
            m_throngMgr.AvoidNearThrong(m_velocityMgr, toVec, m_maxSpeed, m_turningPower);
        }
        else {
            m_chaseTarget.TargetLost();
        }
    }


    //�A�N�Z�b�T-----------------------------------------------------------------------------

    public void SetMaxSpeed(float speed){
        m_maxSpeed = speed;
    }
}
