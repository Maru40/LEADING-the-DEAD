using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using MaruUtility.Sound;

[RequireComponent(typeof(AudioFade))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClipParametor> m_audioClipParams = new List<AudioClipParametor>();

    [SerializeField]
    private bool m_isActive = true;
    public bool IsActive => m_isActive;

    [SerializeField]
    private bool m_isRandom = true;
    public bool IsRandom => m_isRandom;

    [Header("ボリュームのランダム幅"), SerializeField]
    private RandomRange m_volumeRandomRange = new RandomRange(0.5f, 1.0f);
    [Header("ピッチのランダム幅"), SerializeField]
    private RandomRange m_pitchRandomRange = new RandomRange(0.5f, 0.7f);

    private AudioSource m_audioSource;
    private AudioFade m_fade;

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

        var index = MyRandom.RandomValue(0, m_audioClipParams.Count);
        var param = m_audioClipParams[index];

        //randomにvolumeとpitchを決める
        if (IsRandom)
        {
            m_audioSource.volume = param.volume * Random.Range(m_volumeRandomRange.min, m_volumeRandomRange.max);
            m_audioSource.pitch = Random.Range(m_pitchRandomRange.min, m_pitchRandomRange.max);
        }
        
        m_audioSource?.PlayOneShot(param.clip);
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
