using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatorCtrl_ZombieNormal : MonoBehaviour
{
    float m_defaultAttackSpeed = 1.0f;
    float m_baseMoveSpeed = 1.0f;
    public float BaseMoveSpeed
    {
        get => m_baseMoveSpeed;
        set => m_baseMoveSpeed = value;
    }

    Rigidbody m_rigid;
    Animator m_animator;
    AngerManager m_angerManager;
    Stator_ZombieNormal m_stator;
    StatusManager_ZombieNormal m_statusManager;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        m_angerManager = GetComponent<AngerManager>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_statusManager = GetComponent<StatusManager_ZombieNormal>();
    }

    void Update()
    {
        //仮歩き同期
        moveSpeed = m_rigid.velocity.magnitude * BaseMoveSpeed;
    }

    //アクセッサ---------------------------------------------------

    public float moveSpeed
    {
        set { m_animator.SetFloat("moveSpeed", value); }
        get { return m_animator.GetFloat("moveSpeed"); }
    }

    public void AttackTriggerFire()
    {
        m_animator.SetTrigger("attackTrigger");
    }

    public void StartStun()
    {
        m_animator.SetBool("isStun", true);
    }

    public void EndStun()
    {
        m_animator.SetBool("isStun", false);
    }

    public void StartAnger()
    {
        m_animator.SetTrigger("angerTrigger");

        m_animator.SetFloat("attackSpeed", m_defaultAttackSpeed * m_angerManager.GetRiseParametor().attackAnimeSpeed);
    }

    public void EndAngerAnimation()
    {
        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }
}
