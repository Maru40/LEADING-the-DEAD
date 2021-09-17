using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class NavSeekTarget : NodeBase<EnemyBase>
{
    //目的地にたどり着いたと判定する範囲
    float m_nearRange = 3.0f;
    Vector3 m_targetPosition = new Vector3();

    TargetMgr m_targetMgr;
    NavMeshAgent m_navMesh;

    public NavSeekTarget(EnemyBase owner)
        :this(owner, 3.0f, 3.0f)
    { }

    public NavSeekTarget(EnemyBase owner, float speed, float nearRange)
        : base(owner)
    {
        m_nearRange = nearRange;

        m_targetMgr = owner.GetComponent<TargetMgr>();
        m_navMesh = owner.GetComponent<NavMeshAgent>();
        m_navMesh.speed = speed;
    }

    public override void OnStart()
    {
    }

    public override void OnUpdate()
    {
        SetNavMeshTargetPosition();
    }

    public override void OnExit()
    {

    }

    //目的地にたどり着いたかどうか
    bool IsRouteEnd()
    {
        var toVec = m_targetPosition - GetOwner().transform.localPosition;
        float nowRange = toVec.magnitude;

        return nowRange <= m_nearRange ? true : false;
    }

    //NavMeshを利用して目的地までのルートを計算
    void SetNavMeshTargetPosition()
    {
        //NavMeshの準備ができているなら。
        if (m_navMesh.pathStatus != NavMeshPathStatus.PathPartial)
        {
            var target = m_targetMgr.GetNowTarget();

            m_targetPosition = target.transform.localPosition;
            m_targetPosition.y = GetOwner().transform.position.y;  //高さの調整

            m_navMesh.SetDestination(m_targetPosition);
        }
    }
}
