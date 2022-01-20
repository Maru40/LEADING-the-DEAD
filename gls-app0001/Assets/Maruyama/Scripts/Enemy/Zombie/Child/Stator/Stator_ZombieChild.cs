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
        Chase,    //追いかける
        Escape,   //逃げる
        Cry,      //泣く
        Max
    }

    public struct TransitionMember
    {
        public MyTrigger plowlingTrigger;
        public MyTrigger escapeTrigger;
        public MyTrigger cryTrigger;
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("発見時に泣く対象")]
        public List<FoundType> cryTargets;
        [Header("泣くパラメータ")]
        public StateNode_ZombieChild_Cry.Parametor cryParam;
        [Header("逃げるパラメータ")]
        public StateNode_ZombieChild_Escape.Parametor escapeParam;
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
        m_stateMachine.AddNode(StateType.Cry, new StateNode_ZombieChild_Cry(enemy, m_param.cryParam));
    }

    private void CreateEdge()
    {
        //徘徊
        m_stateMachine.AddEdge(StateType.Plowling, StateType.Cry, IsCryTrigger);
        m_stateMachine.AddEdge(StateType.Plowling, StateType.Cry, IsFindCryTarget);

        //逃げる
        m_stateMachine.AddEdge(StateType.Escape, StateType.Plowling, IsPlowlingTrigger);

        //泣く
        m_stateMachine.AddEdge(StateType.Cry, StateType.Escape, IsEscapeTrigger);

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

    }
}
