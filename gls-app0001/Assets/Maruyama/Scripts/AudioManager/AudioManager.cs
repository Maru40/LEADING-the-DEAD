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
    [SerializeField]
    private bool m_isFadeOut = false;
    public bool IsFadeOut => m_isFadeOut;

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

    public void PlayOneShot(AudioClipParametor param, bool isUseGameAudioManager = false)
    {
        if (IsActive == false) {
            return;
        }

        //GameAudioManagerを利用するかどうか
        if (isUseGameAudioManager)
        {
            Manager.GameAudioManager.Instance.SEPlayOneShot(param.clip, CalcuRandomVolume(param));
        }
        else
        {
            //randomにvolumeとpitchを決める
            SettingRandom(param);

            m_audioSource?.PlayOneShot(param.clip);

            if (IsFadeOut)  //フェード処理を入れるなら
            {
                FadeOutStart();
            }
        }
    }

    /// <summary>
    /// 音を一度だけ鳴らす。
    /// </summary>
    public void PlayRandomClipOneShot(bool isUseGameAudioManager = false)
    {
        if(m_audioClipParams.Count == 0) {
            return;
        }

        var index = MyRandom.RandomValue(0, m_audioClipParams.Count);
        var param = m_audioClipParams[index];

        PlayOneShot(param, isUseGameAudioManager);
    }

    public void PlayAllOneShot(bool isUseGameAudioManager = false)
    {
        foreach (var param in m_audioClipParams)
        {
            PlayOneShot(param, isUseGameAudioManager);
        }
    }

    public void Stop()
    {
        m_audioSource.Stop();
    }

    public void FadeOutStart()
    {
        m_fade.FadeStart(AudioFade.FadeType.Out);
    }

    /// <summary>
    /// randomにvolumeとpitchを決める
    /// </summary>
    private void SettingRandom(AudioClipParametor param)
    {
        if (IsRandom)
        {
            m_audioSource.volume = CalcuRandomVolume(param);
            m_audioSource.pitch = CalcuRandomPitch(param);
        }
    }

    private float CalcuRandomVolume(AudioClipParametor param)
    {
        return param.volume* Random.Range(m_volumeRandomRange.min, m_volumeRandomRange.max);
    }

    private float CalcuRandomPitch(AudioClipParametor param)
    {
        return Random.Range(m_pitchRandomRange.min, m_pitchRandomRange.max);
    }
}
