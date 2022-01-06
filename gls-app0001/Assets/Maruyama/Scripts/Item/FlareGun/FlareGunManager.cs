using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareGunManager : MonoBehaviour
{
    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    private string[] m_rayObstacleLayerStrings = new string[] { "L_Ground" };

    [SerializeField]
    private GameObject m_inflenceArea = null;   //光の範囲を示す範囲

    [SerializeField]
    private BindActivateArea m_bindArea = null; //縛る範囲

    public void Bomb()
    {
        var direct = Vector3.down;

        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direct, out hit, obstacleLayer))
        {
            m_bindArea.GetAreaCenterObject().transform.position = hit.point;
            m_inflenceArea.gameObject.transform.position = hit.point;
        }
    }
}
