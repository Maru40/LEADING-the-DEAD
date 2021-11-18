using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using FoundType = FoundObject.FoundType;

public class EnState_ChaseTarget : EnemyStateNodeBase<EnemyBase>
{
    AttackNodeManagerBase m_attackComp;
    TargetManager m_targetManager;

    public EnState_ChaseTarget(EnemyBase owner)
        : base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
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
        var throngMgr = owner.GetComponent<ThrongManager>();
        if (chaseTarget && throngMgr)
        {
            float range = chaseTarget.GetInThrongRange();
            throngMgr.SetInThrongRange(range);
        }
    }

    public override void OnUpdate()
    {
        StateCheck();

        //if (m_attackComp.IsAttackStartRange())
        //{
        //    m_attackComp.AttackStart();
        //}
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
        //現状はSmallManagerで管理中、後で習性候補
        return;

        //if (!m_targetManager.HasTarget()) {
        //    return;
        //}

        //肉なら食べるモーション


        //匂いならターゲットをnullにする。
        //var positionCheck = m_targetManager.GetToNowTargetVector();
        //if(positionCheck == null) {
        //    return;
        //}
        //var toTargetVec = (Vector3)positionCheck;

        //const float near = 0.5f;
        //if(toTargetVec.magnitude < near)
        //{
        //    m_targetManager.AddExcludeNowTarget();
        //}
    }
}
