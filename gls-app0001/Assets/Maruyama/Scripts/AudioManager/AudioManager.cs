using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    struct RandomRange
    {
        public float min;
        public float max;

        public RandomRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [SerializeField]
    AudioClip m_audioClip;

    [SerializeField]
    bool m_isRandom = true;

    [Header("ボリュームのランダム幅"), SerializeField]
    RandomRange m_volumeRandomRange = new RandomRange(0.5f, 1.0f);
    [Header("ピッチのランダム幅"), SerializeField]
    RandomRange m_pitchRandomRange = new RandomRange(0.5f, 0.7f);

    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 音を一度だけ鳴らす。
    /// </summary>
    public void PlayOneShot()
    {
        //randomにvolumeとpitchを決める
        m_audioSource.volume = Random.Range(m_volumeRandomRange.min, m_volumeRandomRange.max);
        m_audioSource.pitch = Random.Range(m_pitchRandomRange.min, m_pitchRandomRange.max);

        m_audioSource?.PlayOneShot(m_audioClip);
    }
}
