using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Attack : EnState_AttackBase
{
    public enum AttackType
    {
        Normal,
        Dash,
        WallAttack,
    }

    private enum TaskEnum
    {
        Wait, //待機
        Preliminary, //予備動作
    }

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private TargetManager m_targetManager;
    private Stator_ZombieNormal m_stator;
    private BlackBoard_ZombieNormal m_blackBoard;

    public StateNode_ZombieNormal_Attack(EnemyBase owner)
        : base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_blackBoard = owner.GetComponent<BlackBoard_ZombieNormal>();

        DefineTask();
    }

    protected override void PlayStartAnimation()
    {
        
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<AttackManager_ZombieNormal>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        SelectTask();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_taskList.UpdateTask();
        if(m_taskList.IsEnd)
        {
            m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
            return;
        }

        if (!m_targetManager.HasTarget())
        {
            m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        m_taskList.AbsoluteReset();
    }

    private void DefineTask()
    {

    }

    private void SelectTask()
    {
        TaskEnum[] tasks = m_blackBoard.Struct.attackType switch {
            AttackType.Normal => new TaskEnum[]{ TaskEnum.Wait },
            AttackType.Dash => new TaskEnum[] { },
            AttackType.WallAttack => new TaskEnum[] { },
            _ => null
        };

        foreach(var task in tasks)
        {
            m_taskList.AddTask(task);
        }
    }
}
