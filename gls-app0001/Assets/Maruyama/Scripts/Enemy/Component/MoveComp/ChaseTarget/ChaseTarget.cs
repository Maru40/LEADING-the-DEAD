using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

/// <summary>
/// ターゲットの追従
/// </summary>
public class ChaseTarget : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed = 3.0f;

    LinerSeekTarget m_linerSeek;  //直線的に追いかける
    NavSeekTarget m_navSeek;      //障害物などがあり、ナビメッシュを利用して移動する。

    //コンポーネント系------------------

    NavMeshAgent m_navMesh;

    void Start()
    {
        m_navMesh = GetComponent<NavMeshAgent>();

        m_linerSeek = new LinerSeekTarget(gameObject, m_maxSpeed);
    }

    void Update()
    {
        MoveProcess();
    }

    void MoveProcess()
    {
        m_linerSeek.Move();
    }

}
