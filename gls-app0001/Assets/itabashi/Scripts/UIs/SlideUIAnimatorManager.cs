using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SlideUIAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private StageSelectSceneEvent m_sceneEvent;

    private void Awake()
    {
        var baseLayer = SlideUITable.BaseLayer;

        var idleBehaviour = baseLayer.Idle.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        idleBehaviour.onStateEntered.Subscribe(_ => m_sceneEvent.enabled = true);

        var leftScrollBehaviour = baseLayer.LeftScroll.LeftScrollOut.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        leftScrollBehaviour.onStateEntered.Subscribe(_ => m_sceneEvent.enabled = false);

        var rightScrollBehaviour = baseLayer.RightScroll.RihgtScrollOut.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        rightScrollBehaviour.onStateEntered.Subscribe(_ => m_sceneEvent.enabled = false);
    }

    public void LeftScroll()
    {
        m_animator.Play(SlideUITable.BaseLayer.LeftScroll.LeftScrollOut.stateFullPath);
    }

    public void RightScroll()
    {
        m_animator.Play(SlideUITable.BaseLayer.RightScroll.RihgtScrollOut.stateFullPath);
    }
}
