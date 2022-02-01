using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class EventVideoPlayer : MonoBehaviour
{
    [SerializeField]
    VideoPlayer m_videoPlayer;

    [SerializeField]
    UnityEvent m_finishEvent;

    // Start is called before the first frame update
    void Start()
    {
        m_videoPlayer.loopPointReached += videoPlayer =>
        { m_finishEvent.Invoke(); Debug.Log("ムービーが終わりました"); };   
    }

    public bool IsPlaying => m_videoPlayer.isPlaying;

    public void SetVolume(float volume)
    {
        m_videoPlayer.SetDirectAudioVolume(0, volume);
    }
}
