using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_WallRising : EnemyStateNodeBase<EnemyBase>
{
    enum TaskEnum
    {
           
    }

    public struct Parametor
    {
        public float speed;
    }

    private Parametor m_param = new Parametor();

    public StateNode_ZombieNormal_WallRising(EnemyBase owner)
        :this(owner,new Parametor())
    { }

    public StateNode_ZombieNormal_WallRising(EnemyBase owner, Parametor parametor)
        : base(owner)
    {
        m_param = parametor;
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
