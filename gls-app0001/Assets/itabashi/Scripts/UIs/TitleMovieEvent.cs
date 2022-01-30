using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitleMovieEvent : MonoBehaviour
{
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
