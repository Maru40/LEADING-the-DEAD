using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TaskBaseParametor = TaskNodeBase_Ex<EnemyBase>.BaseParametor;

public class AttackNode_Dash : TaskNodeBase<EnemyBase>
{
    private enum TaskEnum
    {
        Preliminary,
        Chase,
        Attack,
        Wait,
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("予備動作パラメータ")]
        public PreliminaryParametor preliminaryParam;
        [Header("追うパラメータ")]
        public Task_ChaseTarget.Parametor chaseParam;
        [Header("攻撃パラメータ")]
        public Task_WallAttack.Parametor attackParam;
        [Header("待機状態パラメータ")]
        public Task_Wait.Parametor waitParam;
        //[Header("行動確率")]
        //public float probability;
        //[Header("行動始める距離")]
        //public float startRange;
        //[Header("確率計算インターバル")]
        //public float probabilityInterbalTime;

        public Parametor(PreliminaryParametor preliminaryParam, Task_ChaseTarget.Parametor chaseParam,
            Task_WallAttack.Parametor attackParam, Task_Wait.Parametor waitParam)
            //float probability, float startRange, float probabilityIntervalTime)
        {
            this.preliminaryParam = preliminaryParam;
            this.chaseParam = chaseParam;
            this.attackParam = attackParam;
            this.waitParam = waitParam;
            //this.probability = probability;
            //this.startRange = startRange;
            //this.probabilityInterbalTime = probabilityIntervalTime;
        }
    }

    private Parametor m_param = new Parametor();
    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private EnemyVelocityManager m_velocityManager;
    private AnimatorManager_ZombieNormal m_animatorManager;
    private EnemyRotationCtrl m_rotationController;
    private ObstacleEvasion m_evasion;

    public AttackNode_Dash(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_evasion = owner.GetComponent<ObstacleEvasion>();

        DefineTask();
    }

    public override void OnEnter()
    {
        m_taskList.AbsoluteReset();

        SelectTask();
    }

    public override bool OnUpdate()
    {
        m_taskList.UpdateTask();

        return m_taskList.IsEnd;
    }

    public override void OnExit()
    {

    }

    /// <summary>
    /// タスクの選択
    /// </summary>
    private void SelectTask()
    {
        TaskEnum[] types = {
            TaskEnum.Preliminary, //予備動作
            TaskEnum.Chase,  //追いかける
            TaskEnum.Attack, //攻撃
            TaskEnum.Wait,   //待機
        };

        foreach (var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    private void DefineTask()
    {
        var enemy = GetOwner();

        //予備動作
        m_taskList.DefineTask(TaskEnum.Preliminary, 
            new Task_Preliminary(enemy, m_param.preliminaryParam, 
                new TaskBaseParametor(() => m_animatorManager.CrossFadePreliminaryNormalAttackAniamtion(), null, null)));

        //追従
        m_taskList.DefineTask(TaskEnum.Chase, 
            new Task_ChaseTarget(enemy, m_param.chaseParam,
                new TaskBaseParametor(() => m_animatorManager.CrossFadeDashAttackMove(), null, null)));

        //攻撃
        m_taskList.DefineTask(TaskEnum.Attack, 
            new Task_WallAttack(enemy, m_param.attackParam,
                new TaskBaseParametor(() => m_animatorManager.CrossFadeDashAttack(), null, null)));

        //待機
        m_param.waitParam.enter = () => { 
            m_velocityManager.StartDeseleration();
            m_rotationController.enabled = false;
            m_evasion.enabled = false;
        };
        m_param.waitParam.exit = () => {
            m_velocityManager.SetIsDeseleration(false);
            m_rotationController.enabled = true;
            m_evasion.enabled = true;
            //EndAnimationEvent();
        };
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));
    }
}
