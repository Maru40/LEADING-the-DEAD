using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatorCtrl_ZombieNormal : MonoBehaviour
{
    Rigidbody m_rigid;
    Animator m_animator;
    AngerManager m_angerManager;
    Stator_ZombieNormal m_stator;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        m_angerManager = GetComponent<AngerManager>();
        m_stator = GetComponent<Stator_ZombieNormal>();
    }

    void Update()
    {
        //仮歩き同期
        moveSpeed = m_rigid.velocity.magnitude;
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
    }

    public void EndAnger()
    {
        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }
}
