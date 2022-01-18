using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_EnemyWait : TaskNodeBase_Ex<EnemyBase>
{
    public struct Parametor
    {

    }

    private Parametor m_param = new Parametor();

    private EnemyVelocityManager m_velocityManager;

    public Task_EnemyWait(EnemyBase owner)
        : this(owner, new Parametor(), new BaseParametor())
    { }

    public Task_EnemyWait(EnemyBase owner, Parametor parametor)
        : this(owner, parametor, new BaseParametor())
    { }

    public Task_EnemyWait(EnemyBase owner, BaseParametor baseParametor)
        : this(owner, new Parametor(), baseParametor)
    { }

    public Task_EnemyWait(EnemyBase owner, Parametor parametor, BaseParametor baseParametor)
        : base(owner, baseParametor)
    {
        m_param = parametor;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ObstacleEvasion>(), false, true);
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, true);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_velocityManager.StartDeseleration();
    }

    public override bool OnUpdate()
    {
        base.OnUpdate();

        if (!m_velocityManager.IsDeseleration)
        {
            m_velocityManager.ResetAll();
        }

        return true;
    }

    public override void OnExit()
    {
        base.OnExit();

        m_velocityManager.SetIsDeseleration(false);
    }
}
