using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

public class Task_WallAttack : TaskNodeBase_Ex<EnemyBase>
{
    [Serializable]
    public struct Parametor
    {
        [Header("攻撃時間")]
        public float time;
        [Header("最大スピード")]
        public float maxSpeed;
        public List<AudioManager_Ex.Parametor> audioParams;
        //public Action enterAnimation;

        public Parametor(float time, float maxSpeed, List<AudioManager_Ex.Parametor> audioParams)
        {
            this.time = time;
            this.maxSpeed = maxSpeed;
            this.audioParams = audioParams;
            //this.enterAnimation = action;
        }
    }

    private Parametor m_param = new Parametor();

    private GameTimer m_timer = new GameTimer();

    private EnemyVelocityManager m_velocityManager;
    private TargetManager m_targetManager;
    private EnemyRotationCtrl m_rotationController;
    private AudioManager_Ex m_audioManager;

    public Task_WallAttack(EnemyBase owner, Parametor param)
        :this(owner, param, new BaseParametor())
    { }

    public Task_WallAttack(EnemyBase owner, Parametor param, BaseParametor baseParametor)
         : base(owner, baseParametor)
    {
        m_param = param;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_audioManager = owner.GetComponent<AudioManager_Ex>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_audioManager?.PlayRandomClipOneShot(m_param.audioParams);
        //m_param.enterAnimation?.Invoke();
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
        base.OnUpdate();

        m_timer.UpdateTimer();

        Move();
        Rotation();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        base.OnExit();
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
