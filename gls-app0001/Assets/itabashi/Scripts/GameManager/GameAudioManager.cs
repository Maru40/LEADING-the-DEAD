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

        public static GameAudioManager Instance { private set; get; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void BGMPlay(AudioClip bgmClip)
        {
            if(m_bgmSource.isPlaying)
            {
                m_bgmSource.Stop();
            }

            m_bgmSource.clip = bgmClip;

            m_bgmSource.Play();
        }

        public void BGMStop()
        {
            m_bgmSource.Stop();
        }

        public void BGMPause()
        {
            m_bgmSource.Pause();
        }

        public void BGMUnPause()
        {
            m_bgmSource.UnPause();
        }

        public void SEPlayOneShot(AudioClip seClip, float volumeScale)
        {
            m_seSource.PlayOneShot(seClip, volumeScale);
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
            set => m_audioMixer.SetFloat(BGM_VOLUME_KEY, Mathf.Clamp(LeapToDB(value), DB_DEFAULT_MIN, DB_DEFAULT_MAX));
            get
            {
                m_audioMixer.GetFloat(BGM_VOLUME_KEY, out float value);
                return DBToLeap(value);
            }
        }

        public float SEVolume
        {
            set => m_audioMixer.SetFloat(SE_VOLUME_KEY, Mathf.Clamp(LeapToDB(value), DB_DEFAULT_MIN, DB_DEFAULT_MAX));
            get
            {
                m_audioMixer.GetFloat(SE_VOLUME_KEY, out float value);
                return DBToLeap(value);
            }
        }

        static private float LeapToDB(float value)
        {
            Debug.Log(DB_DEFAULT_MIN - DB_DEFAULT_MIN * value);
            return DB_DEFAULT_MIN - DB_DEFAULT_MIN * value;
        }

        static private float DBToLeap(float dB)
        {
            return (-DB_DEFAULT_MIN + dB) / -DB_DEFAULT_MIN;
        }

    }
}