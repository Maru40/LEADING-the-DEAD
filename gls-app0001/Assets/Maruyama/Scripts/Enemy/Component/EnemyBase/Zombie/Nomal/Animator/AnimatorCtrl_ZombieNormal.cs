using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AnimatorParameters_ZombieNormal
{
    Animator m_animator;

    public AnimatorParameters_ZombieNormal(Animator animator)
    {
        m_animator = animator;
    }

    public float moveSpeed
    {
        set { m_animator.SetFloat("moveSpeed", value); }
        get { return m_animator.GetFloat("moveSpeed"); }
    }
}

public class AnimatorCtrl_ZombieNormal : MonoBehaviour
{
    Rigidbody m_rigid;
    Animator m_animator;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
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
}
