using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

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

[System.Serializable]
public struct ZombieNormalTransitionMember
{
    public MyTrigger rondomPlowlingTrigger;
    public MyTrigger findTrigger;
    public MyTrigger eatTrigger;
    public MyTrigger chaseTrigger;
    public MyTrigger attackTrigger;
    public MyTrigger wallRising;
    public MyTrigger stunTrigger;
    public MyTrigger angerTirgger;
    //public MyTrigger dyingTrigger = new MyTrigger();
    public MyTrigger deathTrigger;
    [Header("通常攻撃に遷移する距離")]
    public float normalAttackRange;
    [Header("ダッシュ攻撃に遷移する条件パラメータ")]
    public DashAttackTransitionManager.Parametor dashAttackTransitionParam;
}

public class Stator_ZombieNormal : StatorBase
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("遷移用のパラメータ")]
        public TransitionMember transitionMember;
        [Header("攻撃パラメータ")]
        public StateNode_ZombieNormal_Attack.Parametor attackParam;
    }

    private StateMachine m_stateMachine;
    private DashAttackTransitionManager m_dashAttackTransitionManager; //DashAttackの遷移を管理する

    private EyeSearchRange m_eye;
    private BlackBoard_ZombieNormal m_blackBoard;
    private TargetManager m_targetManager;

    //パラメータ----------------------------

    [SerializeField]
    private StateNode_ZombieNormal_Find.Parametor m_findParametor = new StateNode_ZombieNormal_Find.Parametor(1.0f, 2.0f);
    [SerializeField]
    private StateNode_ZombieNormal_Dying.Parametor m_dyingParametor =
        new StateNode_ZombieNormal_Dying.Parametor(0.5f, 1.5f);

    [SerializeField]
    private Parametor m_param = new Parametor();
    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }

    private void Awake()
    {
        m_eye = GetComponent<EyeSearchRange>();
        m_blackBoard = GetComponent<BlackBoard_ZombieNormal>();
        m_targetManager = GetComponent<TargetManager>();

        //CreateStateMachine();
    }

    private void Start()
    {
        m_stateMachine = new StateMachine(m_param.transitionMember);

        CreateStateMachine();
    }

    private void Update()
    {
        m_stateMachine.OnUpdate();
    }

    private void CreateStateMachine()
    {
        CreateReserve();
        CreateNode();
        CreateEdge();
    }

    /// <summary>
    /// 生成準備(必要なマネージャクラスの生成など)
    /// </summary>
    private void CreateReserve()
    {
        var enemy = GetComponent<EnemyBase>();

        m_dashAttackTransitionManager = new DashAttackTransitionManager(enemy,
            () => m_blackBoard.Struct.attackParam.startType = StateNode_ZombieNormal_Attack.StateType.Dash, //ダッシュ状態に遷移
            m_param.transitionMember.dashAttackTransitionParam);
    }

    private void CreateNode()
    {
        var zombie = GetComponent<ZombieNormal>();

        m_stateMachine.AddNode(StateType.RandomPlowling, new EnState_RandomPlowling(zombie));
        m_stateMachine.AddNode(StateType.Find, new StateNode_ZombieNormal_Find(zombie, m_findParametor));
        m_stateMachine.AddNode(StateType.Chase, new EnState_ChaseTarget(zombie));
        m_stateMachine.AddNode(StateType.Eat, new StateNode_ZombieNormal_Eat(zombie));
        m_stateMachine.AddNode(StateType.Attack, new StateNode_ZombieNormal_Attack(zombie, m_param.attackParam));
        m_stateMachine.AddNode(StateType.WallRising, new StateNode_ZombieNormal_WallRising(zombie));
        m_stateMachine.AddNode(StateType.Stun, new EnState_Stun(zombie));
        m_stateMachine.AddNode(StateType.Anger, new StateNode_ZombieNormal_Anger(zombie));
        m_stateMachine.AddNode(StateType.KnockBack, new StateNode_KnockBack_EnemyBase(zombie));
        m_stateMachine.AddNode(StateType.Dying, new StateNode_ZombieNormal_Dying(zombie, m_dyingParametor));
        m_stateMachine.AddNode(StateType.Death, new StateNode_ZombiNormal_Death(zombie));
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
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, IsNormalAttack);
        m_stateMachine.AddEdge(StateType.Chase, StateType.Attack, IsDashAttack);
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

    private bool ToFindTrigger(ref TransitionMember member)
    {
        return member.findTrigger.Get();
    }

    private bool ToChaseTrigger(ref TransitionMember member)
    {
        return member.chaseTrigger.Get();
    }

    private bool ToEatTrigger(ref TransitionMember member)
    {
        return member.eatTrigger.Get();
    }

    private bool ToRandomPlowling(ref TransitionMember member)
    {
        return member.rondomPlowlingTrigger.Get();
    }

    private bool ToAttackTrigger(ref TransitionMember member)
    {
        return member.attackTrigger.Get();
    }

    private bool ToWallRisingTrigger(ref TransitionMember member)
    {
        return member.wallRising.Get();
    }

    private bool ToStunTrigger(ref TransitionMember member)
    {
        return member.stunTrigger.Get();
    }

    private bool ToAngerTrigger(ref TransitionMember member)
    {
        return member.angerTirgger.Get();
    }

    private bool ToDeathTrigger(ref TransitionMember member)
    {
        return member.deathTrigger.Get();
    }

    private bool IsNormalAttack(ref TransitionMember member)
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        //ターゲットがプレイヤーで無かったら
        if(m_targetManager.GetNowTargetType() != FoundObject.FoundType.Player) {
            return false;
        }

        var range = member.normalAttackRange;
        var targetPosition = (Vector3)m_targetManager.GetNowTargetPosition();

        //範囲内かつ、障害物がない時
        if (Calculation.IsRange(transform.position, targetPosition, range) && 
            !Obstacle.IsLineCastObstacle(transform.position, targetPosition))
        {
            m_blackBoard.Struct.attackParam.startType = StateNode_ZombieNormal_Attack.StateType.Normal;
            return true;
        }

        return false;
    }

    private bool IsDashAttack(ref TransitionMember member)
    {
        return m_dashAttackTransitionManager.IsTransition(member.dashAttackTransitionParam);
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

    public void SetIsTransitionLock(bool isLock)
    {
        m_stateMachine.SetIsTransitionLock(isLock);
    }

    public override void StateReset()
    {
        m_stateMachine.Reset();
    }
}
