using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TaskParametor = TaskList_Ex<StateTester.TaskEnum>.TaskNode.Parametor;

public class Task_TestA : TaskNodeBase<StateTester>
{
    GameTimer m_timer = new GameTimer();

    public Task_TestA(StateTester owner)
        :base(owner)
    { }

    public override void OnEnter()
    {
        m_timer.ResetTimer(5.0f);
    }

    public override bool OnUpdate()
    {
        Debug.Log("A");
        m_timer.UpdateTimer();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        Debug.Log("A終了");
        Debug.Break();
    }
}

public class Task_TestB : TaskNodeBase<StateTester>
{
    GameTimer m_timer = new GameTimer();

    public Task_TestB(StateTester owner)
        : base(owner)
    { }

    public override void OnEnter()
    {
        m_timer.ResetTimer(3.0f);
    }

    public override bool OnUpdate()
    {
        Debug.Log("B");

        m_timer.UpdateTimer();
        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        Debug.Log("B終了");
        Debug.Break();
    }
}

public class StateTester : MonoBehaviour
{
    public enum TaskEnum
    {
        A,
        B
    }

    TaskList_Ex<TaskEnum> m_taskList = new TaskList_Ex<TaskEnum>();

    void Start()
    {
        DefineTask();

        SelectTask();
    }

    void Update()
    {
        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            SelectTask();
        }
    }

    void DefineTask()
    {
        m_taskList.DefineTask(TaskEnum.A, new Task_TestA(this));
        m_taskList.DefineTask(TaskEnum.B, new Task_TestB(this));
    }

    void SelectTask()
    {
        m_taskList.AddTask(new TaskParametor(TaskEnum.A, () => { return true; }, true));

        var parametors = new TaskParametor[]{
            new TaskParametor(TaskEnum.A, () => { return true; }, false),
            new TaskParametor(TaskEnum.B, IsTransition, (int)TaskEnum.B, 0.0f, false),
        };

        m_taskList.AddTask(parametors);

        m_taskList.AddTask(new TaskParametor(TaskEnum.A, () => { return true; }, true));
    }

    bool IsTransition()
    {
        return true;
    }
}
