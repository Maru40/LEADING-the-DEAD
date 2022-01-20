using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieChild_Cry : EnemyStateNodeBase<EnemyBase>
{
    public enum TaskEnum
    {
        Cry,
    }

    [System.Serializable]
    public struct Parametor
    {
        [Header("泣くパラメータ")]
        public Task_Cry.Parametor cryParam;
    }

    private Parametor m_param = new Parametor();

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private Stator_ZombieChild m_stator;

    public StateNode_ZombieChild_Cry(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_stator = owner.GetComponent<Stator_ZombieChild>();

        DefineTask();
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();

        m_taskList.AbsoluteReset();

        SelectTask();
    }

    public override void OnUpdate()
    {
        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            m_stator.GetTransitionMember().escapeTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void DefineTask()
    {
        var enemy = GetOwner();

        m_taskList.DefineTask(TaskEnum.Cry, new Task_Cry(enemy, m_param.cryParam));
    }

    private void SelectTask()
    {
        TaskEnum[] tasks = {
            TaskEnum.Cry
        };

        foreach(var task in tasks)
        {
            m_taskList.AddTask(task);
        }
    }
}
