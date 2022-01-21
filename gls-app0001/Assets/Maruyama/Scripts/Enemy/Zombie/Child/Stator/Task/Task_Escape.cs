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
    }

    private Parametor m_param = new Parametor();

    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private EnemyRotationCtrl m_rotationController;

    public Task_Escape(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override bool OnUpdate()
    {
        Move();
        Rotation();

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
        var force = CalcuVelocity.CalucSeekVec(velocity ,toTargetVec, m_param.maxSpeed);

        m_velocityManager.AddForce(force);
    }

    private void Rotation()
    {
        m_rotationController.SetDirect(m_velocityManager.velocity);
    }

    private bool IsEnd()
    {
        if (!m_targetManager.HasTarget()) {
            return true;
        }

        return false;
    }
}
