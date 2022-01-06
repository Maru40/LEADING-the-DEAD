using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl_ZombieTank : MonoBehaviour
{
    Animator m_animator;
    EnemyVelocityManager m_velocityManager;
    
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_velocityManager = GetComponent<EnemyVelocityManager>();
    }

    void Update()
    {
        moveSpeed = m_velocityManager.velocity.magnitude;
    }

    public float moveSpeed
    {
        set { m_animator.SetFloat("moveSpeed", value); }
        get { return m_animator.GetFloat("moveSpeed"); }
    }

    public void NearAttackTriggerFire()
    {
        m_animator.SetTrigger("nearAttackTrigger");
    }

    public void TackleTriggerFire()
    {
        //m_animator.SetTrigger("tackleTrigger");
    }
}
