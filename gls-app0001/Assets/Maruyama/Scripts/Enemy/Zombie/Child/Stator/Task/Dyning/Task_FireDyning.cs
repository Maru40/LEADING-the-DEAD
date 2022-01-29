using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_FireDining : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        public float time;
    }

    private Parametor m_param = new Parametor();
    private GameTimer m_timer = new GameTimer();

    private EnemyVelocityManager m_velocityManager;

    public Task_FireDining(EnemyBase owner, Parametor parametor)
        : base(owner)
    {
        m_param = parametor;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_velocityManager.StartDeseleration();
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
}
