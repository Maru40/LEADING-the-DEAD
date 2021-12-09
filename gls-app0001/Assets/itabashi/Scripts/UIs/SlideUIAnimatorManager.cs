using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Manager;
using UnityEngine.UI;

public class SlideUIAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private StageSelectSceneEvent m_sceneEvent;

    [SerializeField]
    private GameObject m_episodeTextObject;

    [SerializeField]
    private Image m_previewImage;

    private void Awake()
    {
        var baseLayer = SlideUITable.BaseLayer;

        var idleBehaviour = baseLayer.Idle.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        idleBehaviour.onStateEntered
            .Subscribe(_ => m_sceneEvent.enabled = true)
            .AddTo(this);

        var leftScrollOutBehaviour = baseLayer.LeftScroll.LeftScrollOut.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        leftScrollOutBehaviour.onStateEntered
            .Subscribe(_ => m_sceneEvent.enabled = false)
            .AddTo(this);

        var rightScrollInBehaviour = baseLayer.LeftScroll.RightScrollIn.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        rightScrollInBehaviour.onStateEntered
            .Subscribe(_ => SlideUIUpdate())
            .AddTo(this);

        var rightScrollOutBehaviour = baseLayer.RightScroll.RihgtScrollOut.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        rightScrollOutBehaviour.onStateEntered
            .Subscribe(_ => m_sceneEvent.enabled = false)
            .AddTo(this);

        var leftScrollInBehaviour = baseLayer.RightScroll.LeftScrollIn.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        leftScrollInBehaviour.onStateEntered
            .Subscribe(_ => SlideUIUpdate())
            .AddTo(this);
    }

    public void SlideUIUpdate()
    {
        if(GameStageManager.Instance.isSelectStage)
        {
            m_previewImage.gameObject.SetActive(true);
            m_episodeTextObject.SetActive(false);

            var stageData = GameStageManager.Instance.currentStageData;
            m_previewImage.sprite = stageData.sprite;
        }
        else
        {
            m_episodeTextObject.SetActive(true);
            m_previewImage.gameObject.SetActive(false);
        }
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
