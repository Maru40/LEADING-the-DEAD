using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Button m_retryButton;

    [SerializeField]
    private Button m_goStageSelectButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(m_retryButton.gameObject);
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
