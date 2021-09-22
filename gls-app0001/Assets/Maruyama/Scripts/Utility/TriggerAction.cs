using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TriggerAction : MonoBehaviour
{
    Action<Collider> m_enterAction = null;
    Action<Collider> m_stayAction = null;
    Action<Collider> m_exitAction = null;

    //Enter-------------------------------------------------------

    public void AddEnterAction(Action<Collider> action)
    {
        m_enterAction += action;
    }

    public void SetEnterAction(Action<Collider> action)
    {
        m_enterAction = action;
    }

    //Stay--------------------------------------------------------

    public void AddStayAction(Action<Collider> action)
    {
        m_stayAction += action;
    }

    public void SetStayAction(Action<Collider> action)
    {
        m_stayAction = action;
    }

    //Exit--------------------------------------------------------

    public void AddExitAction(Action<Collider> action)
    {
        m_exitAction += action;
    }

    public void SetExitAction(Action<Collider> action)
    {
        m_exitAction = action;
    }

    //OnTrigger----------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        m_enterAction?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        m_stayAction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_exitAction?.Invoke(other);
    }
}
