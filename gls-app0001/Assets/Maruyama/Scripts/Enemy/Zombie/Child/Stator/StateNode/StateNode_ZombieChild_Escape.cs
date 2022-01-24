using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

using StateMachine =
    EnemyMainStateMachine<EnemyBase, StateNode_ZombieChild_Escape.StateType, StateNode_ZombieChild_Escape.TransitionMember>;

public class StateNode_ZombieChild_Escape : EnemyStateNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("遷移条件関係")]
        public TransitionMember transitionMember;
        [Header("逃げる時のパラメータ")]
        public Escape.Parametor escapeParam;
        [Header("逃げ切ってから警戒するパラメータ")]
        public Guard.Parametor guardParam;
    }

    public enum StateType
    {
        Escape,     //逃げている状態
        Guard,      //警戒
    }

    [System.Serializable]
    public struct TransitionMember
    {
        [Header("逃げ切ったと判断する距離")]
        public float escapeRange;
    }

    private Parametor m_param = new Parametor();

    private StateMachine m_stateMachine;

    private TargetManager m_targetManager = null;
    //private AllEnemyGeneratorManager m_enemyGenerator = null;

    public StateNode_ZombieChild_Escape(EnemyBase owner, Parametor parametor)
        : base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        //m_enemyGenerator = GameObject.FindObjectOfType<AllEnemyGeneratorManager>();

        m_stateMachine = new StateMachine(m_param.transitionMember);
        CreateNode();
        CreateEdge();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<FoundObject>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        Debug.Log("△△ Escape");

        m_stateMachine.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        m_targetManager.SetNowTarget(GetType(), null);
    }

    private void CreateNode()
    {
        var enemy = GetOwner();

        m_stateMachine.AddNode(StateType.Escape, new Escape(enemy, m_param.escapeParam));
        m_stateMachine.AddNode(StateType.Guard, new Guard(enemy, m_param.guardParam));
    }

    private void CreateEdge()
    {
        //逃げている状態
        m_stateMachine.AddEdge(StateType.Escape, StateType.Guard, IsGuard);

        //警戒状態
        m_stateMachine.AddEdge(StateType.Guard, StateType.Escape, IsEscape);
    }

    //遷移条件----------------------------------------------------------------------------------------------------

    private bool IsGuard(ref TransitionMember member)
    {
        //ターゲットが範囲外ならtrue
        return !IsTargetRange(ref member) ? true : false;
    }

    private bool IsEscape(ref TransitionMember member)
    {
        //ターゲットが範囲内にいるなら逃げ続ける
        return IsTargetRange(ref member) ? true : false;
    }

    /// <summary>
    /// ターゲットが範囲内にいるかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsTargetRange(ref TransitionMember member)
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        var owner = GetOwner();
        var target = m_targetManager.GetNowTarget();

        return Calculation.IsRange(owner.gameObject, target.gameObject, member.escapeRange) ? true : false;
    }

    //StateNode-------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 逃げる状態
    /// </summary>
    public class Escape : NodeBase<EnemyBase>
    {
        [System.Serializable]
        public struct Parametor
        {
            [Header("逃げるパラメータ")]
            public Task_Escape.Parametor escapeParam;
            //public Task_NavMeshEscape.Parametor escapeParam;
        }

        public enum TaskEnum
        {
            Escape,
        }

        private Parametor m_param = new Parametor();

        private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

        public Escape(EnemyBase owner, Parametor parametor)
            :base(owner)
        {
            m_param = parametor;

            DefineTask();
        }

        public override void OnStart()
        {
            m_taskList.AbsoluteReset();

            SelectTask();
        }

        public override void OnUpdate()
        {
            m_taskList.UpdateTask();
        }

        public override void OnExit()
        {
            m_taskList.AbsoluteStop();
        }

        private void DefineTask()
        {
            var enemy = GetOwner();

            m_taskList.DefineTask(TaskEnum.Escape, new Task_Escape(enemy, m_param.escapeParam));
        }

        private void SelectTask()
        {
            TaskEnum[] tasks = {
                TaskEnum.Escape
            };

            foreach(var task in tasks)
            {
                m_taskList.AddTask(task);
            }
        }
    }

    /// <summary>
    /// 逃げ切ったと判断してからの猶予時間
    /// </summary>
    public class Guard : NodeBase<EnemyBase>
    {
        [System.Serializable]
        public struct Parametor
        {
            [Header("逃げ切ってから警戒する時間")]
            public float time;
            [Header("逃げるパラメータ")]
            public Task_Escape.Parametor escapeParam;
            //public Task_NavMeshEscape.Parametor escapeParam;
        }

        public enum TaskEnum
        {
            Escape,
        }

        private Parametor m_param = new Parametor();

        private GameTimer m_timer = new GameTimer();
        private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

        private Stator_ZombieChild m_stator;

        public Guard(EnemyBase owner, Parametor parametor)
            :base(owner)
        {
            m_param = parametor;

            m_stator = owner.GetComponent<Stator_ZombieChild>();

            DefineTask();
        }

        public override void OnStart()
        {
            m_timer.ResetTimer(m_param.time);

            SelectTask();
        }

        public override void OnUpdate()
        {
            m_taskList.UpdateTask();
            m_timer.UpdateTimer();

            if (m_timer.IsTimeUp)
            {
                m_stator.GetTransitionMember().plowlingTrigger.Fire();
            }
        }

        public override void OnExit()
        {
            m_taskList.AbsoluteStop();
        }

        private void DefineTask()
        {
            var enemy = GetOwner();

            m_taskList.DefineTask(TaskEnum.Escape, new Task_Escape(enemy, m_param.escapeParam));
        }

        private void SelectTask()
        {
            m_taskList.AddTask(TaskEnum.Escape);
        }
    }

}
