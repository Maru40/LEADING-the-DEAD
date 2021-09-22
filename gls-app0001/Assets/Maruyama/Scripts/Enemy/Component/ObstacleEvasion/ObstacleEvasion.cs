using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ObstacleEvasion : MonoBehaviour
{
    //Ray�̒���
    [SerializeField]
    float m_rayRange = 3.0f;
    //Ray�̊p�x(�z��̐���������)
    [SerializeField]
    float[] m_rayDegs = new float[] { +45.0f, -45.0f }; 

    [SerializeField]
    float m_maxSpeed = 3.0f;

    EnemyVelocityMgr m_velocityMgr;

    /// <summary>
    /// Ray�̏�Q������Layer�̔z��
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    void Start()
    {
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
    }

    private void Update()
    {
        EvasionUpdate();
    }

    void EvasionUpdate()
    {
        var force = Vector3.zero;

        foreach (var deg in m_rayDegs)
        {
            var forward = transform.forward;
            var direct = Quaternion.AngleAxis(deg, Vector3.up) * forward; //�t�H���[�h����]

            var hitPoint = CalcuRayHitPoint(direct);  //�q�b�g�����ꏊ�̎擾
            if (hitPoint == null) {
                continue;
            }

            var toSelfVec = transform.position - (Vector3)hitPoint;
            var power = m_maxSpeed - toSelfVec.magnitude;
            var moveVec = toSelfVec.normalized * power;

            force += moveVec;
        }

        m_velocityMgr.AddForce(force);
    }

    /// <summary>
    /// Ray�����������ꏊ�̎擾
    /// </summary>
    /// <param name="direct">Ray���΂�����</param>
    /// <returns>���������ꏊ</returns>
    Vector3? CalcuRayHitPoint(Vector3 direct)
    {
        var hit = new RaycastHit();

        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        if(Physics.Raycast(transform.position, direct,out hit, m_rayRange, obstacleLayer)) //��Q���Ƀq�b�g������
        {
            return hit.point;
        }

        return null;
    }
}
