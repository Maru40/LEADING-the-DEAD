using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatorCtrl_ZombieNormal : MonoBehaviour
{
    private float m_defaultAttackSpeed = 1.0f;
    private float m_baseMoveSpeed = 1.0f;
    public float BaseMoveSpeed
    {
        get => m_baseMoveSpeed;
        set => m_baseMoveSpeed = value;
    }

    private Rigidbody m_rigid;
    private Animator m_animator;
    public Animator animator
    {
        get => m_animator;
        set => m_animator = value;
    }

    private AngerManager m_angerManager;
    private Stator_ZombieNormal m_stator;
    private StatusManager_ZombieNormal m_statusManager;
    private EnemyRotationCtrl m_rotationController;

    private void Start()
    {
        m_rigid = GetComponentInChildren<Rigidbody>();
        m_animator = GetComponent<Animator>();
        m_angerManager = GetComponent<AngerManager>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_statusManager = GetComponent<StatusManager_ZombieNormal>();
        m_rotationController = GetComponent<EnemyRotationCtrl>();
    }

    private void Update()
    {
        if(m_animator == null)
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        //歩き同期
        moveSpeed = m_rigid.velocity.magnitude * BaseMoveSpeed;

        RotationUpdate();
    }

    private void RotationUpdate()
    {
        const float stoppingSpeed = 0.1f; //停止と判断するスピード
        //歩いていないかつ、ローテーション中なら
        if (moveSpeed <= stoppingSpeed && m_rotationController.IsRotation)
        {
            rotationSpeed = BaseMoveSpeed;
        }
        else
        {
            rotationSpeed = 0.0f;
        }
    }

    //アクセッサ---------------------------------------------------

    public float moveSpeed
    {
        set { m_animator.SetFloat("moveSpeed", value); }
        get { return m_animator.GetFloat("moveSpeed"); }
    }

    public float rotationSpeed
    {
        set { m_animator.SetFloat("rotationSpeed", value); }
        get { return m_animator.GetFloat("rotationSpeed"); }
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
}
