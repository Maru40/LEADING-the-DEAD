using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Task_WallAttack : TaskNodeBase<EnemyBase>
{
    public struct Parametor
    {
        public float time;
    }

    Parametor m_param = new Parametor();

    GameTimer m_timer = new GameTimer();

    public Task_WallAttack(EnemyBase owner, Parametor param)
        :base(owner)
    {
        m_param = param;
    }

    public override void OnEnter()
    {
        //アニメーションの変更

        m_timer.ResetTimer(m_param.time);
    }

    public override bool OnUpdate()
    {
        m_timer.UpdateTimer();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        //アニメーションの変更


    }
}
