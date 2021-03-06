using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCtrl : MonoBehaviour
{
    //オブジェクトの参照
    [SerializeField]
    private GameObject m_target;

    //コンポーネントの参照
    private NavMeshAgent m_navMesh;

    //現在木曜にしているポジション
    private Vector3 m_targetPosition;

    //目的地についたと判断される距離
    [SerializeField]
    private float m_nearRange = 3.0f;

    private void Start()
    {
        if (m_target == null){
            m_target = GameObject.Find("Player");
        }

        m_navMesh = GetComponent<NavMeshAgent>();

        SetNavMeshTargetPosition();
    }


    private void Update()
    {
        if (IsRouteEnd()) {
            SetNavMeshTargetPosition();
        }
    }

    //目的地にたどり着いたかどうか
    private bool IsRouteEnd()
    { 
        var toVec = m_targetPosition - transform.localPosition;
        float nowRange = toVec.magnitude;

        return nowRange <= m_nearRange ? true : false;
    }

    //NavMeshを利用して目的地までのルートを計算
    private void SetNavMeshTargetPosition()
    {
        //NavMeshの準備ができているなら。
        if(m_navMesh.pathStatus != NavMeshPathStatus.PathPartial)
        {
            m_targetPosition = m_target.transform.localPosition;
            m_targetPosition.y = transform.position.y;  //高さの調整

            m_navMesh.SetDestination(m_targetPosition);
        }
    }
}
