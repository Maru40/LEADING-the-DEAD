using System.Collections;
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
    [Header("最初に再生するアニメーション")]
    public System.Action enterAnimation;
    [Header("予備動作中に出す音")]
    public AudioManager audioManager;

    public PreliminaryParametor(RandomRange timeRandomRange, float moveSpeed)
    {
        this.timeRandomRange = timeRandomRange;
        this.moveSpeed = moveSpeed;
        this.enterAnimation = null;
        this.audioManager = null;
    }
}

/// <summary>
/// 予備動作
/// </summary>
public class Task_Preliminary : TaskNodeBase<EnemyBase>
{
    private PreliminaryParametor m_param = new PreliminaryParametor();

    private GameTimer m_timer = new GameTimer();

    private EnemyRotationCtrl m_rotationController;
    private TargetManager m_targetManager;

    public Task_Preliminary(EnemyBase owner, PreliminaryParametor param)
        :base(owner)
    {
        m_param = param;

        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_targetManager = owner.GetComponent<TargetManager>();
    }

    public override void OnEnter()
    {
        //タイマーセット
        var time = m_param.timeRandomRange.RandomValue;
        m_timer.ResetTimer(time);

        m_rotationController.enabled = true;
        m_param.enterAnimation?.Invoke();

        m_param.audioManager?.PlayRandomClipOneShot();  //声を出す。
    }

    public override bool OnUpdate()
    {
        //Debug.Log("△予備");

        m_timer.UpdateTimer();
        Rotation();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        m_param.audioManager?.FadeOutStart();
    }

    private void Rotation()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return;
        }

        var direct = (Vector3)m_targetManager.GetToNowTargetVector();
        m_rotationController.SetDirect(direct.normalized);
    }
}
