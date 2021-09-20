using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの回転をコントロールする
/// </summary>
public class EnemyRotationCtrl : MonoBehaviour
{
    [SerializeField]
    float m_rotationSpeed = 3.0f;

    Rigidbody m_rigid;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //仮で回転するようにした。
        //将来的にはゆっくり回るように調整
        var direct = m_rigid.velocity;
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
}
