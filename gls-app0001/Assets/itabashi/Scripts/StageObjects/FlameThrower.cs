using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField]
    private bool m_isShoot = false;

    [SerializeField]
    private ParticleSystem m_flameParticle;

    [SerializeField]
    private Collider m_flameDamageTrigger;

    // Start is called before the first frame update
    void Start()
    {
        m_flameDamageTrigger.enabled = m_isShoot;

        if (m_isShoot)
        {
            m_flameParticle.Play();
        }
        else
        {
            m_flameParticle.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsShootChange()
    {
        m_isShoot = !m_isShoot;

        m_flameDamageTrigger.enabled = m_isShoot;

        if(m_isShoot)
        {
            m_flameParticle.gameObject.SetActive(true);
        }
        else
        {
            m_flameParticle.gameObject.SetActive(false);
        }
    }
}
