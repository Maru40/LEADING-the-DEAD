using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

public class NormalAttack : AttackNodeBase
{
    [Serializable]
    public struct Parametor : I_Random<Parametor>
    {
        public float moveSpeed;
        [Header("曲がれる角度")]
        public float turningDegree; 
        [Header("近くのゾンビを避ける力")]
        public float nearAvoidVec;
        [Header("終了時にストップする時間")]
        public float endWaitTime;

        public Parametor(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.turningDegree = 0.0f;
            this.nearAvoidVec = 1.0f;
            this.endWaitTime = 0.1f;
        }

        public void Random(RandomRange<Parametor> range)
        {
            if (range.isActive == false) { return; }

            moveSpeed = UnityEngine.Random.Range(range.min.moveSpeed, range.max.moveSpeed);
            endWaitTime = UnityEngine.Random.Range(range.min.endWaitTime, range.max.endWaitTime);
        }
    }

    private AttackNodeManagerBase m_attackManager;
    private TargetManager m_targetMgr;
    private EnemyVelocityManager m_velocityMgr;
    private EyeSearchRange m_eyeRange;
    private StatusManagerBase m_statusManager;
    private ThrongManager m_throngManager;
    private EnemyRotationCtrl m_rotationController;
    private WaitTimer m_waitTimer;

    [SerializeField]
    private Parametor m_param = new Parametor(3.0f);

    [SerializeField]
    private AudioManager m_audioManager = null;

    private bool m_isTargetChase = true;  //攻撃の途中まではターゲットを追うようにするため。

    private void Awake()
    {
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_targetMgr = GetComponent<TargetManager>();
        m_velocityMgr = GetComponent<EnemyVelocityManager>();
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_statusManager = GetComponent<StatusManagerBase>();
        m_throngManager = GetComponent<ThrongManager>();
        m_rotationController = GetComponent<EnemyRotationCtrl>();
        m_waitTimer = GetComponent<WaitTimer>();
    }

    private void Update()
    {
        if (m_isTargetChase)
        {
            TargetChase();
        }
    }

    /// <summary>
    /// 攻撃の途中までは敵を追従するため。
    /// </summary>
    private void TargetChase()
    {
        var positionCheck = m_targetMgr.GetNowTargetPosition();
        if (positionCheck == null) {
            return;
        }
        var position = (Vector3)positionCheck;

        //視界外ならターンしない
        if (!m_eyeRange.IsInEyeRange(position))
        {
            Move(transform.forward);
            ThrongAdjust();
            return;
        }

        var toVec = position - transform.position;
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
        if(m_throngManager == null) {
            return;
        }

        var avoidVec = m_throngManager.CalcuSumAvoidVector();
        if (avoidVec != Vector3.zero) //回避が必要なら
        {
            var velocity = m_velocityMgr.velocity;
            Vector3 avoidForce = CalcuVelocity.CalucSeekVec(velocity, avoidVec, velocity.magnitude);
            m_velocityMgr.AddForce(avoidForce);
        }
    }

    private void Move(Vector3 moveVec)
    {
        if (!CalcuVelocity.IsTurningVector(m_velocityMgr.velocity, moveVec, m_param.turningDegree)) {  //追従できる角度でないなら
            Debug.Log("曲がれない");
            moveVec = transform.forward; //直進する。
        }

        float moveSpeed = m_param.moveSpeed * m_statusManager.GetBuffParametor().angerParam.speed;

        m_velocityMgr.velocity = moveVec.normalized * moveSpeed;
    }

    /// <summary>
    /// ターゲットの方向を向く処理
    /// </summary>
    private void SetForwardTarget()
    {
        var position = m_targetMgr.GetNowTargetPosition();
        if (position != null)
        {
            var toVec = (Vector3)position - transform.position;
            toVec.y = 0.0f;  //(yのベクトルを殺す。)
            transform.forward = toVec.normalized;
        }
    }

    private void Rotation(Vector3 direct)
    {
        m_rotationController.SetDirect(direct);
    }

    /// <summary>
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    override public bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;

        var position = m_targetMgr.GetNowTargetPosition();
        if (position != null)
        {
            return m_eyeRange.IsInEyeRange((Vector3)position, range);
            //return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else
        {
            return false;
        }
    }

    public override void AttackStart()
    {
        m_audioManager?.PlayOneShot();

        m_isTargetChase = true;
        SetForwardTarget();
        m_rotationController.enabled = true;
        m_waitTimer.AbsoluteEndTimer(GetType(), false);

        enabled = true;
    }

    public override void EndAnimationEvent()
    {
        Debug.Log("終了タイマー" + m_param.endWaitTime);
        m_waitTimer.AddWaitTimer(GetType(),m_param.endWaitTime,() => { m_attackManager.EndAnimationEvent(); enabled = false; });

        m_velocityMgr.SetIsDeseleration(false);
        m_velocityMgr.ResetForce();
        m_velocityMgr.ResetVelocity();
    }

    /// <summary>
    /// 追従処理終了
    /// </summary>
    public void ChaseEnd()
    {
        m_isTargetChase = false;

        m_velocityMgr.StartDeseleration();
    }

    //アクセッサ・プロパティ----------------------------------------------------------

    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }

}
