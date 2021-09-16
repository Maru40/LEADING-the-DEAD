using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

/// <summary>
/// �^�[�Q�b�g�̒Ǐ]
/// </summary>
public class ChaseTarget : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed = 3.0f;

    LinerSeekTarget m_linerSeek;  //�����I�ɒǂ�������
    NavSeekTarget m_navSeek;      //��Q���Ȃǂ�����A�i�r���b�V���𗘗p���Ĉړ�����B

    //�R���|�[�l���g�n------------------

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
