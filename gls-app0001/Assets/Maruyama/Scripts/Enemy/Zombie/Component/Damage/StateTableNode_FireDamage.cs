using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTableNode_FireDamage : StateTableNodeBase<EnemyBase>
{
    private WaitTimer m_waitTimer;

    private GameObject m_createParticle;  //生成するparticle
    private DamageParticleManager m_particleManager; //particleマネージャ

    private float m_time = 0.0f;

    public StateTableNode_FireDamage(EnemyBase owner, GameObject createParticle, float time = 3.0f)
        :base(owner)
    { 
        m_waitTimer = owner.GetComponent<WaitTimer>();
        m_particleManager = owner.GetComponent<DamageParticleManager>();

        m_createParticle = createParticle;
        m_time = time;
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();

        m_waitTimer.AddWaitTimer(GetType(), m_time, End);  //指定時間後に終了関数を呼ぶ。
        m_particleManager.StartDamage(m_time, m_createParticle);  //particleマネージャにparticleの生成依頼。
    }

    public override void OnUpdate()
    {}

    //アクセッサ---------------------------------------------------

    public float Timer
    {
        set { m_time = value; }
        get { return m_time; }
    }
}
