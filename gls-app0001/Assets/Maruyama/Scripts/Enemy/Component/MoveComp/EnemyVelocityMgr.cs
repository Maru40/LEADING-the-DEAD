using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityMgr : MonoBehaviour
{
    Rigidbody m_rigid;

    Vector3 m_force = new Vector3();
    Vector3 m_velocity = new Vector3();

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //m_velocity = m_rigid.velocity;
        //return;

        m_velocity += m_force * Time.deltaTime;

        m_rigid.velocity = m_velocity;

        ResetForce();

        m_velocity.y = 0.0f;

        //float dampValue = 0.2f;  //減衰処理
        //m_force *= dampValue;
        //if (m_force.magnitude <= 0.1f)
        //{
        //    ResetForce();
        //}
    }


    //アクセッサ-------------------------------------------------------

    public Vector3 velocity
    {
        set { m_velocity = value; }
        get { return m_velocity; }
    }

    public void AddForce(Vector3 force)
    {
        //m_rigid.AddForce(force);
        m_force += force;
    }

    public Vector3 GetForce()
    {
        return m_force;
    }

    public void ResetVelocity()
    {
        m_velocity = Vector3.zero;
        m_rigid.velocity = Vector3.zero;
    }

    public void ResetForce()
    {
        m_force = Vector3.zero;
    }

}
