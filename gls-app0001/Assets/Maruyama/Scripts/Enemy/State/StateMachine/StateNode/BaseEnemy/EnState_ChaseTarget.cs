using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using FoundType = FoundObject.FoundType;

public class EnState_ChaseTarget : EnemyStateNodeBase<EnemyBase>
{
    private AttackNodeManagerBase m_attackComp;
    private TargetManager m_targetManager;
    private EyeSearchRange m_eye;
    private ChaseTarget m_chaseTarget;
    private ThrongManager m_throngManager;

    private FoundObject m_eyeTarget = null;

    public EnState_ChaseTarget(EnemyBase owner)
        : base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_eye = owner.GetComponent<EyeSearchRange>();
        m_attackComp = owner.GetComponent<AttackNodeManagerBase>();
        m_chaseTarget = owner.GetComponent<ChaseTarget>();
        m_throngManager = owner.GetComponent<ThrongManager>();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ChaseTarget>(), true, false);
        AddChangeComp(owner.GetComponent<EnemyVelocityManager>(), true, true);
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), true, true);
    }

    public override void OnStart()
    {
        base.OnStart();

        var owner = GetOwner();

        //var chaseTarget = owner.GetComponent<ChaseTarget>();

        //集団行動設定
        //var throngManager = owner.GetComponent<ThrongManager>();
        if (m_chaseTarget && m_throngManager)
        {
            float range = m_chaseTarget.GetInThrongRange();
            m_throngManager.SetInThrongRange(range);
        }
    }

    public override void OnUpdate()
    {
        Debug.Log("〇ChaseState");

        StateCheck();

        TargetCheck();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    /// <summary>
    /// ステートの遷移管理
    /// </summary>
    private void StateCheck()
    {
        if(!m_targetManager.HasTarget()) {  //ターゲットがnullだったら
            return; //処理をしない
        }

        var type = m_targetManager.GetNowTargetType();

        System.Action action = type switch
        {
            FoundType.Player => PlayerCheck,
            FoundType.Smell => SmellCheck,
            _ => null,
        };

        action?.Invoke();
    }

    private void PlayerCheck()
    {
        if(m_attackComp == null) {
            return;
        }

        if (m_attackComp.IsAttackStartRange())
        {
            m_attackComp.AttackStart();
        }
    }

    private void SmellCheck()
    {

    }

    /// <summary>
    /// ターゲットのチェック
    /// </summary>
    private void TargetCheck()
    {
        //ターゲットが視界に入ったら切り替える。
        if(m_eyeTarget == null) {
            SearchEyeTarget();
        }

        if(m_eye.IsInEyeRange(m_eyeTarget.gameObject)) { //視界の中にいたら
            m_targetManager.SetNowTarget(GetType(), m_eyeTarget);  //ターゲットの変更
        }
    }

    private void SearchEyeTarget()
    {
        var owner = GetOwner();

        var target = GameObject.Find("Player");
        var foundObject = target.GetComponent<FoundObject>();
        //m_targetManager?.SetNowTarget(GetType(), null);

        m_eyeTarget = foundObject;
    }
}
