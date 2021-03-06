using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

using FoundType = FoundObject.FoundType;

/// <summary>
/// 直線的なターゲット追従
/// </summary>
public class LinerSeekTarget : NodeBase<EnemyBase>
{
    public enum MoveType
    {
        Seek,    //追従行動
        Arrive,  //到着行動
    }

    private float m_maxSpeed = 3.0f;
    private float m_turningPower = 1.0f; //旋回する力
    private MoveType m_moveType = MoveType.Seek;

    private ChaseTarget m_chaseTarget;
    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private EnemyRotationCtrl m_rotationCtrl;
    private StatusManagerBase m_statusManager;

    public LinerSeekTarget(EnemyBase owner)
        : this(owner,3.0f, 1.0f)
    { }

    public LinerSeekTarget(EnemyBase owner, float maxSpeed, float turningPower)
        : base(owner)
    {
        m_maxSpeed = maxSpeed;
        m_turningPower = turningPower;

        m_chaseTarget = owner.GetComponent<ChaseTarget>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_rotationCtrl = owner.GetComponent<EnemyRotationCtrl>();
        m_statusManager = owner.GetComponent<StatusManagerBase>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        UpdateMove();
    }

    public override void OnExit()
    {

    }

    private void UpdateMove()
    {
        Debug.Log("LinerTargret");

        //FoundObject target = m_targetManager.GetNowTarget();
        var position = m_targetManager.GetNowTargetPosition();
        if(position != null)
        {
            Move((Vector3)position);
        }
        else
        {
            m_chaseTarget.TargetLost("LinerSeek");
        }
    }

    private void Move(Vector3 targetPosition)
    {
        Vector3 toVec = targetPosition - GetOwner().transform.position;
        float maxSpeed = m_maxSpeed * m_statusManager.GetBuffParametor().SpeedBuffMultiply;

        var type = (FoundType)m_targetManager.GetNowTargetType();
        var force = type switch {
            FoundType.Smell => CalcuVelocity.CalucArriveVec(m_velocityManager.velocity, toVec, maxSpeed),
            FoundType.ChildZombie => CalcuVelocity.CalucArriveVec(m_velocityManager.velocity, toVec, maxSpeed),
            _ => CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, maxSpeed),
        };

        force.y = 0.0f;
        m_velocityManager.AddForce(force * m_turningPower);

        m_rotationCtrl.SetDirect(m_velocityManager.velocity);
    }

    //アクセッサ-----------------------------------------------------------------------------

    public void SetMaxSpeed(float speed) {
        m_maxSpeed = speed;
    }
}
