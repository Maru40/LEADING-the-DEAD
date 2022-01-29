using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

using FoundType = FoundObject.FoundType;

using StateMachine = EnemyMainStateMachine<EnemyBase, Stator_ZombieChild.StateType, Stator_ZombieChild.TransitionMember>;

public class Stator_ZombieChild : StatorBase
{
    public enum StateType
    {
        Plowling, //徘徊
        Find,     //見つける
        Chase,    //追いかける
        Escape,   //逃げる
        Cry,      //泣く
        Dying,    //瀕死状態
        Death,    //死亡状態
        Max
    }

    public struct TransitionMember
    {
        public MyTrigger plowlingTrigger;
        public MyTrigger escapeTrigger;
        public MyTrigger cryTrigger;
        public MyTrigger deathTrigger;
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("発見時に泣く対象")]
        public List<FoundType> cryTargets;
        [Header("発見ステート")]
        public StateNode_ZombieChild_Find.Parametor findParam;
        [Header("泣くパラメータ")]
        public StateNode_ZombieChild_Cry.Parametor cryParam;
        [Header("逃げるパラメータ")]
        public StateNode_ZombieChild_Escape.Parametor escapeParam;
        [Header("死亡(瀕死)時パラメータ")]
        public StateNode_ZombieChild_Dyning.Parametor dyningParam;
        [Header("完全死亡時パラメータ")]
        public StateNode_ZombieChild_Death.Parametor deathParam;
    }

    [SerializeField]
    private Parametor m_param = new Parametor();
    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }

    private StateMachine m_stateMachine = new StateMachine();

    //コンポーネント系-------------------------------------------------------

    private EyeSearchRange m_eye;
    private TargetManager m_targetManager;

    //-----------------------------------------------------------------------

    public void Awake()
    {
        m_eye = GetComponent<EyeSearchRange>();
        m_targetManager = GetComponent<TargetManager>();
    }

    public void Start()
    {
        CreateStateMachine();
    }

    public void Update()
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
        var enemy = GetComponent<EnemyBase>();

        m_stateMachine.AddNode(StateType.Plowling, new StateNode_ZombieChild_Plowling(enemy));
        m_stateMachine.AddNode(StateType.Escape, new StateNode_ZombieChild_Escape(enemy, m_param.escapeParam));
        m_stateMachine.AddNode(StateType.Find, new StateNode_ZombieChild_Find(enemy, m_param.findParam));
        m_stateMachine.AddNode(StateType.Cry, new StateNode_ZombieChild_Cry(enemy, m_param.cryParam));
        m_stateMachine.AddNode(StateType.Dying, new StateNode_ZombieChild_Dyning(enemy, m_param.dyningParam));
        m_stateMachine.AddNode(StateType.Death, new StateNode_ZombieChild_Death(enemy, m_param.deathParam));
    }

    private void CreateEdge()
    {
        //徘徊
        m_stateMachine.AddEdge(StateType.Plowling, StateType.Cry, IsCryTrigger);
        m_stateMachine.AddEdge(StateType.Plowling, StateType.Find, IsFindCryTarget);
        //m_stateMachine.AddEdge(StateType.Plowling, StateType.Cry, IsFindCryTarget);

        //逃げる
        m_stateMachine.AddEdge(StateType.Escape, StateType.Plowling, IsPlowlingTrigger);

        //見つける
        m_stateMachine.AddEdge(StateType.Find, StateType.Cry, IsCryTrigger);
        m_stateMachine.AddEdge(StateType.Find, StateType.Plowling, IsFindTargetLost);

        //泣く
        m_stateMachine.AddEdge(StateType.Cry, StateType.Escape, IsEscapeTrigger);

        //瀕死
        m_stateMachine.AddEdge(StateType.Dying, StateType.Death, IsDeathTrigger);

        //死亡
    }

    //遷移条件系---------------------------------------------------------------

    private bool IsPlowlingTrigger(ref TransitionMember member)
    {
        return member.plowlingTrigger.Get();
    }

    private bool IsEscapeTrigger(ref TransitionMember member)
    {
        return member.escapeTrigger.Get();
    }

    private bool IsCryTrigger(ref TransitionMember member)
    {
        return member.cryTrigger.Get();
    }

    private bool IsDeathTrigger(ref TransitionMember member)
    {
        return member.deathTrigger.Get();
    }

    /// <summary>
    /// 泣く対象を発見したとき
    /// </summary>
    /// <returns></returns>
    private bool IsFindCryTarget(ref TransitionMember member)
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        if (IsTargetType(m_param.cryTargets.ToArray())) { //泣く対象だったら
            return true;
        }

        return false;
    }

    /// <summary>
    /// 発見時にターゲットをロストする。
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    private bool IsFindTargetLost(ref TransitionMember member)
    {
        if (!m_targetManager.HasTarget()) {
            return true;
        }

        const float eyeDegree = 90.0f;
        var param = m_eye.GetParam();
        param.degree = eyeDegree;
        //ターゲットが視界内にいないから
        if (!m_eye.IsInEyeRange(m_targetManager.GetNowTarget().gameObject, param))
        {
            m_targetManager.SetNowTarget(GetType(), null);
            return true;  //遷移する。
        }

        return false;
    }

    /// <summary>
    /// ターゲットが対象タイプかどうか
    /// </summary>
    /// <param name="nowType"></param>
    /// <returns></returns>
    private bool IsTargetType(params FoundType[] types)
    {
        var nowType = (FoundType)m_targetManager.GetNowTargetType();

        foreach (var type in types)
        {
            if (nowType == type)
            {
                return true;
            }
        }

        return false;
    }

    //アクセッサ-----------------------------------------------------------------------------------

    public ref TransitionMember GetTransitionMember()
    {
        return ref m_stateMachine.GetTransitionStructMember();
    }

    public override void StateReset()
    {
        m_stateMachine.ChangeState(StateType.Plowling, (int)StateType.Max);
    }

    public override void ChangeState<EnumType>(EnumType type, int priority = 0)
    {
        if (type is StateType)
        {
            StateType? stateType = type as StateType?;
            m_stateMachine.ChangeState((StateType)stateType, priority);
        }
    }

    /// <summary>
    /// ステート変更
    /// </summary>
    /// <param name="type">ステートタイプ</param>
    /// <param name="priority">優先度</param>
    public void ChangeState(StateType type, int priority)
    {
        m_stateMachine.ChangeState(type, priority);
    }

    public StateType GetNowState()
    {
        return m_stateMachine.GetNowType();
    }
}
