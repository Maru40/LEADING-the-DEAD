using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class NavSeekTarget : UtilityEnemyBase
{
    TargetMgr m_targetMgr;
    NavMeshAgent m_navMesh;

    public NavSeekTarget(GameObject owner)
        :base(owner)
    {
        m_targetMgr = owner.GetComponent<TargetMgr>();
        m_navMesh = owner.GetComponent<NavMeshAgent>();
    }

    public void Move()
    {
        if (m_navMesh)
        {

        }
    }
}
