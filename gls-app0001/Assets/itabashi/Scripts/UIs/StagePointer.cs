using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;


public class StagePointer : MonoBehaviour
{
    [SerializeField]
    private Image m_pointerImage;

    [SerializeField]
    private Text m_stageNameText;

    private RectTransform m_rectTransform;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void OnStageChanged()
    {
        StageData stageData = GameStageManager.Instance.currentStageData;

        if(stageData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        m_rectTransform.anchoredPosition = stageData.stagePoint;

        m_stageNameText.text = stageData.StageName;
    }
}
