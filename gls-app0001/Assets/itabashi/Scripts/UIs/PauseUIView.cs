using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Manager;


public class PauseUIView : MonoBehaviour
{
    [SerializeField]
    private Button m_backGameButton;

    [SerializeField]
    private Button m_goStageSelectBotton;

    //private void OnEnable()
    //{
    //    GameFocusManager.PushFocus(m_backGameButton.gameObject);
    //}

    //private void OnDisable()
    //{
    //    GameFocusManager.PopFocus();
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
