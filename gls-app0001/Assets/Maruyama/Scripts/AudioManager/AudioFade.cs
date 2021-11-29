﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioManager))]
public class AudioFade : MonoBehaviour
{
    public enum FadeType {
        In,
        Out,
    }

    [System.Serializable]
    struct Parametor 
    {
        public float m_inTime;   //フェードインに掛ける時間
        public float m_outTime;  //フェードアウトに掛ける時間

        public float m_initVolume;  //初期ボリューム
    }

    [SerializeField]
    Parametor m_param = new Parametor();

    AudioSource m_audioSource;

    GameTimer m_timer = new GameTimer();

    System.Action m_updateAction = null;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        enabled = false;

        m_param.m_initVolume = m_audioSource.volume;
    }

    void Update()
    {
        m_timer.UpdateTimer();
        m_updateAction?.Invoke();
    }

    void FadeInUpdate()
    {

    }

    void FadeOutUpdate()
    {
        m_timer.UpdateTimer();

        m_audioSource.volume = m_param.m_initVolume - m_timer.TimeRate;
    }

    public void FadeStart(FadeType type)
    {
        enabled = true;

        FadeInit(type);
    }

    /// <summary>
    /// フェード準備
    /// </summary>
    /// <param name="type">フェードタイプ</param>
    void FadeInit(FadeType type)
    {
        m_timer.AbsoluteEndTimer(true);

        switch(type)
        {
            case FadeType.In:
                m_updateAction = FadeInUpdate;
                m_audioSource.volume = 0.0f;
                break;
            case FadeType.Out:
                m_updateAction = FadeOutUpdate;
                m_param.m_initVolume = m_audioSource.volume;
                m_timer.ResetTimer(m_param.m_outTime,() => FadeEnd(type));
                break;
        }
    }

    /// <summary>
    /// フェード終了
    /// </summary>
    /// <param name="type">フェードタイプ</param>
    void FadeEnd(FadeType type)
    {
        switch (type)
        {
            case FadeType.In:

                break;
            case FadeType.Out:
                m_audioSource.Stop();
                m_audioSource.volume = m_param.m_initVolume;
                enabled = false;
                m_updateAction = null;
                break;
        }
    }
}