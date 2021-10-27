using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioOptioner : MonoBehaviour
{
    [SerializeField]
    private AudioMixer m_audioMixer = null;

    private const string MASTER_VOLUME_KEY = "MasterVolume";

    private const string BGM_VOLUME_KEY = "BGMVolume";

    private const string SE_VOLUME_KEY = "SEVolume";

    private const float DB_DEFAULT_MIN = -80.0f;

    private const float DB_DEFAULT_MAX = 20.0f;

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
