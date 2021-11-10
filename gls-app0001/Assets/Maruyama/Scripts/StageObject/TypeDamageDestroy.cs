﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;

public class TypeDamageDestroy : MonoBehaviour
{

    [SerializeField]
    List<DamageType> m_takeDamageTypes = new List<DamageType>();

    [SerializeField]
    List<GameObject> m_particles = new List<GameObject>();

    public void Damaged(DamageData data)
    {
        foreach(var type in m_takeDamageTypes)
        {
            if(data.type == type)
            {
                Damage(data);
                break;
            }
        }
    }

    void Damage(DamageData data)
    {
        CreateParticles();

        const float time = 0.1f;
        Destroy(gameObject, time);
    }

    void CreateParticles()
    {
        foreach (var particle in m_particles)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
