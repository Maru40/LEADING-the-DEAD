using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

public class NormalAttack : AttackNodeBase
{
    [Serializable]
    public struct Parametor
    {
        public float moveSpeed;
        public float endWaitTime;  //終了時にストップする時間

        public Parametor(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            this.endWaitTime = 0.1f;
        }
    }

    //Stator_ZombieTank m_stator;
    AttackNodeManagerBase m_attackManager;
    TargetManager m_targetMgr;
    EnemyVelocityMgr m_velocityMgr;
    EyeSearchRange m_eyeRange;
    StatusManagerBase m_statusManager;
    ThrongManager m_throngManager;
    EnemyRotationCtrl m_rotationController;
    WaitTimer m_waitTimer;

    [SerializeField]
    Parametor m_param = new Parametor(3.0f);

    bool m_isTargetChase = true;  //攻撃の途中まではターゲットを追うようにするため。

    void Awake()
    {
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_targetMgr = GetComponent<TargetManager>();
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_statusManager = GetComponent<StatusManagerBase>();
        m_throngManager = GetComponent<ThrongManager>();
        m_rotationController = GetComponent<EnemyRotationCtrl>();
        m_waitTimer = GetComponent<WaitTimer>();
    }

    void Update()
    {
        if (m_isTargetChase)
        {
            TargetChase();
        }
    }

    /// <summary>
    /// 攻撃の途中までは敵を追従するため。
    /// </summary>
    void TargetChase()
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
    void ThrongAdjust()
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
        float moveSpeed = m_param.moveSpeed * m_statusManager.GetBuffParametor().angerParam.speed;
        m_velocityMgr.velocity = moveVec.normalized * moveSpeed;

        //if(UtilityMath.IsFront(transform.forward, moveVec, 30.0f))
        //{
        //    m_velocityMgr.velocity = moveVec.normalized * moveSpeed;
        //    Rotation(moveVec);
        //}
    }

    /// <summary>
    /// ターゲットの方向を向く処理
    /// </summary>
    void SetForwardTarget()
    {
        var position = m_targetMgr.GetNowTargetPosition();
        if (position != null)
        {
            var toVec = (Vector3)position - transform.position;
            toVec.y = 0.0f;  //(yのベクトルを殺す。)
            transform.forward = toVec.normalized;
        }
    }

    void Rotation(Vector3 direct)
    {
        m_rotationController.SetDirect(direct);
        //transform.forward = direct;
    }

    /// <summary>
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    override public bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        //FoundObject target = m_targetMgr.GetNowTarget();
        var position = m_targetMgr.GetNowTargetPosition();
        if (position != null)
        {
            return m_eyeRange.IsInEyeRange((Vector3)position);
            //return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else
        {
            return false;
        }
    }

    public override void AttackStart()
    {
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
        //m_attackManager.EndAnimationEvent();

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
