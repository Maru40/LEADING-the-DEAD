using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy�̉�]���R���g���[������
/// </summary>
public class EnemyRotationCtrl : MonoBehaviour
{
    [SerializeField]
    float m_rotationSpeed = 3.0f;

    Vector3 m_direct = new Vector3();

    Rigidbody m_rigid;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //���ŉ�]����悤�ɂ����B
        //�����I�ɂ͂��������悤�ɒ���
        //var direct = m_rigid.velocity;
        var direct = m_direct;
        direct.y = 0;
        //transform.forward = direct.normalized;
        if(direct != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                     Quaternion.LookRotation(direct),
                                     m_rotationSpeed * Time.deltaTime);
        }

        //Debug.Log("velocityRange" + m_rigid.velocity.magnitude);
    }

    //�A�N�Z�b�T------------------------------------------------------------

    public void SetDirect(Vector3 direct)
    {
        m_direct = direct;
    }
    public Vector3 GetDirect()
    {
        return m_direct;
    }

}
