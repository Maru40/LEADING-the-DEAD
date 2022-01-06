using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_KnockBack_EnemyBase : EnemyStateNodeBase<EnemyBase>
{
    private enum TaskEnum
    {
        KnockBack,
        Stun,
    }

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    //コンポーネント系

    private StatusManagerBase m_status;
    private KnockBackManager m_knockBackManager;
    private I_Stun m_iStun;
    private StatorBase m_stator;

    public StateNode_KnockBack_EnemyBase(EnemyBase owner)
        :base(owner)
    {
        m_status = owner.GetComponent<StatusManagerBase>();
        m_knockBackManager = owner.GetComponent<KnockBackManager>();
        m_stator = owner.GetComponent<StatorBase>();
        m_iStun = owner.GetComponent<I_Stun>();

        DifineTask();
    }

    protected override void ReserveChangeComponents()
    {
        
    }

    public override void OnStart()
    {
        base.OnStart();

        SelectTask();
    }

    public override void OnUpdate()
    {
        Debug.Log("KnockBack");

        m_taskList.UpdateTask();
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    private void DifineTask()
    {
        m_taskList.DefineTask(TaskEnum.KnockBack, new Task_KnockBack());
        m_taskList.DefineTask(TaskEnum.Stun, new Task_Stun(m_iStun));
    }

    private void SelectTask()
    {
        m_taskList.AddTask(TaskEnum.KnockBack);

        if (m_status.IsStun)
        {
            m_taskList.AddTask(TaskEnum.Stun);
        }
        else
        {
            Debug.Log("Stun出ない");
        }
    }

    //タスク-----------------------------------------------

    private class Task_KnockBack : TaskNodeBase
    {
        private GameTimer m_timer = new GameTimer();
        private float m_time = 0.5f;

        public override void OnEnter()
        {
            m_timer.ResetTimer(m_time);
        }

        public override bool OnUpdate()
        {
            Debug.Log("Task_Knock");

            m_timer.UpdateTimer();
            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {

        }
    }


    private class Task_Stun : TaskNodeBase
    {
        private I_Stun m_iStun;

        public Task_Stun(I_Stun stun)
        {
            m_iStun = stun;
        }

        public override void OnEnter()
        {
            Debug.Log("StunEnter");
            m_iStun.StartStun();
        }

        public override bool OnUpdate()
        {
            return true;
        }

        public override void OnExit()
        {
            
        }
    }
}
