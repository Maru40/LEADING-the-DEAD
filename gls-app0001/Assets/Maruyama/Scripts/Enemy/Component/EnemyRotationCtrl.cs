using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy�̉�]���R���g���[������
/// </summary>
public class EnemyRotationCtrl : MonoBehaviour
{
    Rigidbody m_rigid;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //���ŉ�]����悤�ɂ����B
        //�����I�ɂ͂��������悤�ɒ���
        transform.forward = m_rigid.velocity;
    }
}
