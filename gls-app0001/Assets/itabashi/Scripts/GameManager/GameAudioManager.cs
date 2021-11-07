using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameAudioManager : MonoBehaviour
    {
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
    }
}