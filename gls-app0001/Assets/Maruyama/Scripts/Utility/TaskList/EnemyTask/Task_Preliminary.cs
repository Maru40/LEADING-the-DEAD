﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

[System.Serializable]
public struct PreliminaryParametor
{
    [Header("予備動作のランダム時間範囲")]
    public RandomRange timeRandomRange;
    [Header("予備動作の移動スピード")]
    public float moveSpeed;

    public PreliminaryParametor(RandomRange timeRandomRange, float moveSpeed)
    {
        this.timeRandomRange = timeRandomRange;
        this.moveSpeed = moveSpeed;
    }
}

/// <summary>
/// 予備動作
/// </summary>
public class Task_Preliminary : TaskNodeBase<EnemyBase>
{
    PreliminaryParametor m_param = new PreliminaryParametor();

    GameTimer m_timer = new GameTimer();

    EnemyRotationCtrl m_rotationController;
    TargetManager m_targetManager;

    public Task_Preliminary(EnemyBase owner, PreliminaryParametor param)
        :base(owner)
    {
        m_param = param;

        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_targetManager = owner.GetComponent<TargetManager>();
    }

    public override void OnEnter()
    {
        //アニメーション遷移


        //タイマーセット
        var time = m_param.timeRandomRange.RandomValue;
        m_timer.ResetTimer(time);

        m_rotationController.enabled = true;
    }

    public override bool OnUpdate()
    {
        m_timer.UpdateTimer();
        Rotation();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        //アニメーションの遷移

    }

    void Rotation()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return;
        }

        var direct = (Vector3)m_targetManager.GetToNowTargetVector();
        m_rotationController.SetDirect(direct.normalized);
    }
}
