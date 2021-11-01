using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DyingTypeEnum = StatusManager_ZombieNormal.DyingTypeEnum;

public class StateNode_ZombieNormal_Dying : EnemyStateNodeBase<EnemyBase>
{
    enum TaskEnum
    {
        Fire
    }

    float m_fireTime = 1.0f;  //炎に包まれながら動く時間

    StatusManager_ZombieNormal m_statusManager;
    Stator_ZombieNormal m_stator;
    WaitTimer m_waitTimer;

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    public StateNode_ZombieNormal_Dying(EnemyBase owner)
        :base(owner)
    {
        m_statusManager = owner.GetComponent<StatusManager_ZombieNormal>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_waitTimer = owner.GetComponent<WaitTimer>();
    }

    protected override void ReserveChangeComponents()
    {
        //特になし
    }

    public override void OnStart()
    {
        base.OnStart();

        //タスクの定義
        DefineTask();

        //タスクのセレクト
        SelectTask();
    }

    public override void OnUpdate()
    {
        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)  //タスクが終了したら。
        {
            //死亡状態に変更
            m_stator.GetTransitionMember().deathTrigger.Fire();
        }
    }

    /// <summary>
    /// タスク定義
    /// </summary>
    void DefineTask()
    {
        m_taskList.DefineTask(TaskEnum.Fire, OnTaskFireEnter, OnTaskFireUpdate, OnTaskFireExit);
    }

    /// <summary>
    /// タスクの選択
    /// </summary>
    void SelectTask()
    {
        var dyingType = m_statusManager.DyingType;

        var types = dyingType switch
        {
            DyingTypeEnum.Fire => new TaskEnum[] { TaskEnum.Fire }, //炎に包まれた時
            _ => new TaskEnum[] { }
        };

        foreach(var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    //フェードして消える-----------------------------------------------------------------------------------------

    void OnTaskFadeEnter()
    {

    }

    bool OnTaskFadeUpdate()
    {
        return true;
    }

    void OnTaskFadeExit()
    {

    }

    //ファイアー--------------------------------------------------------------------------------------

    void OnTaskFireEnter()
    {
        
    }

    bool OnTaskFireUpdate()
    {
        return true;
    }

    void OnTaskFireExit()
    {

    }
}
