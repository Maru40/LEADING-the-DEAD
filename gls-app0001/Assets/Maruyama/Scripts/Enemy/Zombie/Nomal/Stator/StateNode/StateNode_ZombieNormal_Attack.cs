using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Attack : EnState_AttackBase
{
    public enum StateType
    {
        None,
        Normal,
        Dash,
        WallAttack,
        Max
    }

    public struct BlackBoardParametor
    {
        public StateType startType;
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("通常攻撃")]
        public AttackNode_Normal.Parametor normalParam;
        [Header("ダッシュ攻撃")]
        public AttackNode_Dash.Parametor dashParam;
        [Header("壁攻撃")]
        public AttackNode_WallDash.Parametor wallAttackParam;
    }

    private Parametor m_param =  new Parametor();

    private TargetManager m_targetManager;
    private Stator_ZombieNormal m_stator;
    private BlackBoard_ZombieNormal m_blackBoard;

    private TaskList<StateType> m_taskList = new TaskList<StateType>();

    public StateNode_ZombieNormal_Attack(EnemyBase owner, Parametor parametor)
        : base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_blackBoard = owner.GetComponent<BlackBoard_ZombieNormal>();

        DefineTask();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<AttackManager_ZombieNormal>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        //SelectState();
        SelectTask();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            m_stator.GetTransitionMember().chaseTrigger.Fire();
        }

        //ターゲットが存在しない
        if (!m_targetManager.HasTarget())
        {
            m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        m_taskList.ForceReset();
        m_blackBoard.Struct.attackParam.startType = StateType.None;
    }

    private void DefineTask()
    {
        var enemy = GetOwner();

        m_taskList.DefineTask(StateType.Normal, new AttackNode_Normal(enemy, m_param.normalParam));

        m_taskList.DefineTask(StateType.Dash, new AttackNode_Dash(enemy, m_param.dashParam));

        m_taskList.DefineTask(StateType.WallAttack, new AttackNode_WallDash(enemy, m_param.wallAttackParam));
    }

    private void SelectTask()
    {
        StateType[] tasks = m_blackBoard.GetStruct().attackParam.startType switch
        {
            StateType.Normal => new StateType[] { StateType.Normal },
            StateType.Dash => new StateType[] { StateType.Dash },
            StateType.WallAttack => new StateType[] { StateType.WallAttack, },
            _ => new StateType[] { }
        };

        foreach (var task in tasks)
        {
            m_taskList.AddTask(task);
        }
    }
}
