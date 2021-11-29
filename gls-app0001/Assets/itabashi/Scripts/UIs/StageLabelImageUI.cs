using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public class StageLabelImageUI : MonoBehaviour
{
    [SerializeField]
    private Image m_image;

    [SerializeField]
    private Sprite m_stageLabelSprite;

    [SerializeField]
    private Sprite m_tutorialLabelSprite;

    public void ChangeLabelImage()
    {
        m_image.sprite = GameStageManager.Instance.isTutorial ? m_tutorialLabelSprite : m_stageLabelSprite;
    }
}
