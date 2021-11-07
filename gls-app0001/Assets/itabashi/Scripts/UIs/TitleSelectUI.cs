using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleSelectUI : MonoBehaviour
{
    [SerializeField]
    private Button m_gameStartButton;

    [SerializeField]
    private Button m_optionButton;

    [SerializeField]
    private Button m_exitButton;

    private void Awake()
    {
        //EventSystem.current.SetSelectedGameObject(m_gameStartButton.gameObject);
    }
}
