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
        var direct = m_rigid.velocity;
        direct.y = 0;
        transform.forward = direct.normalized;

        //Debug.Log("velocityRange" + m_rigid.velocity.magnitude);
    }
}
