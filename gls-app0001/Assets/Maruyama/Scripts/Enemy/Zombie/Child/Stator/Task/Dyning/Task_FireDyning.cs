using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ParticleData = ParticleManager.ParticleData;

public class Task_FireDining : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        public ParticleData particleData;
        public float time;

        public List<AudioManager_Ex.Parametor> audioParams;
    }

    private Parametor m_param = new Parametor();
    private GameTimer m_timer = new GameTimer();

    private EnemyVelocityManager m_velocityManager;
    private AudioManager_Ex m_audioManager;

    public Task_FireDining(EnemyBase owner, Parametor parametor)
        : base(owner)
    {
        m_param = parametor;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_audioManager = owner.GetComponent<AudioManager_Ex>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        CreateParticle();
        m_velocityManager.StartDeseleration();
        m_audioManager.PlayRandomClipOneShot(m_param.audioParams);
        m_timer.ResetTimer(m_param.time);
    }

    public override bool OnUpdate()
    {
        m_timer.UpdateTimer();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        base.OnExit();

        m_velocityManager.SetIsDeseleration(false);  //減速終了
    }

    private void CreateParticle()
    {
        //var particle = ParticleManager.Instance.Play(m_param.particleData.id, m_param.particleData.CreatePosition);
        //particle.transform.parent = GetOwner().transform;
    }
}
