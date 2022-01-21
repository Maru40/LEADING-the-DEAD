using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager_ZombieChild : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    private Rigidbody m_rigid;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (m_animator == null)
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        //歩き同期
        moveSpeed = m_rigid.velocity.magnitude;
    }

    public float moveSpeed
    {
        set { m_animator.SetFloat("moveSpeed", value); }
        get { return m_animator.GetFloat("moveSpeed"); }
    }
}
