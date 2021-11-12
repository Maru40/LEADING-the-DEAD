using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SearchLightHetedObject : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Vector3> m_lightHitEvent = null;

    public void OnLightHited(Vector3 lightCenterPoint)
    {
        m_lightHitEvent?.Invoke(lightCenterPoint);
    }
}
