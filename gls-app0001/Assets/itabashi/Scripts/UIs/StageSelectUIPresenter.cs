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

    private void Awake()
    {
        m_stageSelecter.OnSelectIndexIncrement
            .Subscribe(stageData => m_slideUIAnimatorManager.LeftScroll(stageData))
            .AddTo(this);

        m_stageSelecter.OnSelectIndexDecrement
            .Subscribe(stageData => m_slideUIAnimatorManager.RightScroll(stageData))
            .AddTo(this);
    }

}
