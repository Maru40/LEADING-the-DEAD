using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

/// <summary>
/// 攻撃時の追従軸合わせ
/// </summary>
public class Task_AttackChase : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor : I_Random<Parametor>
    {
        public float moveSpeed;
        [Header("曲がれる角度")]
        public float turningDegree;
        [Header("近くのゾンビを避ける力")]
        public float nearAvoidVec;
        [Header("追従する時間")]
        public float chaseTime;
        public bool isTimer;
        [Header("攻撃時に鳴らしたい音")]
        public AudioManager audioManager;

        public System.Action enterAnimation;

        public Parametor(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.turningDegree = 0.0f;
            this.nearAvoidVec = 1.0f;
            this.chaseTime = 1.0f;
            this.isTimer = false;
            //this.endWaitTime = 0.1f;
            this.enterAnimation = null;
            this.audioManager = null;
        }

        public void Random(RandomRange<Parametor> range)
        {
            if (range.isActive == false) { return; }

            moveSpeed = UnityEngine.Random.Range(range.min.moveSpeed, range.max.moveSpeed);
            //endWaitTime = UnityEngine.Random.Range(range.min.endWaitTime, range.max.endWaitTime);
        }
    }

    private Parametor m_param = new Parametor();
    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }

    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private EyeSearchRange m_eyeRange;
    private StatusManagerBase m_statusManager;
    private ThrongManager m_throngManager;
    private EnemyRotationCtrl m_rotationController;

    private GameTimer m_timer = new GameTimer();

    public Task_AttackChase(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_eyeRange = owner.GetComponent<EyeSearchRange>();
        m_statusManager = owner.GetComponent<StatusManagerBase>();
        m_throngManager = owner.GetComponent<ThrongManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
    }

    public override void OnEnter()
    {
        SetForwardTarget();
        m_rotationController.enabled = true;

        m_param.enterAnimation?.Invoke();
        m_timer.ResetTimer(m_param.chaseTime);
        m_param.audioManager?.PlayRandomClipOneShot();
    }

    public override bool OnUpdate()
    {
        TargetChase();

        m_timer.UpdateTimer();

        return IsEnd;
    }

    public override void OnExit()
    {

    }

    /// <summary>
    /// 攻撃の途中までは敵を追従するため。
    /// </summary>
    private void TargetChase()
    {
        var owner = GetOwner();

        var positionCheck = m_targetManager.GetNowTargetPosition();
        if (positionCheck == null)
        {
            return;
        }
        var position = (Vector3)positionCheck;

        //視界外ならターンしない
        if (!m_eyeRange.IsInEyeRange(position))
        {
            Move(owner.transform.forward);
            ThrongAdjust();
            return;
        }

        var toVec = position - owner.transform.position;
        toVec.y = 0;
        Move(toVec);
        Rotation(toVec);
        ThrongAdjust();
    }

    /// <summary>
    /// 集団ベクトル調整
    /// </summary>
    private void ThrongAdjust()
    {
        if (m_throngManager == null)
        {
            return;
        }

        var avoidVec = m_throngManager.CalcuSumAvoidVector();
        if (avoidVec != Vector3.zero) //回避が必要なら
        {
            var velocity = m_velocityManager.velocity;
            Vector3 avoidForce = CalcuVelocity.CalucSeekVec(velocity, avoidVec, velocity.magnitude);
            m_velocityManager.AddForce(avoidForce);
        }
    }

    private void Move(Vector3 moveVec)
    {
        var owner = GetOwner();

        if (!CalcuVelocity.IsTurningVector(m_velocityManager.velocity, moveVec, m_param.turningDegree))
        {  //追従できる角度でないなら
            Debug.Log("曲がれない");
            moveVec = owner.transform.forward; //直進する。
        }

        float moveSpeed = m_param.moveSpeed * m_statusManager.GetBuffParametor().angerParam.speed;

        m_velocityManager.velocity = moveVec.normalized * moveSpeed;
    }

    private void Rotation(Vector3 direct)
    {
        m_rotationController.SetDirect(direct);
    }

    /// <summary>
    /// ターゲットの方向を向く処理
    /// </summary>
    private void SetForwardTarget()
    {
        var owner = GetOwner();

        var position = m_targetManager.GetNowTargetPosition();
        if (position != null)
        {
            var toVec = (Vector3)position - owner.transform.position;
            toVec.y = 0.0f;  //(yのベクトルを殺す。)
            owner.transform.forward = toVec.normalized;
        }
    }

    private bool IsEnd
    {
        get
        {
            if (m_param.isTimer)
            {
                return m_timer.IsTimeUp;
            }

            return false;
        }
    }

}
