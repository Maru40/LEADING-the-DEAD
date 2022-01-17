using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using NormalAttackHitColliderType = AnimatorManager_ZombieNormal.NormalAttackHitColliderType;
using TaskBaseParametor = TaskNodeBase_Ex<EnemyBase>.BaseParametor;

public class AttackNode_Normal : TaskNodeBase<EnemyBase>
{
    private enum TaskEnum 
    { 
        Preliminary,  //予備動作
        Chase,
        Deseleration,  //減速行動
        Wait,  //待機時間
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("予備動作パラメータ")]
        public PreliminaryParametor preliminaryParam;
        [Header("追従パラメータ")]
        public Task_AttackChase.Parametor attackParam;
        [Header("待機パラメータ")]
        public Task_Wait.Parametor waitParam;
    }

    private Parametor m_param;
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

    public AttackNode_Normal(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_evasion = owner.GetComponent<ObstacleEvasion>();

        DefineTask();
        SettingAnimation();
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
        m_taskList.AbsoluteReset();
    }

    private void DefineTask()
    {
        var enemy = GetOwner();

        //予備動作
        m_taskList.DefineTask(TaskEnum.Preliminary, new Task_Preliminary(enemy, m_param.preliminaryParam));

        //攻撃
        //m_param.attackParam.enterAnimation = m_animatorManager.CrossFadeNormalAttackAnimation;
        m_taskList.DefineTask(TaskEnum.Chase, 
            new Task_AttackChase(enemy, m_param.attackParam,
                new TaskBaseParametor(() => m_animatorManager.CrossFadeNormalAttackAnimation(), null, null)));

        //減速
        m_taskList.DefineTask(TaskEnum.Deseleration, 
            () => { m_velocityManager.StartDeseleration(); }, 
            () => { return false; }, 
            () => { m_velocityManager.SetIsDeseleration(false); });

        //待機状態
        m_param.waitParam.enter = () => {
            m_velocityManager.SetIsDeseleration(false);
            m_velocityManager.ResetForce();
            m_velocityManager.ResetVelocity();
            m_rotationController.enabled = false;
            m_evasion.enabled = false;
        };
        m_param.waitParam.exit = () => { 
            m_rotationController.enabled = true;
            m_evasion.enabled = true;
        };
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));
    }

    private void SelectTask()
    {
        TaskEnum[] tasks = { 
            TaskEnum.Preliminary,
            TaskEnum.Chase,
            TaskEnum.Deseleration,
        };

        foreach(var task in tasks)
        {
            m_taskList.AddTask(task);
        }
    }

    private void SettingAnimation()
    {
        var animator = m_animatorManager.animator;
        var behavior = ZombieNormalTable.UpperLayer.NormalAttack.GetBehaviour<TimeEventStateMachineBehaviour>(animator);

        var timeParam = m_animatorManager.NormalAttackParametor;
        var leftTimeParam = timeParam[NormalAttackHitColliderType.Left];
        var timeEvent = behavior.onTimeEvent;
        //左腕の攻撃に合わせて次のタスクにする
        timeEvent.ClampWhere(leftTimeParam.startTime)
            .Subscribe(_ => m_taskList.AbsoluteNextTask())
            .AddTo(GetOwner());

        behavior.onStateExited.Subscribe(_ => EndAnimation()).AddTo(GetOwner());
    }

    private void EndAnimation()
    {
        m_taskList.AbsoluteReset();
        m_taskList.AddTask(TaskEnum.Wait);
    }
}
