using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StageSelectUIPresenter : MonoBehaviour
{
    [SerializeField]
    private StageSelecter m_stageSelecter;

    [SerializeField]
    private SlideUIAnimatorManager m_slideUIAnimatorManager;

    [SerializeField]
    private SelectStageLabelAnimatorManager m_selectStageLabelAnimatorManager;

    [SerializeField]
    private SelectDirectionAnimatorManager m_selectDirectionAnimatorManager;

    [SerializeField]
    private StagePointer m_stagePointer;

    private void Awake()
    {
        m_stageSelecter.OnSelectIndexIncrement
            .Subscribe(stageData =>
            {
                m_slideUIAnimatorManager.LeftScroll(stageData);
                m_selectStageLabelAnimatorManager.OnSelectStageChanged(stageData);
                m_selectDirectionAnimatorManager.OnStageChanged(stageData);
                m_stagePointer.OnStageChanged(stageData);
            })
            .AddTo(this);

        m_stageSelecter.OnSelectIndexDecrement
            .Subscribe(stageData =>
            {
                m_slideUIAnimatorManager.RightScroll(stageData);
                m_selectStageLabelAnimatorManager.OnSelectStageChanged(stageData);
                m_selectDirectionAnimatorManager.OnStageChanged(stageData);
                m_stagePointer.OnStageChanged(stageData);
            })
            .AddTo(this);
    }

}
