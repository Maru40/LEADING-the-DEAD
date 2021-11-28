using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

[RequireComponent(typeof(AudioFade))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip m_audioClip;

    [SerializeField]
    bool m_isActive = true;
    public bool IsActive => m_isActive;

    [SerializeField]
    bool m_isRandom = true;
    public bool IsRandom => m_isRandom;

    [Header("ボリュームのランダム幅"), SerializeField]
    RandomRange m_volumeRandomRange = new RandomRange(0.5f, 1.0f);
    [Header("ピッチのランダム幅"), SerializeField]
    RandomRange m_pitchRandomRange = new RandomRange(0.5f, 0.7f);

    AudioSource m_audioSource;
    AudioFade m_fade;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_fade = GetComponent<AudioFade>();
    }

    /// <summary>
    /// 音を一度だけ鳴らす。
    /// </summary>
    public void PlayOneShot()
    {
        if (IsActive == false) {
            return;
        }

        //randomにvolumeとpitchを決める
        if (IsRandom)
        {
            m_audioSource.volume = Random.Range(m_volumeRandomRange.min, m_volumeRandomRange.max);
            m_audioSource.pitch = Random.Range(m_pitchRandomRange.min, m_pitchRandomRange.max);
        }

        m_audioSource?.PlayOneShot(m_audioClip);
    }

    public void Stop()
    {
        m_audioSource.Stop();
    }

    public void FadeOutStart()
    {
        m_fade.FadeStart(AudioFade.FadeType.Out);
    }
}
