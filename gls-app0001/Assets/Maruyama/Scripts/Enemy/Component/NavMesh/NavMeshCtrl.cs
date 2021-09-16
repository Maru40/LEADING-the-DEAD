using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCtrl : MonoBehaviour
{
    //�I�u�W�F�N�g�̎Q��
    [SerializeField]
    GameObject m_target;

    //�R���|�[�l���g�̎Q��
    NavMeshAgent m_navMesh;

    //���ݖؗj�ɂ��Ă���|�W�V����
    Vector3 m_targetPosition;

    //�ړI�n�ɂ����Ɣ��f����鋗��
    [SerializeField]
    float m_nearRange = 3.0f;

    void Start()
    {
        if (m_target == null){
            m_target = GameObject.Find("Player");
        }

        m_navMesh = GetComponent<NavMeshAgent>();

        SetNavMeshTargetPosition();
    }


    void Update()
    {
        if (IsRouteEnd()) {
            SetNavMeshTargetPosition();
        }
    }

    //�ړI�n�ɂ��ǂ蒅�������ǂ���
    bool IsRouteEnd()
    { 
        var toVec = m_targetPosition - transform.localPosition;
        float nowRange = toVec.magnitude;

        return nowRange <= m_nearRange ? true : false;
    }

    //NavMesh�𗘗p���ĖړI�n�܂ł̃��[�g���v�Z
    void SetNavMeshTargetPosition()
    {
        //NavMesh�̏������ł��Ă���Ȃ�B
        if(m_navMesh.pathStatus != NavMeshPathStatus.PathPartial)
        {
            m_targetPosition = m_target.transform.localPosition;
            m_targetPosition.y = transform.position.y;  //�����̒���

            m_navMesh.SetDestination(m_targetPosition);
        }
    }
}
