using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = ZombieTankState;
using TransitionMember = ZombieTankTransitionMember;
using StateMachine = EnemyMainStateMachine<EnemyBase, ZombieTankState, ZombieTankTransitionMember>;

public enum ZombieTankState
{
    RandomPlowling,
    Chase,
    Attack,
}

public class ZombieTankTransitionMember
{
    public MyTrigger rondomPlowlingTrigger = new MyTrigger();
    public MyTrigger chaseTrigger = new MyTrigger();
    public MyTrigger attackTrigger = new MyTrigger();
}

public class Stator_ZombieTank : StatorBase
{
    StateMachine m_stateMachine;

    void Start()
    {
        m_stateMachine = new StateMachine();

        CreateNode();
        CreateEdge();
    }

    void Update()
    {
        m_stateMachine.OnUpdate();
    }

    void CreateNode()
    {
        var zombie = GetComponent<ZombieTank>();

        m_stateMachine.AddNode(StateType.RandomPlowling, new EnState_RandomPlowling(zombie));
        m_stateMachine.AddNode(StateType.Chase, new EnState_ChaseTarget(zombie));
        m_stateMachine.AddNode(StateType.Attack, new StateNode_ZombieTank_Attack(zombie));
    }

    void CreateEdge()
    {
        //ランダム徘徊
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Chase, ToChaseTrigger);

        //追従処理
        m_stateMachine.AddEdge(StateType.Chase, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, ToAttackTrigger);

        //攻撃処理
        m_stateMachine.AddEdge(StateType.Attack, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.RandomPlowling, ToRandomPlowling);
    }

    //遷移条件系---------------------------------------------------------------

    bool ToChaseTrigger(TransitionMember member)
    {
        return member.chaseTrigger.Get();
    }

    bool ToRandomPlowling(TransitionMember member)
    {
        return member.rondomPlowlingTrigger.Get();
    }

    bool ToAttackTrigger(TransitionMember member)
    {
        return member.attackTrigger.Get();
    }

    //アクセッサ----------------------------------------------------------------

    /// <summary>
    /// 遷移に利用するメンバーの取得
    /// </summary>
    /// <returns>遷移条件メンバー</returns>
    public TransitionMember GetTransitionMember()
    {
        return m_stateMachine.GetTransitionStructMember();
    }

    public override void Reset()
    {
        m_stateMachine.Reset();
    }
}
