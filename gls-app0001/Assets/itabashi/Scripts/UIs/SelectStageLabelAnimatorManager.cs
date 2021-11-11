using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SelectStageLabelAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private GameObject m_episodeLabel;

    [SerializeField]
    private GameObject m_numStageLabel;

    [SerializeField]
    private StageSelecter m_stageSelecter;

    [SerializeField]
    private NumberImage m_numberImage;

    private StageSelecter.SelectStageData m_selectStageData;

    private void Awake()
    {
        SelectStageLabelTable.BaseLayer.FadeIn.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator).onStateEntered
            .Subscribe(_ => StageLabelChange())
            .AddTo(this);
    }

    public void OnSelectStageChanged(StageSelecter.SelectStageData selectStageData)
    {
        m_selectStageData = selectStageData;
        m_animator.Play(SelectStageLabelTable.BaseLayer.FadeOut.stateFullPath);
    }

    private void StageLabelChange()
    {
        if(m_selectStageData.stageData == null)
        {
            m_episodeLabel.SetActive(true);
            m_numStageLabel.SetActive(false);
            return;
        }

        m_episodeLabel.SetActive(false);
        m_numStageLabel.SetActive(true);

        m_numberImage.value = m_stageSelecter.selectIndex + 1;

        Debug.Log("呼ばれた");
    }
}
