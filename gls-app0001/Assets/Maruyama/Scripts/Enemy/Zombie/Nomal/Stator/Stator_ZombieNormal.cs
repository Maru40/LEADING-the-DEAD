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
    Eat,   //食べる。
    Attack,
    Find,  //見つけた
    WallRising,  //壁のぼり
    Stun,
    Anger,
    KnockBack,  //ノックバック
    Dying,  //瀕死状態
    Death,  //死亡状態
}

public class ZombieNormalTransitionMember
{
    public MyTrigger rondomPlowlingTrigger = new MyTrigger();
    public MyTrigger findTrigger = new MyTrigger();
    public MyTrigger eatTrigger = new MyTrigger();
    public MyTrigger chaseTrigger = new MyTrigger();
    public MyTrigger attackTrigger = new MyTrigger();
    public MyTrigger wallRising = new MyTrigger();
    public MyTrigger stunTrigger = new MyTrigger();
    public MyTrigger angerTirgger = new MyTrigger();
    //public MyTrigger dyingTrigger = new MyTrigger();
    public MyTrigger deathTrigger = new MyTrigger();
}

public class Stator_ZombieNormal : StatorBase
{
    private StateMachine m_stateMachine;

    //パラメータ

    [SerializeField]
    private StateNode_ZombieNormal_Find.Parametor m_findParametor = new StateNode_ZombieNormal_Find.Parametor(1.0f, 2.0f);

    private void Awake()
    {
        m_stateMachine = new StateMachine();

        CreateStateMachine();
    }

    private void Update()
    {
        m_stateMachine.OnUpdate();
    }

    private void CreateStateMachine()
    {
        CreateNode();
        CreateEdge();
    }

    private void CreateNode()
    {
        var zombie = GetComponent<ZombieNormal>();

        m_stateMachine.AddNode(StateType.RandomPlowling, new EnState_RandomPlowling(zombie));
        m_stateMachine.AddNode(StateType.Find,           new StateNode_ZombieNormal_Find(zombie, m_findParametor));
        m_stateMachine.AddNode(StateType.Chase,          new EnState_ChaseTarget(zombie));
        m_stateMachine.AddNode(StateType.Eat,            new StateNode_ZombieNormal_Eat(zombie));
        m_stateMachine.AddNode(StateType.Attack,         new StateNode_ZombieNormal_Attack(zombie));
        m_stateMachine.AddNode(StateType.WallRising,     new StateNode_ZombieNormal_WallRising(zombie));
        m_stateMachine.AddNode(StateType.Stun,           new EnState_Stun(zombie));
        m_stateMachine.AddNode(StateType.Anger,          new StateNode_ZombieNormal_Anger(zombie));
        m_stateMachine.AddNode(StateType.KnockBack,      new StateNode_KnockBack_EnemyBase(zombie));
        m_stateMachine.AddNode(StateType.Dying,          new StateNode_ZombieNormal_Dying(zombie));
        m_stateMachine.AddNode(StateType.Death,          new StateNode_ZombiNormal_Death(zombie));
    }

    private void CreateEdge()
    {
        //ランダム徘徊
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Find, ToFindTrigger);
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Stun, ToStunTrigger);
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.RandomPlowling, StateType.Chase, ToChaseTrigger);

        //見つけた
        m_stateMachine.AddEdge(StateType.Find, StateType.Chase, ToChaseTrigger);

        //追従処理
        m_stateMachine.AddEdge(StateType.Chase, StateType.Stun, ToStunTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, ToAttackTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Eat, ToEatTrigger);
        m_stateMachine.AddEdge(StateType.Chase, StateType.WallRising, ToWallRisingTrigger);

        //食べているときの処理
        m_stateMachine.AddEdge(StateType.Eat, StateType.RandomPlowling, ToRandomPlowling);
        m_stateMachine.AddEdge(StateType.Eat, StateType.Chase, ToChaseTrigger);

        //攻撃処理
        m_stateMachine.AddEdge(StateType.Attack, StateType.Stun, ToStunTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.Attack, StateType.RandomPlowling, ToRandomPlowling);

        //壁のぼり時
        m_stateMachine.AddEdge(StateType.WallRising, StateType.Chase, ToChaseTrigger);
        m_stateMachine.AddEdge(StateType.WallRising, StateType.RandomPlowling, ToRandomPlowling);

        //スタン時
        m_stateMachine.AddEdge(StateType.Stun, StateType.Anger, ToAngerTrigger);
        m_stateMachine.AddEdge(StateType.Stun, StateType.RandomPlowling, ToRandomPlowling);

        //ノックバック時
        m_stateMachine.AddEdge(StateType.KnockBack, StateType.Stun, ToStunTrigger);

        //怒り状態開始
        m_stateMachine.AddEdge(StateType.Anger, StateType.RandomPlowling, ToRandomPlowling);

        //瀕死状態
        m_stateMachine.AddEdge(StateType.Dying, StateType.Death, ToDeathTrigger);

        //死亡状態
    }


    //遷移条件系---------------------------------------------------------------

    private bool ToFindTrigger(TransitionMember member)
    {
        return member.findTrigger.Get();
    }

    private bool ToChaseTrigger(TransitionMember member) {
        return member.chaseTrigger.Get();
    }

    private bool ToEatTrigger(TransitionMember member)
    {
        return member.eatTrigger.Get();
    }

    private bool ToRandomPlowling(TransitionMember member) {
        return member.rondomPlowlingTrigger.Get();
    }

    private bool ToAttackTrigger(TransitionMember member) {
        return member.attackTrigger.Get();
    }

    private bool ToWallRisingTrigger(TransitionMember member) {
        return member.wallRising.Get();
    }

    private bool ToStunTrigger(TransitionMember member)
    {
        return member.stunTrigger.Get();
    }

    private bool ToAngerTrigger(TransitionMember member)
    {
        return member.angerTirgger.Get();
    }

    private bool ToDeathTrigger(TransitionMember member)
    {
        return member.deathTrigger.Get();
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
    public TransitionMember GetTransitionMember()
    {
        return m_stateMachine.GetTransitionStructMember();
    }

    public StateType GetNowStateType()
    {
        return m_stateMachine.GetNowType();
    }

    public void SetIsTransitionLock(bool isLock)
    {
        m_stateMachine.SetIsTransitionLock(isLock);
    }

    public override void Reset()
    {
        m_stateMachine.Reset();
    }
}
