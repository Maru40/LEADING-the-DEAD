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
    private float m_maxSpeed = 3.0f;
    private float m_turningPower = 1.0f; //旋回する力

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
            m_chaseTarget.TargetLost();
        }
    }

    //ターゲットを追従する処理
    private void LinerTarget(FoundObject target)
    {
        Move(target.transform.position);
    }

    //見失った場所を探す。
    private void LinerLostPosition()
    {
        var lostPosition = m_targetManager.GetLostPosition();
        if (lostPosition == null)
        {
            m_chaseTarget.TargetLost();
            return;
        }

        Move((Vector3)lostPosition);
    }

    private void Move(Vector3 targetPosition)
    {
        Vector3 toVec = targetPosition - GetOwner().transform.position;
        float maxSpeed = m_maxSpeed * m_statusManager.GetBuffParametor().SpeedBuffMultiply;

        var type = (FoundType)m_targetManager.GetNowTargetType();
        var force = type switch {
            FoundType.Smell => CalcuVelocity.CalucArriveVec(m_velocityManager.velocity, toVec, maxSpeed),
            _ => CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, maxSpeed),
        };

        m_velocityManager.AddForce(force * m_turningPower);

        m_rotationCtrl.SetDirect(m_velocityManager.velocity);
    }

    //アクセッサ-----------------------------------------------------------------------------

    public void SetMaxSpeed(float speed) {
        m_maxSpeed = speed;
    }
}
