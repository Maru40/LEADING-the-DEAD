using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = ZombieNormalState;
using TransitionMember = ZombieNormalTransitionMember;
using StateMachine = EnemyMainStateMachine<EnemyBase, ZombieNormalState, ZombieNormalTransitionMember>;


enum ZombieNormalState
{
    RandomPlowling,
    Chase,
    Attack,
}

public class ZombieNormalTransitionMember
{
    public MyTrigger rondomPlowlingTrigger = new MyTrigger();
    public MyTrigger chaseTrigger = new MyTrigger();
    public MyTrigger attackTrigger = new MyTrigger();
}

public class Stator_ZombieNormal : StatorBase
{
    StateMachine m_stateMachine;

    void Start()
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
        m_stateMachine.AddNode(StateType.Attack,         new EnState_Attack(zombie));
    }

    void CreateEdge()
    {
        //ƒ‰ƒ“ƒ_ƒ€œpœj
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Chase, ToChaseTrigger);

        //’Ç]ˆ—
        m_stateMachine.AddEdge(StateType.Chase, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, ToAttackTrigger);

        //UŒ‚ˆ—
        m_stateMachine.AddEdge(StateType.Attack, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.RandomPlowling, ToRandomPlowling);
    }


    //‘JˆÚğŒŒn---------------------------------------------------------------

    bool ToChaseTrigger(TransitionMember member) {
        return member.chaseTrigger.Get();
    }

    bool ToRandomPlowling(TransitionMember member) {
        return member.rondomPlowlingTrigger.Get();
    }

    bool ToAttackTrigger(TransitionMember member) {
        return member.attackTrigger.Get();
    }

    //ƒAƒNƒZƒbƒT----------------------------------------------------------------
    
    /// <summary>
    /// ‘JˆÚ‚É—˜—p‚·‚éƒƒ“ƒo[‚Ìæ“¾
    /// </summary>
    /// <returns>‘JˆÚğŒƒƒ“ƒo[</returns>
    public TransitionMember GetTransitionMember()
    {
        return m_stateMachine.GetTransitionStructMember();
    }

}
