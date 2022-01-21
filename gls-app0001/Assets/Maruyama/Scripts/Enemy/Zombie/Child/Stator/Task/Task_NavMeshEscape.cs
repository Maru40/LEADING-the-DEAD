using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using System.Linq;

using MaruUtility;

public class Task_NavMeshEscape : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        [SerializeField, Range(5, 30)] 
        public float runAwayDistance;
        
    }

    private Parametor m_param = new Parametor();
    private Vector3 m_destination;  //目的地

    private NavMeshAgent m_navAgent;
    private TargetManager m_targetManager;

    public Task_NavMeshEscape(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_navAgent = owner.GetComponent<NavMeshAgent>();
        m_targetManager = owner.GetComponent<TargetManager>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (m_targetManager.HasTarget())
        {
            ChangeDestination(CalcuDestination());
        }
    }

    public override bool OnUpdate()
    {
        if (IsChangeDestination()) //目的地を変更するかどうか
        {
            ChangeDestination(CalcuDestination());
        }

        //if (m_navAgent.CalculatePath(CalcuDestination(), m_navAgent.path))
        //{
        //    ChangeDestination(CalcuDestination());
        //}

        return IsEnd();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    //行先を変更するかどうか
    private bool IsChangeDestination()
    {
        //「目的地に近い」かつ、「ターゲットに近い」
        return IsNearDestination() && IsNearTarget() ? true : false;
    }

    /// <summary>
    /// 目的地に近いかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsNearDestination()
    {
        var owner = GetOwner();
        const float NearRange = 0.5f;

        //目的地とのベクトルを取得
        var toDestinationVec = m_destination - owner.transform.position;

        //目的地と自分のポジションが近いかどうか
        return toDestinationVec.magnitude <= NearRange ? true : false;
    }

    /// <summary>
    /// ターゲットに近い
    /// </summary>
    /// <returns></returns>
    private bool IsNearTarget()
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        var owner = GetOwner();

        //ターゲットからプレイヤーの位置と自身までの距離を計算
        var target = m_targetManager.GetNowTarget();
        float toTargetDistance = Vector3.Distance(target.transform.position, owner.transform.position);

        //ターゲットが近くにいたら
        return toTargetDistance <= m_param.runAwayDistance ? true : false;
    }

    /// <summary>
    /// 目的地の計算
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcuDestination()
    {
        var owner = GetOwner();
        var target = m_targetManager.GetNowTarget();

        //方向を計算
        var direction = (owner.transform.position - target.transform.position).normalized;
        direction.y = owner.transform.position.y;
        var destination = owner.transform.position + (direction * m_param.runAwayDistance); //行先

        //パスが存在しない場合は
        if (!m_navAgent.CalculatePath(destination, m_navAgent.path))
        {
            const float Sides = 180;
            const float PI2 = Mathf.PI * 2.0f;
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < Sides; i++)
            {
                float rad = (PI2 / Sides) * i;

                var newDestination = Quaternion.AngleAxis(rad * Mathf.Rad2Deg, Vector3.up) * destination;
                //障害物があったら
                if (Obstacle.IsLineCastObstacle(owner.transform.position, newDestination))
                {
                    var hit = Obstacle.CalcuRayCastHit(owner.transform.position, destination);
                    var toHitDistance = Vector3.Distance(hit.collider.transform.position, owner.transform.position);

                    destination = destination.normalized * toHitDistance;
                }

                if (m_navAgent.CalculatePath(newDestination, m_navAgent.path))  //パスが存在するなら
                {
                    positions.Add(newDestination);
                }
            }

            if (positions.Count != 0)
            {
                var sortPositions = positions.OrderByDescending(position => (position - owner.transform.position).magnitude).ToList();
                destination = sortPositions[0];
            }
            else
            {
                return Vector3.zero;
            }
        }

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<BoxCollider>().enabled = false;
        cube.transform.position = destination;
        //GameObject.Destroy(cube, 1.0f);

        destination.y = owner.transform.position.y;

        return destination;
    }

    /// <summary>
    /// 行先の変更
    /// </summary>
    /// <param name="destination"></param>
    private void ChangeDestination(Vector3 destination)
    {
        m_destination = destination;
        m_navAgent.destination = (destination);
    }

    private bool IsEnd()
    {
        //ターゲットが無かったら、終了
        return !m_targetManager.HasTarget() ? true : false;
    }
}
