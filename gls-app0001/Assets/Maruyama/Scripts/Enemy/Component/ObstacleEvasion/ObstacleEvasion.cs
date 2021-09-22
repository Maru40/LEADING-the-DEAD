using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ObstacleEvasion : MonoBehaviour
{
    //Rayの長さ
    [SerializeField]
    float m_rayRange = 3.0f;
    //Rayの角度(配列の数だけ生成)
    [SerializeField]
    float[] m_rayDegs = new float[] { +45.0f, -45.0f }; 

    [SerializeField]
    float m_maxSpeed = 3.0f;

    EnemyVelocityMgr m_velocityMgr;

    /// <summary>
    /// Rayの障害物するLayerの配列
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
            var direct = Quaternion.AngleAxis(deg, Vector3.up) * forward; //フォワードを回転

            var hitPoint = CalcuRayHitPoint(direct);  //ヒットした場所の取得
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
    /// Rayがあたった場所の取得
    /// </summary>
    /// <param name="direct">Rayを飛ばす方向</param>
    /// <returns>当たった場所</returns>
    Vector3? CalcuRayHitPoint(Vector3 direct)
    {
        var hit = new RaycastHit();

        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        if(Physics.Raycast(transform.position, direct,out hit, m_rayRange, obstacleLayer)) //障害物にヒットしたら
        {
            return hit.point;
        }

        return null;
    }
}
