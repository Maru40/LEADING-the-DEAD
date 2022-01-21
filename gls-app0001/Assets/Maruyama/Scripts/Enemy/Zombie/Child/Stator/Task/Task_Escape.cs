using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Task_Escape : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        public float maxSpeed;
        [Header("壁突っかかり回避時間")]
        public float wallEvasionTime;
    }

    private Parametor m_param = new Parametor();

    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private EnemyRotationCtrl m_rotationController;
    private CollisionAction m_collisionAction;

    private GameTimer m_timer = new GameTimer();
    private Vector3 m_reflectionVec = new Vector3();

    public Task_Escape(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();

        m_collisionAction = owner.GetComponent<CollisionAction>();
        m_collisionAction.AddEnterAction(CollisionHit);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_timer.ResetTimer(0.0f);
    }

    public override bool OnUpdate()
    {
        if (m_timer.IsTimeUp)
        {
            Move();
            Rotation();
        }
        else
        {
            m_timer.UpdateTimer();
            ReflectionMove();
        }

        return IsEnd();
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

        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();

        var velocity = m_velocityManager.velocity;
        var force = CalcuVelocity.CalucSeekVec(velocity ,-toTargetVec, m_param.maxSpeed);

        m_velocityManager.AddForce(force);
    }

    private void ReflectionMove()
    {
        var velocity = m_velocityManager.velocity;
        var force = CalcuVelocity.CalucSeekVec(velocity, m_reflectionVec, m_param.maxSpeed);

        m_velocityManager.AddForce(force);
    }

    private void Rotation()
    {
        m_rotationController.SetDirect(m_velocityManager.velocity);
    }

    private void CollisionHit(Collision other)
    {
        if (m_timer.IsTimeUp)
        {
            m_reflectionVec = CalcuVelocity.Reflection(m_velocityManager.velocity, other);
            m_timer.ResetTimer(m_param.wallEvasionTime);
        }
    }

    private bool IsEnd()
    {
        if (!m_targetManager.HasTarget()) {
            return true;
        }

        return false;
    }
}
