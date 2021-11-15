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
            .Subscribe(_ =>
            {
                m_slideUIAnimatorManager.LeftScroll();
                m_selectStageLabelAnimatorManager.OnSelectStageChanged();
                m_selectDirectionAnimatorManager.OnStageChanged();
                m_stagePointer.OnStageChanged();
            })
            .AddTo(this);

        m_stageSelecter.OnSelectIndexDecrement
            .Subscribe(_ =>
            {
                m_slideUIAnimatorManager.RightScroll();
                m_selectStageLabelAnimatorManager.OnSelectStageChanged();
                m_selectDirectionAnimatorManager.OnStageChanged();
                m_stagePointer.OnStageChanged();
            })
            .AddTo(this);
    }

    private void Start()
    {
        m_selectStageLabelAnimatorManager.StageLabelChange();
        m_stagePointer.OnStageChanged();
    }
}
