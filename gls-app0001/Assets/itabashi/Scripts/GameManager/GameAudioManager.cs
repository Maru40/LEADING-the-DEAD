using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Manager
{
    public class GameAudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer m_audioMixer;

        private const string MASTER_VOLUME_KEY = "MasterVolume";

        private const string BGM_VOLUME_KEY = "BGMVolume";

        private const string SE_VOLUME_KEY = "SEVolume";

        private const float DB_DEFAULT_MIN = -80.0f;

        private const float DB_DEFAULT_MAX = 20.0f;

        [Space]

        [SerializeField]
        private AudioSource m_bgmSource;

        [SerializeField]
        private AudioSource m_seSource;

        private float m_bgmVolume;

        public static GameAudioManager Instance { private set; get; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            m_bgmVolume = m_bgmSource.volume;
        }

        public bool IsBGMPlaying => m_bgmSource.isPlaying;

        public void BGMPlay(AudioClip bgmClip,float fadeTime = 0.0f)
        {
            if(m_bgmSource.isPlaying)
            {
                m_bgmSource.Stop();
            }

            m_bgmSource.clip = bgmClip;

            StartCoroutine(SoundFadeIn(fadeTime));
        }

        public void BGMStop(float fadeTime = 0.0f)
        {
            StartCoroutine(SoundFadeOut(fadeTime));
        }

        public void BGMPause()
        {
            m_bgmSource.Pause();
        }

        public void BGMUnPause()
        {
            m_bgmSource.UnPause();
        }

        public void SEPlayOneShot(AudioClip seClip, float volumeScale = 1.0f)
        {
            m_seSource.PlayOneShot(seClip, volumeScale);
        }

        private IEnumerator SoundFadeOut(float fadeTime)
        {
            float bgmVolume = m_bgmSource.volume;

            float countTime = 0.0f;

            while(countTime < fadeTime)
            {
                countTime += Time.unscaledDeltaTime;
                m_bgmSource.volume = bgmVolume * (1 - countTime / fadeTime);
                yield return null;
            }

            m_bgmSource.volume = 0.0f;
            m_bgmSource.Stop();
        }

        private IEnumerator SoundFadeIn(float fadeTime)
        {
            m_bgmSource.Play();

            float countTime = 0.0f;

            while(countTime < fadeTime)
            {
                countTime += Time.unscaledDeltaTime;
                m_bgmSource.volume = m_bgmVolume * countTime / fadeTime;
                yield return null;
            }
        }

        public float MasterVolume
        {
            set => m_audioMixer.SetFloat(MASTER_VOLUME_KEY, Mathf.Clamp(LeapToDB(value), DB_DEFAULT_MIN, DB_DEFAULT_MAX));
            get
            {
                m_audioMixer.GetFloat(MASTER_VOLUME_KEY, out float value);
                return DBToLeap(value);
            }
        }

        public float BGMVolume
        {
            set => m_audioMixer.SetFloat(BGM_VOLUME_KEY, LeapToDB(value));
            get
            {
                m_audioMixer.GetFloat(BGM_VOLUME_KEY, out float value);
                return DBToLeap(value);
            }
        }

        public float SEVolume
        {
            set => m_audioMixer.SetFloat(SE_VOLUME_KEY, LeapToDB(value));
            get
            {
                m_audioMixer.GetFloat(SE_VOLUME_KEY, out float value);
                return DBToLeap(value);
            }
        }

        static private float LeapToDB(float value)
        {
            return Mathf.Clamp(Mathf.Log10(Mathf.Clamp(value, 0f, 1f)) * 20f, DB_DEFAULT_MIN, DB_DEFAULT_MAX);
        }

        static private float DBToLeap(float dB)
        {
            return Mathf.Pow(10, dB / 20.0f);
        }

    }
}