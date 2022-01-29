using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieChild_Death : EnemyStateNodeBase<EnemyBase>
{
    public struct Parametor
    {

    }

    private Parametor m_param = new Parametor();

    public StateNode_ZombieChild_Death(EnemyBase owner, Parametor parametor)
        :base(owner)
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
        Debug.Log("Death");
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
