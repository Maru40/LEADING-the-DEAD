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
        m_velocity += m_force * Time.deltaTime;

        m_rigid.velocity = new Vector3(m_velocity.x, m_rigid.velocity.y ,m_velocity.z);

        ResetForce();
    }


    //アクセッサ-------------------------------------------------------

    public void SetVelcoity(Vector3 velocity)
    {
        m_velocity = velocity;
    }
    public Vector3 GetVelocity()
    {
        return m_velocity;
    }

    public void AddForce(Vector3 force)
    {
        m_force += force;
    }


    public void ResetVelocity()
    {
        m_velocity = Vector3.zero;
    }

    public void ResetForce()
    {
        m_force = Vector3.zero;
    }

}
