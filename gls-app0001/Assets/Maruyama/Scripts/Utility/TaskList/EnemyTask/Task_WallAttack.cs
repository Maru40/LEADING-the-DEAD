using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Task_WallAttack : TaskNodeBase<EnemyBase>
{
    struct Parametor
    {
        
    }

    GameTimer m_timer = new GameTimer();

    public Task_WallAttack(EnemyBase owner)
        :base(owner)
    {

    }

    public override void OnEnter()
    {
        //アニメーションの変更

    }

    public override bool OnUpdate()
    {
        return true;
    }

    public override void OnExit()
    {

    }
}
