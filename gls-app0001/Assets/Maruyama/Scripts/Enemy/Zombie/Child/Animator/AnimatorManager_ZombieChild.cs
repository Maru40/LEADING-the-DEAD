using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager_ZombieChild : AnimatorManagerBase
{
    private Rigidbody m_rigid;

    protected override void Awake()
    {
        base.Awake();

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

    public void CrossFadeCry(int layerIndex, float transitionTime = 0.25f)
    {
        CrossFadeState("Cry", layerIndex, transitionTime);
    }

    public int BaseLayerIndex => m_animator.GetLayerIndex("Base Layer");
}
