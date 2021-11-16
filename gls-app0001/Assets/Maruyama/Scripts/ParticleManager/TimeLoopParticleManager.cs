using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLoopParticleManager : MonoBehaviour
{
    [SerializeField]
    float m_intervalTime = 1.0f;

    [SerializeField]
    ParticleManager.ParticleID m_particleID;
    [SerializeField]
    Vector3 m_paticleSize = Vector3.one;

    bool m_isActive = false;
    GameTimer m_timer = new GameTimer();

    void Update()
    {
        if (m_isActive == false) {
            return;
        }

        m_timer.UpdateTimer();

        if (m_timer.IsTimeUp)
        {
            ParticleManager.Instance.Play(m_particleID, transform.position, m_paticleSize);
            m_timer.ResetTimer(m_intervalTime);
        }
    }

    public void TimerStart()
    {
        m_isActive = true;

        m_timer.ResetTimer(m_intervalTime);
    }
}
