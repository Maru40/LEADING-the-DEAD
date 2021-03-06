using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Manager;
using UnityEngine.UI;

public class SelectStageLabelAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private GameObject m_episodeLabel;

    [SerializeField]
    private GameObject m_numStageLabel;

    [SerializeField]
    private StageLabelImageUI m_stageLabelImage;

    [SerializeField]
    private NumberImage m_numberImage;

    private void Awake()
    {
        SelectStageLabelTable.BaseLayer.FadeIn.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator).onStateEntered
            .Subscribe(_ => StageLabelChange())
            .AddTo(this);
    }

    public void OnSelectStageChanged()
    {
        m_animator.Play(SelectStageLabelTable.BaseLayer.FadeOut.stateFullPath);
    }

    public void StageLabelChange()
    {
        if(GameStageManager.Instance.currentStageData == null)
        {
            m_episodeLabel.SetActive(true);
            m_numStageLabel.SetActive(false);
            return;
        }

        m_episodeLabel.SetActive(false);
        m_numStageLabel.SetActive(true);

        m_stageLabelImage.ChangeLabelImage();
        m_numberImage.value = GameStageManager.Instance.stageIndex + 1;
    }
}
