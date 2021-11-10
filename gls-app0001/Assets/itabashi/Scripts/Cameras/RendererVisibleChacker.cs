using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;

public class RendererVisibleChacker : MonoBehaviour
{
    [SerializeField]
    private GameObject m_checkObject;

    [SerializeField]
    private UnityEvent<bool> m_visibleChangeEvent = new UnityEvent<bool>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_checkObject)
        {
            m_visibleChangeEvent?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_checkObject)
        {
            m_visibleChangeEvent?.Invoke(false);
        }
    }
}
