using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class EnemyVelocityMgr : MonoBehaviour
{
    Rigidbody m_rigid;

    Vector3 m_force = new Vector3();
    Vector3 m_velocity = new Vector3();

    bool m_isDeseleration = false;  //減速中かどうか
    float m_deselerationPower = 1.0f;

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //m_velocity = m_rigid.velocity;
        //return;

        //減速処理
        Deseleration();

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

    /// <summary>
    /// 減速処理
    /// </summary>
    void Deseleration()
    {
        if (!m_isDeseleration) {
            return;
        }

        //Debug.Log("Deseleration");

        var force = CalcuVelocity.CalucSeekVec(velocity, -velocity, velocity.magnitude * m_deselerationPower);
        AddForce(force);

        float stopSpeed = 0.3f;
        if (velocity.magnitude <= stopSpeed) {
            m_isDeseleration = false;
        }
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

    /// <summary>
    /// 減速開始
    /// </summary>
    /// <param name="power">減速する力</param>
    public void StartDeseleration(float power = 1.0f)
    {
        m_isDeseleration = true;
        m_deselerationPower = power;
    }

    public void SetIsDeseleration(bool isDeseleration)
    {
        m_isDeseleration = isDeseleration;
    }

    /// <summary>
    /// 減速の強さ
    /// </summary>
    public float deselerationPower
    {
        set { m_deselerationPower = value; }
        get { return m_deselerationPower; }
    }
}
