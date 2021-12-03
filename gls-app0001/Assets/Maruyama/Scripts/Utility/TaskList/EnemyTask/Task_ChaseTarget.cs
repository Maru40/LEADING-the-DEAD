using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Task_ChaseTarget : TaskNodeBase<EnemyBase>
{
    struct Parametor 
    {
        public float maxSpeed;
        /// <summary>
        /// //ターゲットとのフォワードの差がこの数字よりより小さければ、予測タックルにする。
        /// </summary>
        public float subPursuitTargetForward;
        public float nearRange;  //対象に追いついたと思う距離
    }

    Parametor m_param = new Parametor();

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    EnemyRotationCtrl m_rotationController;

    public Task_ChaseTarget(EnemyBase owner)
        :base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
    }

    public override void OnEnter()
    {
        //追いかけるアニメーションに変更


    }

    public override bool OnUpdate()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return true;
        }

        Move();
        Rotation();

        return IsEnd;
    }

    public override void OnExit()
    {

    }

    void Move()
    {
        var owner = GetOwner();

        var target = m_targetManager.GetNowTarget();
        var toVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //相手と自分のフォワードの差(dot)
        var relativeHeading = Vector3.Dot(owner.transform.forward, target.transform.forward);

        //予測シークの条件を満たせば、予測シーク。そうでなければ通常追従
        var force = CalcuVelocity.CalcuConditionPursuitForce( m_velocityManager.velocity, toVec, m_param.maxSpeed, 
            owner.gameObject, m_velocityManager.GetComponent<Rigidbody>(),
            relativeHeading, m_param.subPursuitTargetForward);

        m_velocityManager.AddForce(force);
    }

    //予測Forceを返す。

    void Rotation()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return;
        }

        var direct = (Vector3)m_targetManager.GetToNowTargetVector();
        m_rotationController.SetDirect(direct.normalized);
    }

    bool IsEnd
    {
        get
        {
            return IsNearRange();
        }
    }

    bool IsNearRange()
    {
        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //ターゲットが近くにいたら。
        return toTargetVec.magnitude < m_param.nearRange ? true : false;
    }
}
