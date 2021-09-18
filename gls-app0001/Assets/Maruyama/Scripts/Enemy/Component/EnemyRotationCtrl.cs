using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの回転をコントロールする
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
        //仮で回転するようにした。
        //将来的にはゆっくり回るように調整
        var direct = m_rigid.velocity;
        direct.y = 0;
        transform.forward = direct.normalized;

        //Debug.Log("velocityRange" + m_rigid.velocity.magnitude);
    }
}
