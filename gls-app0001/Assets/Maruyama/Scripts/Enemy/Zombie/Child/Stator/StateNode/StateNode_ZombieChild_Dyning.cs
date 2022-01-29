using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieChild_Dyning : EnemyStateNodeBase<EnemyBase>
{
    public enum DyningType
    {
        Fire,
    }

    public enum TaskEnum
    {
        Fire,
    }

    [System.Serializable]
    public struct BlackBoardParametor
    {
        public DyningType type;
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("炎による死亡")]
        public Task_FireDining.Parametor fireParam;
    }

    private Parametor m_param = new Parametor();
    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private BlackBoard_ZombieChild m_blackBoard;
    private Stator_ZombieChild m_stator;

    public StateNode_ZombieChild_Dyning(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_blackBoard = owner.GetComponent<BlackBoard_ZombieChild>();
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
        Debug.Log("Dyning");

        m_taskList.UpdateTask();
        if (m_taskList.IsEnd)
        {
            m_stator.GetTransitionMember().deathTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void DefineTask()
    {
        var enemy = GetOwner();

        m_taskList.DefineTask(TaskEnum.Fire, new Task_FireDining(enemy, m_param.fireParam));
    }

    private void SelectTask()
    {
        TaskEnum[] tasks = m_blackBoard.GetStruct().dyningType switch {
            DyningType.Fire => new TaskEnum[]{ TaskEnum.Fire },
            _ => new TaskEnum[] { },
        };

        foreach(var task in tasks)
        {
            m_taskList.AddTask(task);
        }
    }
}
