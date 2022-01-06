using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStopParticle : MonoBehaviour
{
    [Header("particleがストップする時間"), SerializeField]
    private float m_stopTime = 5.0f;
    [Header("particleがストップしてから消滅するまでの時間"), SerializeField]
    private float m_destoryTime = 3.0f;

    private GameTimer m_stopTimer = new GameTimer();
    private GameTimer m_destoryTimer = new GameTimer();

    private ParticleSystem m_particle;

    private void Awake()
    {
        m_particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        m_stopTimer.ResetTimer(m_stopTime, ParticleStop);
    }

    private void Update()
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
