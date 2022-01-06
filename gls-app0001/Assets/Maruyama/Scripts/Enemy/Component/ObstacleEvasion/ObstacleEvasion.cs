using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using System.Linq;

public class ObstacleEvasion : MonoBehaviour
{
    //Rayの長さ
    [SerializeField]
    private float m_rayRange = 3.0f;
    //Rayの角度(配列の数だけ生成)
    [SerializeField]
    private float[] m_rayDegs = new float[] { +45.0f, -45.0f };

    [SerializeField]
    private float m_maxSpeed = 3.0f;

    private EnemyVelocityManager m_velocityManager;

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    private string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    private void Awake()
    {
        m_velocityManager = GetComponent<EnemyVelocityManager>();
    }

    private void Update()
    {
        EvasionUpdate();
    }

    private void EvasionUpdate()
    {
        var forces = new List<Vector3>();

        foreach (var deg in m_rayDegs)
        {
            var forward = transform.forward;
            var direct = Quaternion.AngleAxis(deg, Vector3.up) * forward; //フォワードを回転

            //Debug.DrawRay(transform.position + Vector3.up, direct, new Color(1.0f, 0.0f, 0.0f, 1.0f));

            var hitPoint = CalcuRayHitPoint(direct);  //ヒットした場所の取得
            if (hitPoint == null) {
                continue;
            }

            var toSelfVec = transform.position - (Vector3)hitPoint;
            forces.Add(CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toSelfVec, m_maxSpeed));
        }

        if (forces.Count == 0) {
            return;
        }

        forces.Sort((a, b) => (int)(b.magnitude - a.magnitude));
        var force = forces[0];

        m_velocityManager.AddForce(force);
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
        var colliders = Physics.OverlapSphere(transform.position, 1, obstacleLayer);
        if (Physics.Raycast(transform.position, direct,out hit, m_rayRange, obstacleLayer)) //障害物にヒットしたら
        {
            return hit.point;
        }

        return null;
    }
}
