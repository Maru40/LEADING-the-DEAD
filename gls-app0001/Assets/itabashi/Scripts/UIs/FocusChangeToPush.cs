using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusChangeToPush : MonoBehaviour
{
    [SerializeField]
    private GameObject m_pushActiveObject;

    public GameObject pushActiveObject => m_pushActiveObject;

    [SerializeField]
    private Selectable m_moveSelectableObject;
    
    public void NextPushFocus()
    {
        var popUpUI = m_pushActiveObject.GetComponent<PopUpUI>();

        if(popUpUI)
        {
            popUpUI.PopUp();
            return;
        }

        m_pushActiveObject.SetActive(true);
        GameFocusManager.PushFocus(m_moveSelectableObject.gameObject);
    }
}
