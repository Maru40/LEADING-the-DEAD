using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SelectGroupAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private SelectStageLabelAnimatorManager m_labelAnimatorManager;

    [SerializeField]
    private SelectDirectionAnimatorManager m_directionAnimatorManager;

    [SerializeField]
    private StageSelectSceneEvent m_stageSelectEvent;

    [SerializeField]
    private SlideUIAnimatorManager m_slideUIAnimatorManager;

    private bool m_isFading = false;

    public bool isFading => m_isFading;

    private void Awake()
    {
        SelectGroupUIsTable.BaseLayer.Fadein.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator).onStateEntered
            .Subscribe(_ =>
            {
                m_labelAnimatorManager.StageLabelChange();
                m_directionAnimatorManager.ColorReset();
                m_slideUIAnimatorManager.SlideUIUpdate();
            })
            .AddTo(this);

        SelectGroupUIsTable.BaseLayer.NewState.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator).onStateEntered
            .Subscribe(_ =>
            {
                m_isFading = false;
                m_stageSelectEvent.enabled = true;
            })
            .AddTo(this);

    }

    public void SelectGroupAnimationPlay()
    {
        m_isFading = true;
        m_animator.Play(SelectGroupUIsTable.BaseLayer.Fadeout.stateFullPath);
        m_stageSelectEvent.enabled = false;
    }


}
