using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

public class Task_WallAttack : TaskNodeBase<EnemyBase>
{
    [Serializable]
    public struct Parametor
    {
        [Header("攻撃時間")]
        public float time;
        [Header("最大スピード")]
        public float maxSpeed;
        public AudioManager audioManager;
        public Action enterAnimation;

        public Parametor(float time, float maxSpeed, AudioManager audioManager, Action action)
        {
            this.time = time;
            this.maxSpeed = maxSpeed;
            this.audioManager = audioManager;
            this.enterAnimation = action;
        }
    }

    private Parametor m_param = new Parametor();

    private GameTimer m_timer = new GameTimer();

    private EnemyVelocityManager m_velocityManager;
    private TargetManager m_targetManager;
    private EnemyRotationCtrl m_rotationController;

    public Task_WallAttack(EnemyBase owner, Parametor param)
        :base(owner)
    {
        m_param = param;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
    }

    public override void OnEnter()
    {
        m_param.audioManager?.PlayRandomClipOneShot();
        m_param.enterAnimation?.Invoke();
        m_timer.ResetTimer(m_param.time);

        if (m_targetManager.HasTarget())
        {
            var toVec = (Vector3)m_targetManager.GetToNowTargetVector();
            m_velocityManager.velocity = toVec.normalized * m_velocityManager.velocity.magnitude;
        }
        m_rotationController.enabled = true;
    }

    public override bool OnUpdate()
    {
        m_timer.UpdateTimer();

        Move();
        Rotation();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {

    }

    private void Move()
    {
        if (!m_targetManager.HasTarget()) {
            return;
        }

        var toVec = (Vector3)m_targetManager.GetToNowTargetVector();
        var force = CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, m_param.maxSpeed);
        force.y = 0.0f;

        m_velocityManager.AddForce(force);
    }

    private void Rotation()
    {
        m_rotationController.SetDirect(m_velocityManager.velocity);
    }
}
