using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeEffecter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_damageEffect;

    [SerializeField]
    private float m_probability = 0.6f;

    [SerializeField]
    private float m_maxY = 1.0f;

    [SerializeField]
    private float m_minY = -1.0f;

    [SerializeField]
    private float m_particleX = 3.85f;

    [SerializeField, Min(0)]
    private int m_breakParticleNum = 10;

    private void CreateParticle()
    {
        float y = Random.Range(m_minY, m_maxY);

        int i = Random.Range(0, 2);

        Vector3 position = transform.position;
        position.y += y;

        if (i == 0)
        {
            position.x += m_particleX;
        }
        else
        {
            position.x -= m_particleX;
        }

        Instantiate(m_damageEffect, position, transform.rotation);
    }

    public void Damage()
    {
        if (m_probability == 0.0f || Random.Range(0.0f, 1.0f) <= m_probability)
        {
            return;
        }

        CreateParticle();
    }

    public void Break()
    {
        for (int i = 0; i < m_breakParticleNum; ++i)
        {
            CreateParticle();
        }
    }
}
