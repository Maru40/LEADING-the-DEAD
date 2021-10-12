using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = ZombieNormalState;
using TransitionMember = ZombieNormalTransitionMember;
using StateMachine = EnemyMainStateMachine<EnemyBase, ZombieNormalState, ZombieNormalTransitionMember>;

public enum ZombieNormalState
{
    RandomPlowling,
    Chase,
    Attack,
    Stun,
    Anger,
}

public class ZombieNormalTransitionMember
{
    public MyTrigger rondomPlowlingTrigger = new MyTrigger();
    public MyTrigger chaseTrigger = new MyTrigger();
    public MyTrigger attackTrigger = new MyTrigger();
    public MyTrigger stunTrigger = new MyTrigger();
    public MyTrigger angerTirgger = new MyTrigger();
}

public class Stator_ZombieNormal : StatorBase
{
    StateMachine m_stateMachine;

    void Awake()
    {
        m_stateMachine = new StateMachine();

        CreateStateMachine();
    }

    void Update()
    {
        m_stateMachine.OnUpdate();
    }

    void CreateStateMachine()
    {
        CreateNode();
        CreateEdge();
    }

    void CreateNode()
    {
        var zombie = GetComponent<ZombieNormal>();

        m_stateMachine.AddNode(StateType.RandomPlowling, new EnState_RandomPlowling(zombie));
        m_stateMachine.AddNode(StateType.Chase,          new EnState_ChaseTarget(zombie));
        m_stateMachine.AddNode(StateType.Attack,         new StateNode_ZombieNormal_Attack(zombie));
        m_stateMachine.AddNode(StateType.Stun,           new EnState_Stun(zombie));
        m_stateMachine.AddNode(StateType.Anger,          new StateNode_ZombieNormal_Anger(zombie));
    }

    void CreateEdge()
    {
        //ランダム徘徊
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Stun, ToStunTrigger);
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Chase, ToChaseTrigger);

        //追従処理
        m_stateMachine.AddEdge(StateType.Chase, StateType.Stun, ToStunTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, ToAttackTrigger);

        //攻撃処理
        m_stateMachine.AddEdge(StateType.Attack, StateType.Stun, ToStunTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.RandomPlowling, ToRandomPlowling);

        //スタン時
        m_stateMachine.AddEdge(StateType.Stun, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.Stun, StateType.RandomPlowling, ToRandomPlowling);

        //怒り状態開始
        m_stateMachine.AddEdge(StateType.Anger, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Anger, StateType.Chase, ToChaseTrigger);
    }


    //遷移条件系---------------------------------------------------------------

    bool ToChaseTrigger(TransitionMember member) {
        return member.chaseTrigger.Get();
    }

    bool ToRandomPlowling(TransitionMember member) {
        return member.rondomPlowlingTrigger.Get();
    }

    bool ToAttackTrigger(TransitionMember member) {
        return member.attackTrigger.Get();
    }

    bool ToStunTrigger(TransitionMember member)
    {
        return member.stunTrigger.Get();
    }

    bool ToAngerTrigger(TransitionMember member)
    {
        return member.angerTirgger.Get();
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
