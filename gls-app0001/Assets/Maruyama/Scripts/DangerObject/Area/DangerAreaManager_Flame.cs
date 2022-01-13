using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerAreaManager_Flame : DangerAreaManager
{
    [SerializeField]
    private ParticleSystem m_flameParticle = null;
    private CapsuleCollider m_collider = null;

    protected override void Awake()
    {
        base.Awake();

        m_collider = GetComponent<CapsuleCollider>();
    }

    protected override void Start()
    {
        base.Start();

        m_collider.height = m_flameParticle.main.startSpeed.constant * m_flameParticle.main.startLifetime.constant;
        m_collider.center = new Vector3(0, 0, m_collider.height * 0.5f);

        const float ScaleMagnification = 1.1f; //スケールの大きさの倍率
        m_collider.gameObject.transform.localScale = m_flameParticle.gameObject.transform.localScale * ScaleMagnification;
    }
}
