using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = ZombieTankState;
using TransitionMember = ZombieTankTransitionMember;
using StateMachine = EnemyMainStateMachine<EnemyBase, ZombieTankState, ZombieTankTransitionMember>;

public enum ZombieTankState
{
    None,
    RandomPlowling,
    Chase,
    WaitSee,  //様子見ステート
    Attack,
}

public struct ZombieTankTransitionMember
{
    public MyTrigger rondomPlowlingTrigger;
    public MyTrigger chaseTrigger;
    public MyTrigger waitSeeTrigger;
    public MyTrigger attackTrigger;
}

public class Stator_ZombieTank : StatorBase
{
    private StateMachine m_stateMachine;

    private void Start()
    {
        m_stateMachine = new StateMachine();

        CreateNode();
        CreateEdge();
    }

    private void Update()
    {
        m_stateMachine.OnUpdate();
    }

    private void CreateNode()
    {
        var zombie = GetComponent<ZombieTank>();

        m_stateMachine.AddNode(StateType.None , new StateNode_ZombieTank_None(zombie));
        m_stateMachine.AddNode(StateType.RandomPlowling, new EnState_RandomPlowling(zombie));
        m_stateMachine.AddNode(StateType.Chase, new EnState_ChaseTarget(zombie));
        m_stateMachine.AddNode(StateType.WaitSee, new StateNode_ZombieTank_WaitSee(zombie));
        m_stateMachine.AddNode(StateType.Attack, new StateNode_ZombieTank_Attack(zombie));
    }

    private void CreateEdge()
    {
        //None
        m_stateMachine.AddEdge(StateType.None, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.None, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.None, StateType.WaitSee, ToWaitSeeTrigger);
        m_stateMachine.AddEdge(StateType.None, StateType.Attack, ToAttackTrigger);

        //ランダム徘徊
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.WaitSee, ToWaitSeeTrigger);

        //追従処理
        m_stateMachine.AddEdge(StateType.Chase, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Chase, StateType.WaitSee, ToWaitSeeTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, ToAttackTrigger);

        //様子見
        m_stateMachine.AddEdge(StateType.WaitSee, StateType.Attack, ToAttackTrigger);
        m_stateMachine.AddEdge(StateType.WaitSee, StateType.WaitSee, ToWaitSeeTrigger);

        //攻撃処理
        m_stateMachine.AddEdge(StateType.Attack, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.RandomPlowling, ToRandomPlowling);
    }

    //遷移条件系---------------------------------------------------------------

    private bool ToChaseTrigger(ref TransitionMember member)
    {
        return member.chaseTrigger.Get();
    }

    private bool ToRandomPlowling(ref TransitionMember member)
    {
        return member.rondomPlowlingTrigger.Get();
    }

    private bool ToWaitSeeTrigger(ref TransitionMember member)
    {
        return member.waitSeeTrigger.Get();
    }

    private bool ToAttackTrigger(ref TransitionMember member)
    {
        return member.attackTrigger.Get();
    }

    public override void ChangeState<EnumType>(EnumType type, int priority = 0)
    {
        if (type is StateType)
        {
            StateType? stateType = type as StateType?;
            m_stateMachine.ChangeState((StateType)stateType, priority);
        }
    }

    //アクセッサ----------------------------------------------------------------

    /// <summary>
    /// 遷移に利用するメンバーの取得
    /// </summary>
    /// <returns>遷移条件メンバー</returns>
    public ref TransitionMember GetTransitionMember()
    {
        return ref m_stateMachine.GetTransitionStructMember();
    }

    public StateType GetNowStateType()
    {
        return m_stateMachine.GetNowType();
    }

    public override void StateReset()
    {
        m_stateMachine.Reset();
    }
}
