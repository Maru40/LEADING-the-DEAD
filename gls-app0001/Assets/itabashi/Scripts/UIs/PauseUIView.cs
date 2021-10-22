using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseUIView : MonoBehaviour
{
    [SerializeField]
    private Button m_backGameButton;

    [SerializeField]
    private Button m_goStageSelectBotton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(m_backGameButton.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
