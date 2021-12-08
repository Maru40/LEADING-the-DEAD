using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class GameStageBGMManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_gameStageBGM;

    [SerializeField]
    private AudioClip m_clearResultBGM;

    [SerializeField]
    private float m_fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameAudioManager.Instance.IsBGMPlaying)
        {
            GameAudioManager.Instance.BGMPlay(m_gameStageBGM, m_fadeTime);
        }
    }

    public void Claer()
    {
        GameAudioManager.Instance.BGMPlay(m_clearResultBGM);
    }

    public void BGMStop()
    {
        GameAudioManager.Instance.BGMStop(m_fadeTime);
    }
}
