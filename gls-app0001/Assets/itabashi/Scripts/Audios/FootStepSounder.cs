using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSounder : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource;

    [SerializeField]
    private AudioClip[] m_footStepSounds;

    public void SoundPlay()
    {
        if(m_footStepSounds.Length == 0)
        {
            return;
        }

        m_audioSource.PlayOneShot(m_footStepSounds[Random.Range(0, m_footStepSounds.Length)]);
    }
}
