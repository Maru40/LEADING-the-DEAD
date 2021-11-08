using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStopParticle : MonoBehaviour
{
    [Header("particleがストップする時間"), SerializeField]
    float m_stopTime = 5.0f;
    [Header("particleがストップしてから消滅するまでの時間"), SerializeField]
    float m_destoryTime = 3.0f;

    GameTimer m_stopTimer = new GameTimer();
    GameTimer m_destoryTimer = new GameTimer();

    ParticleSystem m_particle;

    private void Awake()
    {
        m_particle = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        m_stopTimer.ResetTimer(m_stopTime, ParticleStop);
    }

    void Update()
    {
        m_stopTimer.UpdateTimer();
        m_destoryTimer.UpdateTimer();
    }

    private void ParticleStop()
    {
        m_particle.Stop();
        m_destoryTimer.ResetTimer(m_destoryTime,() => Destroy(gameObject));
    }
}
