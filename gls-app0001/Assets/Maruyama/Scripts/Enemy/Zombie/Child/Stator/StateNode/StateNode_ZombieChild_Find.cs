using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ActionParametor = TaskNodeBase_Ex<EnemyBase>.ActionParametor;

using AnimatorBaseStateType = AnimatorManager_ZombieChild.BaseStateType;

public class StateNode_ZombieChild_Find : EnemyStateNodeBase<EnemyBase>
{
    public enum TaskEnum
    {
        Realize,  //気づいた状態
        Find,     //発見
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("気づいて様子見している時")]
        public Task_Realize.Parametor realizeParam;
        [Header("完全に気づいた時")]
        public Task_Find.Parametor findParam;
    }

    private Parametor m_param = new Parametor();
    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private AnimatorManager_ZombieChild m_animatorManager;
    private Stator_ZombieChild m_stator;

    public StateNode_ZombieChild_Find(EnemyBase owner, Parametor param)
        :base(owner)
    {
        m_param = param;

        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieChild>();
        m_stator = owner.GetComponent<Stator_ZombieChild>();

        DefineTask();
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();

        m_taskList.ForceReset();
        SelectTask();
    }

    public override void OnUpdate()
    {
        //Debug.Log("Find");
        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            m_stator.GetTransitionMember().cryTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        const float transitionTime = 0.1f;
        m_animatorManager.CrossFadeState(AnimatorBaseStateType.Idle.ToString(), m_animatorManager.BaseLayerIndex, transitionTime);
    }

    private void DefineTask()
    {
        var enemy = GetOwner();

        var realizeActionParam = new ActionParametor();
        realizeActionParam.enter = 
            () => m_animatorManager.CrossFadeState(AnimatorBaseStateType.Realize.ToString(), m_animatorManager.BaseLayerIndex);
        m_taskList.DefineTask(TaskEnum.Realize, new Task_Realize(enemy, m_param.realizeParam, realizeActionParam));

        var findActionParam = new ActionParametor();
        findActionParam.enter =
            () => m_animatorManager.CrossFadeState(AnimatorBaseStateType.Find.ToString(), m_animatorManager.BaseLayerIndex);
        m_taskList.DefineTask(TaskEnum.Find, new Task_Find(enemy, m_param.findParam, findActionParam));
    }

    private void SelectTask()
    {
        TaskEnum[] tasks = {
            TaskEnum.Realize,
            TaskEnum.Find
        };

        foreach (var task in tasks)
        {
            m_taskList.AddTask(task);
        }
    }


    //タスククラス-------------------------------------------------------------------------

    public class Task_Realize : TaskNodeBase_Ex<EnemyBase>
    {
        [System.Serializable]
        public struct Parametor
        {
            public float time;
            [Header("見つけた時のFindMarker")]
            public GameObject findMarker;
        }

        private Parametor m_param = new Parametor();
        private GameTimer m_timer = new GameTimer();

        private EnemyRotationCtrl m_rotationController;
        private EnemyVelocityManager m_velocityManager;
        private FindMarker m_findMarker;
        private TargetManager m_targetManager;

        public Task_Realize(EnemyBase owner, Parametor parametor)
            :this(owner, parametor, new ActionParametor())
        { }

        public Task_Realize(EnemyBase owner, Parametor parametor, ActionParametor actionParametor)
            : base(owner, actionParametor)
        {
            m_param = parametor;

            m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
            m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
            m_findMarker = owner.GetComponent<FindMarker>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            m_timer.ResetTimer(m_param.time);
            m_findMarker.ChangeMaker(m_param.findMarker);
            m_velocityManager.StartDeseleration();
        }

        public override bool OnUpdate()
        {
            Debug.Log("Realize");
            Rotation();
            m_timer.UpdateTimer();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            base.OnExit();

            m_velocityManager.SetIsDeseleration(false);
            m_findMarker.SetMarkerActive(false);
        }

        private void Rotation()
        {
            if (!m_targetManager.HasTarget()) {
                return;
            }

            var toTargretVec = (Vector3)m_targetManager.GetToNowTargetVector();
            m_rotationController.SetDirect(toTargretVec);
        }
    }


    public class Task_Find : TaskNodeBase_Ex<EnemyBase>
    {
        [System.Serializable]
        public struct Parametor
        {
            public float time;
            [Header("見つけた時のFindMarker")]
            public GameObject findMarker;
        }

        private Parametor m_param = new Parametor();
        private GameTimer m_timer = new GameTimer();

        private EnemyRotationCtrl m_rotationController;
        private EnemyVelocityManager m_velocityManager;
        private FindMarker m_findMarker;
        private TargetManager m_targetManager;

        public Task_Find(EnemyBase owner, Parametor parametor)
            :this(owner, parametor, new ActionParametor())
        { }

        public Task_Find(EnemyBase owner, Parametor parametor, ActionParametor actionParam)
            :base(owner, actionParam)
        {
            m_param = parametor;

            m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
            m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
            m_findMarker = owner.GetComponent<FindMarker>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            m_timer.ResetTimer(m_param.time);
            m_findMarker.ChangeMaker(m_param.findMarker);
            m_velocityManager.StartDeseleration();
        }

        public override bool OnUpdate()
        {
            Debug.Log("Find");
            Rotation();
            m_timer.UpdateTimer();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            base.OnExit();

            m_velocityManager.SetIsDeseleration(false);
            m_findMarker.SetMarkerActive(false);
        }

        private void Rotation()
        {
            if (!m_targetManager.HasTarget()) {
                return;
            }

            var toTargretVec = (Vector3)m_targetManager.GetToNowTargetVector();
            m_rotationController.SetDirect(toTargretVec);
        }
    }
}
