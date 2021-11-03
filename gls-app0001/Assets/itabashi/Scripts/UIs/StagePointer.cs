using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void OnStageChanged(StageSelecter.SelectStageData selectStageData)
    {
        if(selectStageData.stageData == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        m_rectTransform.anchoredPosition = selectStageData.stageData.stagePoint;

        m_stageNameText.text = selectStageData.stageData.StageName;
    }
}
