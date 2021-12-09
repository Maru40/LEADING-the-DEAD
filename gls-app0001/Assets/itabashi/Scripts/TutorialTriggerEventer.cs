using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerEventer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_enterEvent;

    [SerializeField]
    private GameObject m_playerObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_playerObject)
        {
            m_enterEvent?.Invoke();
        }
    }
}
