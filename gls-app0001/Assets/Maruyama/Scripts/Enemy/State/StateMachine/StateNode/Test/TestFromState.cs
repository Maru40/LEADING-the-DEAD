using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFromState : EnemyStateNodeBase<TestEnemy>
{
    public TestFromState(TestEnemy owner)
        :base(owner)
    { }

    protected override void ReserveChangeComponents()
    {
    }

    public override void OnStart()
    {
        Debug.Log("StartFrom");
    }

    public override void OnUpdate()
    {
        Debug.Log("UpdateFrom");
    }

    public override void OnExit()
    {
        Debug.Log("ExitFrom");
    }
}
