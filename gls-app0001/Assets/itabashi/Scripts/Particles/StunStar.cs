using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStar : MonoBehaviour
{
    [SerializeField]
    private Animation m_animation;

    [SerializeField]
    private List<ParticleSystem> m_particles;

    private void Awake()
    {
        foreach (var particle in m_particles)
        {
            particle.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        m_animation.Play();

        foreach(var particle in m_particles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }

    public void Stop()
    {
        m_animation.Stop();

        foreach(var particle in m_particles)
        {
            particle.gameObject.SetActive(false);
        }

    }
}
