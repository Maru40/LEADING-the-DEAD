using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_firstSelectObject;

    public void Awake()
    {
        if(m_firstSelectObject == null)
        {
            m_firstSelectObject = gameObject;
        }
    }

    public void PopUp()
    {
        gameObject.SetActive(true);
        GameFocusManager.PushFocus(m_firstSelectObject);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameFocusManager.PopFocus();
    }
}
