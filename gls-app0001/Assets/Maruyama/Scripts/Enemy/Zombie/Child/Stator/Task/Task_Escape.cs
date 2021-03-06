using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using System.Linq;

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
    private Vector3 m_evasionVec = new Vector3();

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
        //Move();
        //Rotation();
        if (m_timer.IsTimeUp)
        {
            Move();
            Rotation();
        }
        else
        {
            m_timer.UpdateTimer();
            Rotation();
            EvasionMove();
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

    private void EvasionMove()
    {
        var velocity = m_velocityManager.velocity;
        var force = CalcuVelocity.CalucSeekVec(velocity, m_evasionVec, m_param.maxSpeed);

        m_velocityManager.AddForce(force);
    }

    private void Rotation()
    {
        m_rotationController.SetDirect(m_velocityManager.velocity);
    }

    //壁回避ベクトルの取得
    private Vector3 CalcuObstacleEvasionVec()
    {
        var owner = GetOwner();

        var hits = new List<RaycastHit>()
        {
            Obstacle.CalcuRayCastHit(owner.transform.position, owner.transform.right), //右方向
            Obstacle.CalcuRayCastHit(owner.transform.position, -owner.transform.right) //左方向
        };

        //const float Sides = 360.0f;
        //for(int i = 0; i < Sides; i++)
        //{
        //    float angle = (Sides / 360.0f) * i;
        //    var rotQuat = Quaternion.AngleAxis(angle, Vector3.up);

        //    var direct = rotQuat * owner.transform.forward;
        //    hits.Add(Obstacle.CalcuRayCastHit(owner.transform.position, direct));
        //}

        var directs = new List<Vector3>();
        foreach (var hit in hits)
        {
            directs.Add(owner.transform.position - hit.point);
        }

        var sortDirects = directs.OrderByDescending(direct => direct.magnitude).ToList(); //降順整理

        GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = sortDirects[0] + owner.transform.position;
        Debug.Break();
        return sortDirects[0];
    }

    private void CollisionHit(Collision other)
    {
        if (!Obstacle.IsObstacle(other.gameObject)) {
            return;
        }

        if (m_timer.IsTimeUp)
        {
            //m_evasionVec = CalcuObstacleEvasionVec();
            m_evasionVec = CalcuVelocity.Reflection(m_velocityManager.velocity, other);
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
