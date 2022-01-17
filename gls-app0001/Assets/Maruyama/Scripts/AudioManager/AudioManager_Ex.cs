using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using MaruUtility.Sound;

[RequireComponent(typeof(AudioFade))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager_Ex : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public AudioClip clip;
        public float volume;
        public bool isInterval;
        public float intervalTime;
        public bool isRandom;
        [Header("ボリュームのランダム幅")]
        public RandomRange volumeRandomRange;
        [Header("ピッチのランダム幅")]
        public RandomRange pitchRandomRange;
        public bool isFadeOut;
        public AudioFade.Parametor fadeParam;
    }

    [SerializeField]
    private bool m_isActive = true;
    public bool IsActive => m_isActive;

    [SerializeField]
    private List<Parametor> m_params = new List<Parametor>();
    public List<Parametor> parametors
    {
        get => new List<Parametor>(m_params);
        set => m_params = new List<Parametor>(value);
    }

    private AudioSource m_audioSource;
    private AudioFade m_fade;
    private AudioFade.Parametor m_lastFadeParam =  new AudioFade.Parametor();

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_fade = GetComponent<AudioFade>();
    }

    public void PlayOneShot(Parametor param, bool isUseGameAudioManager = false)
    {
        if (IsActive == false)
        {
            return;
        }

        m_fade.Stop();

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

            if (param.isFadeOut)  //フェード処理を入れるなら
            {
                FadeOutStart(param.fadeParam);
            }

            m_lastFadeParam = param.fadeParam;
        }
    }

    /// <summary>
    /// 音を一度だけ鳴らす。
    /// </summary>
    public void PlayRandomClipOneShot(bool isUseGameAudioManager = false)
    {
        PlayRandomClipOneShot(m_params, isUseGameAudioManager);
    }

    /// <summary>
    /// 音を一度だけ鳴らす。
    /// </summary>
    public void PlayRandomClipOneShot(List<Parametor> parametors, bool isUseGameAudioManager = false)
    {
        if (parametors.Count == 0)
        {
            return;
        }

        var index = MyRandom.RandomValue(0, parametors.Count);
        var param = parametors[index];

        PlayOneShot(param, isUseGameAudioManager);
    }

    public void PlayAllOneShot(bool isUseGameAudioManager = false)
    {
        PlayAllOneShot(m_params);
    }

    public void PlayAllOneShot(List<Parametor> parametors, bool isUseGameAudioManager = false)
    {
        foreach (var param in parametors)
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
        m_fade.FadeStart(AudioFade.FadeType.Out, m_lastFadeParam);
    }

    public void FadeOutStart(AudioFade.Parametor parametor)
    {
        m_fade.FadeStart(AudioFade.FadeType.Out, parametor);
    }

    /// <summary>
    /// randomにvolumeとpitchを決める
    /// </summary>
    private void SettingRandom(Parametor param)
    {
        if (param.isRandom)
        {
            m_audioSource.volume = CalcuRandomVolume(param);
            m_audioSource.pitch = CalcuRandomPitch(param);
        }
    }

    private float CalcuRandomVolume(Parametor param)
    {
        return Random.Range(param.volumeRandomRange.min, param.volumeRandomRange.max);
    }

    private float CalcuRandomPitch(Parametor param)
    {
        return Random.Range(param.pitchRandomRange.min, param.pitchRandomRange.max);
    }
}
