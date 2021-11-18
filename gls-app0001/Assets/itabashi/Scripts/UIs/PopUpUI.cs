using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{
    [SerializeField]
    public GameObject firstSelectObject;

    public void Awake()
    {
        if(firstSelectObject == null)
        {
            firstSelectObject = gameObject;
        }
    }

    public void PopUp()
    {
        gameObject.SetActive(true);
        GameFocusManager.PushFocus(firstSelectObject);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameFocusManager.PopFocus();
    }
}
