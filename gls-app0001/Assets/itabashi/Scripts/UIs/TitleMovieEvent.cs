using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Manager;

public class TitleMovieEvent : MonoBehaviour
{
    [SerializeField]
    private float m_defaultMovieBGMVolume = 0.4f;

    [SerializeField]
    private EventVideoPlayer m_eventVideoPlayer;

    [SerializeField]
    private UnityEvent m_finishEvent;

    private bool m_isFinish = false;

    private UIControls m_uiControls;

    // Start is called before the first frame update
    void Start()
    {
        m_uiControls = new UIControls();

        this.RegisterController(m_uiControls);

        m_uiControls.UI.Submit.performed += context => FinishEvent(false);
    }

    private void LateUpdate()
    {
        m_eventVideoPlayer.SetVolume(m_defaultMovieBGMVolume * GameAudioManager.Instance.BGMVolume);
    }

    public void FinishEvent(bool forcedStop)
    {
        if (!forcedStop && (!m_eventVideoPlayer.IsPlaying || m_isFinish))
        {
            return;
        }

        m_isFinish = true;

        m_finishEvent.Invoke();

    }
}
