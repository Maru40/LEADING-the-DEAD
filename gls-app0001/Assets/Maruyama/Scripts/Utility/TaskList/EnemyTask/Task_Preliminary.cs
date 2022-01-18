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
    [Header("予備動作中に出す音")]
    public List<AudioManager_Ex.Parametor> audioParams;

    public PreliminaryParametor(RandomRange timeRandomRange, float moveSpeed)
    {
        this.timeRandomRange = timeRandomRange;
        this.moveSpeed = moveSpeed;
        this.audioParams = null;
    }
}

/// <summary>
/// 予備動作
/// </summary>
public class Task_Preliminary : TaskNodeBase_Ex<EnemyBase>
{
    private PreliminaryParametor m_param = new PreliminaryParametor();

    private GameTimer m_timer = new GameTimer();

    private EnemyRotationCtrl m_rotationController;
    private TargetManager m_targetManager;
    private AudioManager_Ex m_audioManager;
    private EnemyVelocityManager m_velocityManager;

    public Task_Preliminary(EnemyBase owner, PreliminaryParametor param)
        : this(owner, param, new BaseParametor())
    { }

    public Task_Preliminary(EnemyBase owner, PreliminaryParametor param, BaseParametor baseParametor)
        : base(owner, baseParametor)
    {
        m_param = param;

        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_audioManager = owner.GetComponent<AudioManager_Ex>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //タイマーセット
        var time = m_param.timeRandomRange.RandomValue;
        m_timer.ResetTimer(time);

        m_rotationController.enabled = true;

        m_audioManager?.PlayRandomClipOneShot(m_param.audioParams);  //声を出す。
    }

    public override bool OnUpdate()
    {
        base.OnUpdate();

        m_timer.UpdateTimer();
        Move();
        Rotation();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        base.OnExit();

        m_audioManager?.FadeOutStart();
    }

    private void Move()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return;
        }

        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();

        var moveSpeed = m_param.moveSpeed;
        var force = CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toTargetVec, moveSpeed);
        m_velocityManager.AddForce(force);
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
