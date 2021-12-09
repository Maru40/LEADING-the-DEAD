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
        public float time;
        public float maxSpeed;
        public AudioManager audioManager;
        public Action enterAnimation;
    }

    Parametor m_param = new Parametor();

    GameTimer m_timer = new GameTimer();

    EnemyVelocityMgr m_velocityManager;
    TargetManager m_targetManager;
    EnemyRotationCtrl m_rotationController;

    public Task_WallAttack(EnemyBase owner, Parametor param)
        :base(owner)
    {
        m_param = param;

        m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
    }

    public override void OnEnter()
    {
        m_param.audioManager?.PlayOneShot();
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
        //Debug.Log("△WallAttack");
        m_timer.UpdateTimer();

        Move();
        Rotation();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {

    }

    void Move()
    {
        if (!m_targetManager.HasTarget()) {
            return;
        }

        var toVec = (Vector3)m_targetManager.GetToNowTargetVector();
        var force = CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, m_param.maxSpeed);

        m_velocityManager.AddForce(force);
    }

    void Rotation()
    {
        m_rotationController.SetDirect(m_velocityManager.velocity);
    }
}
