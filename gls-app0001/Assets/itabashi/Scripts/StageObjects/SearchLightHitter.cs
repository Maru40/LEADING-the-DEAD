using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchLightHitter : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        var m_hetedObject = other.GetComponent<SearchLightHetedObject>();

        if(m_hetedObject)
        {
            m_hetedObject.OnLightHited(transform.position);
        }
    }
}
