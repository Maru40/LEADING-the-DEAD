using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterAction : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent<Collision> m_enterAction = null;
    [SerializeField]
    protected UnityEvent<Collision> m_stayAction = null;
    [SerializeField]
    protected UnityEvent<Collision> m_exitAction = null;

    private void OnCollisionEnter(Collision collision)
    {
        m_enterAction?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        m_stayAction?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_exitAction?.Invoke(collision);
    }
}
