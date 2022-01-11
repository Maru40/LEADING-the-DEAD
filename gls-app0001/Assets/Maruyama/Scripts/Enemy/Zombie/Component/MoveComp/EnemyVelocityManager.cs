using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class EnemyVelocityManager : MonoBehaviour
{
    private Rigidbody m_rigid;

    private Vector3 m_force = new Vector3();
    private Vector3 m_velocity = new Vector3();

    private bool m_isDeseleration = false;  //減速中かどうか
    private float m_deselerationPower = 1.0f;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //減速処理
        Deseleration();

        m_velocity.y += m_rigid.velocity.y - m_velocity.y;
        //m_force.y += m_rigid.velocity.y;
        m_velocity += m_force * Time.deltaTime;

        if (UtilityMath.IsNaN(m_velocity)) {  //速度がNaNなら処理をしない。
            return;
        }
        
        m_rigid.velocity = m_velocity;

        ResetForce();

        //m_velocity.y = 0.0f;
    }

    /// <summary>
    /// 減速処理
    /// </summary>
    private void Deseleration()
    {
        if (!m_isDeseleration) {
            return;
        }

        var force = CalcuVelocity.CalucSeekVec(velocity, -velocity, velocity.magnitude * m_deselerationPower);
        AddForce(force);

        const float stopSpeed = 0.3f;
        if (velocity.magnitude <= stopSpeed) {
            m_isDeseleration = false;
            ResetVelocity();
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

    public void ResetAll()
    {
        ResetVelocity();
        ResetForce();
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
