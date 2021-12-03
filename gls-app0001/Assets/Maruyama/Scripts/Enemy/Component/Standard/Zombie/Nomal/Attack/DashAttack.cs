using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : AttackNodeBase
{
    enum TaskEnum {
        Preliminary,
        Chase,
        Attack,
        Wait,
    }

    [System.Serializable]
    struct Parametor
    {
        [Header("最大スピード")]
        public float maxSpeed;
        [Header("曲がれる最大角度")]
        public float maxTurningDegree;
        [Header("予備動作パラメータ")]
        public PreliminaryParametor preliminaryParam;
        [Header("追うパラメータ")]
        public Task_ChaseTarget.Parametor chaseParam;
        [Header("攻撃パラメータ")]
        public Task_WallAttack.Parametor attackParam;
        [Header("待機状態パラメータ")]
        public Task_Wait.Parametor waitParam;
    }

    [SerializeField]
    Parametor m_param = new Parametor();

    TargetManager m_targetManager;
    EyeSearchRange m_eye;
    AttackNodeManagerBase m_attackManager;

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_eye = GetComponent<EyeSearchRange>();
        m_attackManager = GetComponent<AttackNodeManagerBase>();
    }

    private void Start()
    {
        DefineTask();

        enabled = false;
    }

    void Update()
    {
        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            EndAnimationEvent();
        }
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    void DefineTask()
    {
        var enemy = GetComponent<EnemyBase>();

        m_taskList.DefineTask(TaskEnum.Preliminary, new Task_Preliminary(enemy, m_param.preliminaryParam));
        m_taskList.DefineTask(TaskEnum.Chase, new Task_ChaseTarget(enemy, m_param.chaseParam));
        m_taskList.DefineTask(TaskEnum.Attack, new Task_WallAttack(enemy, m_param.attackParam));
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));
    }

    void SelectTask()
    {
        TaskEnum[] types = { 
            TaskEnum.Preliminary,
            TaskEnum.Chase, 
            TaskEnum.Attack,
            TaskEnum.Wait,
        }; 

        foreach(var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        var position = m_targetManager.GetNowTargetPosition();
        if (position != null)
        {
            return m_eye.IsInEyeRange((Vector3)position, range);
        }

        return false;
    }

    public override void AttackStart()
    {
        SelectTask();
    }

    public override void EndAnimationEvent()
    {
        m_attackManager.EndAnimationEvent();
        enabled = false;
    }
}
