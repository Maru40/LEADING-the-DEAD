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
}

public class ZombieNormalTransitionMember
{
    public MyTrigger rondomPlowlingTrigger = new MyTrigger();
    public MyTrigger chaseTrigger = new MyTrigger();
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
    }

    void CreateEdge()
    {
        //�����_���p�j
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Chase, ToChaseTrigger);

        //�Ǐ]����
        m_stateMachine.AddEdge(StateType.Chase, StateType.RandomPlowling, ToRandomPlowling);
    }


    //�J�ڏ����n---------------------------------------------------------------

    bool ToChaseTrigger(TransitionMember member) {
        return member.chaseTrigger.Get();
    }

    bool ToRandomPlowling(TransitionMember member) {
        return member.rondomPlowlingTrigger.Get();
    }


    //�A�N�Z�b�T----------------------------------------------------------------
    
    /// <summary>
    /// �J�ڂɗ��p���郁���o�[�̎擾
    /// </summary>
    /// <returns>�J�ڏ��������o�[</returns>
    public TransitionMember GetTransitionMember()
    {
        return m_stateMachine.GetTransitionStructMember();
    }

}
