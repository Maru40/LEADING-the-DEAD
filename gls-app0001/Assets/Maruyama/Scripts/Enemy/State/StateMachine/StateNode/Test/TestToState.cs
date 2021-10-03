using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToState : EnemyStateNodeBase<TestEnemy>
{
    public TestToState(TestEnemy owner)
    : base(owner)
    { }

    protected override void ReserveChangeComponents()
    { 
    }

    public override void OnStart()
    {
        Debug.Log("StartTo");
    }

    public override void OnUpdate()
    {
        Debug.Log("UpdateTo");
    }

    public override void OnExit()
    {
        Debug.Log("ExitTo");
    }
}
