using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TutorialOneEventer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_oneCallEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CallEvent()
    {
        if(enabled)
        {
            m_oneCallEvent?.Invoke();
            enabled = false;
        }
    }
}
