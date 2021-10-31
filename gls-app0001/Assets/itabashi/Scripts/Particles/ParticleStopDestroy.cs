using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStopDestroy : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] m_particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var particle in m_particleSystems)
        {
            if(!particle)
            {
                continue;
            }

            if(particle.isEmitting)
            {
                return;
            }
        }

        Destroy(gameObject);
    }
}
