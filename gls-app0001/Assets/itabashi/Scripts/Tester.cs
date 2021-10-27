using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public GameObject focusObject;

    private void Awake()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(focusObject);
    }
}
