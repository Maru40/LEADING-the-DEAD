using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLightManager : MonoBehaviour
{
    [SerializeField]
    private SphereCollider m_lightHitTrigger;

    [SerializeField]
    private Light m_light;

    [SerializeField]
    private LayerMask m_groundLayer;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, m_light.range, m_groundLayer))
        {
            m_lightHitTrigger.transform.position = hitInfo.point;

            m_lightHitTrigger.enabled = true;

            m_lightHitTrigger.radius = Mathf.Tan(Mathf.Deg2Rad * m_light.spotAngle * 0.5f) * hitInfo.distance;
        }
        else
        {
            m_lightHitTrigger.enabled = false;
        }
    }
}
