using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : AttackNodeBase
{
    enum TaskEnum {
        Preliminary,
        Chase,
        Attack,
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
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    void DefineTask()
    {
        var enemy = GetComponent<EnemyBase>();

        m_taskList.DefineTask(TaskEnum.Preliminary, new Task_Preliminary(enemy, m_param.preliminaryParam));
        m_taskList.DefineTask(TaskEnum.Chase, new Task_ChaseTarget(enemy));
        m_taskList.DefineTask(TaskEnum.Attack, new Task_WallAttack(enemy));
    }

    void SelectTask()
    {
        TaskEnum[] types = { 
            TaskEnum.Preliminary,
            TaskEnum.Chase, 
            TaskEnum.Attack 
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
    }
}
