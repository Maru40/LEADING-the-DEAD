using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AlarmObject : MonoBehaviour
{
    /// <summary>
    /// アラーム時になる音
    /// </summary>
    [SerializeField]
    private AudioClip m_alarmSound;

    /// <summary>
    /// アラーム時になる音の範囲
    /// </summary>
    [SerializeField]
    private Collider m_soundRange;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        
        if(m_soundRange)
        {
            m_soundRange.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlarmStart()
    {
        if (m_alarmSound)
        {
            m_audioSource.PlayOneShot(m_alarmSound);
        }

        if (m_soundRange)
        {
            m_soundRange.enabled = true;
        }
    }

    public void AlarmStop()
    {
        if (m_soundRange)
        {
            m_soundRange.enabled = false;
        }
    }
}
