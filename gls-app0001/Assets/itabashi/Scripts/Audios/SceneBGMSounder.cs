using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class SceneBGMSounder : MonoBehaviour
{
    [System.Serializable]
    public class BGMClip
    {
        public AudioClip bgmClip;
        public float fadeTime;
    }

    [SerializeField]
    private BGMClip m_startBGM = null;

    [SerializeField]
    private float m_fadeOutBGMTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameAudioManager.Instance.BGMPlay(m_startBGM.bgmClip, m_startBGM.fadeTime);
    }

    public void BGMStop()
    {
        GameAudioManager.Instance.BGMStop(m_fadeOutBGMTime);
    }

}
