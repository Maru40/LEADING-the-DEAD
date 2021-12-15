using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using FoundType = FoundObject.FoundType;

public class EnState_ChaseTarget : EnemyStateNodeBase<EnemyBase>
{
    AttackNodeManagerBase m_attackComp;
    TargetManager m_targetManager;
    EyeSearchRange m_eye;

    FoundObject m_eyeTarget = null;

    public EnState_ChaseTarget(EnemyBase owner)
        : base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_eye = owner.GetComponent<EyeSearchRange>();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ChaseTarget>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        var owner = GetOwner();

        m_attackComp = owner.GetComponent<AttackNodeManagerBase>();

        var chaseTarget = owner.GetComponent<ChaseTarget>();

        //集団行動設定
        var throngManager = owner.GetComponent<ThrongManager>();
        if (chaseTarget && throngManager)
        {
            float range = chaseTarget.GetInThrongRange();
            throngManager.SetInThrongRange(range);
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
    void StateCheck()
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

    void PlayerCheck()
    {
        if (m_attackComp.IsAttackStartRange())
        {
            m_attackComp.AttackStart();
        }
    }

    void SmellCheck()
    {

    }

    /// <summary>
    /// ターゲットのチェック
    /// </summary>
    void TargetCheck()
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
