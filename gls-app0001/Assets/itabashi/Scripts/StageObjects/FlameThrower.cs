using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : DangerObjectBase
{
    [SerializeField]
    private bool m_isShoot = false;

    [SerializeField]
    private ParticleSystem m_flameParticle;

    [SerializeField]
    private CapsuleCollider m_flameDamageTrigger;

    // Start is called before the first frame update
    void Start()
    {
        m_flameDamageTrigger.enabled = m_isShoot;

        if (m_isShoot)
        {
            m_flameParticle.Play();
            StartCoroutine(ShootStart());
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

        if(m_isShoot)
        {
            m_flameDamageTrigger.enabled = true;
            m_flameParticle.Play();
            StartCoroutine(ShootStart());
        }
        else
        {
            m_flameParticle.Stop();
            StartCoroutine(ShootEnd());
        }
    }

    private IEnumerator ShootStart()
    {
        float countTime = 0.0f;

        while (countTime < m_flameParticle.main.startLifetime.constant)
        {
            countTime += Time.deltaTime;

            float height = m_flameParticle.main.startSpeed.constant * countTime;

            m_flameDamageTrigger.height = height;

            m_flameDamageTrigger.center = new Vector3(0, 0, height * 0.5f);

            yield return null;
        }

        m_flameDamageTrigger.height = m_flameParticle.main.startSpeed.constant * m_flameParticle.main.startLifetime.constant;

        m_flameDamageTrigger.center = new Vector3(0, 0, m_flameDamageTrigger.height * 0.5f);
    }

    private IEnumerator ShootEnd()
    {
        float countTime = 0.0f;

        float endHeight = m_flameParticle.main.startSpeed.constant * m_flameParticle.main.startLifetime.constant;

        while (countTime < m_flameParticle.main.startLifetime.constant)
        {
            countTime += Time.deltaTime;

            float height = endHeight - m_flameParticle.main.startSpeed.constant * countTime;

            m_flameDamageTrigger.height = height;

            m_flameDamageTrigger.center = new Vector3(0, 0, endHeight - height * 0.5f);

            yield return null;
        }

        m_flameDamageTrigger.height = m_flameParticle.main.startSpeed.constant * m_flameParticle.main.startLifetime.constant;

        m_flameDamageTrigger.center = new Vector3(0, 0, endHeight);

        m_flameDamageTrigger.enabled = false;
    }
}
