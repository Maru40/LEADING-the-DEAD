using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

/// <summary>
/// 一つのタスクのベースクラス
/// </summary>
public abstract class TaskNodeBase
{
    public abstract void OnEnter();
    public abstract bool OnUpdate();
    public abstract void OnExit();
}

public abstract class TaskNodeBase<OwnerType> : TaskNodeBase
    where OwnerType : class
{
    protected OwnerType m_owner;

    public TaskNodeBase(OwnerType owner)
    {
        m_owner = owner;
    }

    protected OwnerType Owner => m_owner;

    protected OwnerType GetOwner()
    {
        return m_owner;
    }
}

public abstract class TaskNodeBase_Ex<OwnerType> : TaskNodeBase<OwnerType>
    where OwnerType : class
{
    public struct BaseParametor
    {
        public Action enter;
        public Action update;
        public Action exit;
    }

    private BaseParametor m_actionParam = new BaseParametor();

    public TaskNodeBase_Ex(OwnerType owner, BaseParametor param = new BaseParametor())
        :base(owner)
    {
        m_actionParam = param;
    }

    public override void OnEnter()
    {
        m_actionParam.enter?.Invoke();
    }

    public override bool OnUpdate()
    {
        m_actionParam.update?.Invoke();
        return true;
    }

    public override void OnExit()
    {
        m_actionParam.exit?.Invoke();
    }
}

public class TaskList<EnumType>
{
    class Task
    {
        public EnumType type;
        public Action enter;
        public Func<bool> update;
        public Action exit;

        public Task(EnumType type, Action enter, Func<bool> update, Action exit)
        {
            this.type = type;
            this.enter = enter;
            this.update = update ?? delegate { return true; };
            this.exit = exit;
        }

        public Task(EnumType type, TaskNodeBase task)
        {
            this.type = type;
            this.enter = task.OnEnter;
            this.update = task.OnUpdate;
            this.exit = task.OnExit;
        }
    }

    /// <summary>
    /// 定義されたタスク
    /// </summary>
    private Dictionary<EnumType, Task> m_defineTaskDictionary = new Dictionary<EnumType, Task>();

    /// <summary>
    /// 現在積まれているタスク
    /// </summary>
    private List<Task> m_currentTasks = new List<Task>();
    /// <summary>
    /// 現在動作しているタスク
    /// </summary>
    private Task m_currentTask = null;
    /// <summary>
    /// 現在動作しているタスクのIndex
    /// </summary>
    private int m_currentIndex = 0;

    /// <summary>
    /// 毎フレーム呼ぶ処理(外部でUpdate管理)
    /// </summary>
    public void UpdateTask()
    {
        if (IsEnd) {  //終了状態なら処理を行わない
            return;
        }

        if(m_currentTask == null) //カレントがnullなら
        {
            m_currentTask = m_currentTasks[m_currentIndex];  //現在のタスクの取得
            m_currentTask.enter?.Invoke();
        }

        //タスクのUpdate
        bool isEndOneTask = m_currentTask.update();

        //タスクが終了したら
        if (isEndOneTask) {
            EndOneTask();
        }
    }

    /// <summary>
    /// 一つのタスクの終了時
    /// </summary>
    private void EndOneTask()
    {
        m_currentTask.exit?.Invoke();  //現在のタスクのExit

        m_currentIndex++; //Indexの更新

        if (IsEnd)  //次のタスクがないなら
        {
            m_currentIndex = 0;
            m_currentTask = null;
            m_currentTasks.Clear();
            return;
        }

        m_currentTask = m_currentTasks[m_currentIndex]; //次のタスクを取得
        m_currentTask.enter?.Invoke(); //次のタスクのEnter
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    /// <param name="type">EnumType</param>
    /// <param name="enter"></param>
    /// <param name="update"></param>
    /// <param name="exit"></param>
    public void DefineTask(EnumType type, TaskNodeBase task)
    {
        DefineTask(type, task.OnEnter, task.OnUpdate, task.OnExit);
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    /// <param name="type">EnumType</param>
    /// <param name="enter"></param>
    /// <param name="update"></param>
    /// <param name="exit"></param>
    public void DefineTask(EnumType type, Action enter, Func<bool> update, Action exit)
    {
        var task = new Task(type, enter, update, exit);
        var exist = m_defineTaskDictionary.ContainsKey(type);
        if (exist)
        {
            Debug.Log("既に追加されています。");
            return;
        }
        m_defineTaskDictionary.Add(type, task);
    }

    /// <summary>
    /// タスクの登録
    /// </summary>
    /// <param name="type"></param>
    public void AddTask(EnumType type)
    {
        Task task = null;
        //存在したら取得できる関数
        var exist = m_defineTaskDictionary.TryGetValue(type, out task);
        if(exist == false) 
        {
            Debug.Log("タスクが登録されていません");
            return;
        }

        m_currentTasks.Add(task);
    }

    /// <summary>
    /// 強制終了
    /// </summary>
    public void AbsoluteStop()
    {
        if(m_currentTask != null)
        {
            m_currentTask.exit?.Invoke();
        }
        m_currentTask = null;
        m_currentTasks.Clear();
        m_currentIndex = 0;
    }

    /// <summary>
    /// 強制的に次のタスクに変更する。
    /// </summary>
    public void AbsoluteNextTask()
    {
        EndOneTask();
    }

    /// <summary>
    /// 終了処理を呼ばない強制終了
    /// </summary>
    public void AbsoluteReset()
    {
        m_currentTask = null;
        m_currentTasks.Clear();
        m_currentIndex = 0;
    }

    //アクセッサ-------------------------------------------------------------------------------------------

    /// <summary>
    /// 全てのタスクが終了しているかどうか
    /// </summary>
    public bool IsEnd =>
        m_currentTasks.Count <= m_currentIndex;

    /// <summary>
    /// タスクが動いているかどうか
    /// </summary>
    public bool IsMoveTask => m_currentTask != null;

    /// <summary>
    /// 現在進行中のタスクのタイプを取得
    /// </summary>
    public EnumType CurrentTaskType => (m_currentTask == null) ? default(EnumType) : m_currentTask.type;

    /// <summary>
    /// 追加されているタスクのタイプリスト
    /// </summary>
    public List<EnumType> CurrentTaskTypeList
    {
        get => m_currentTasks.Select(x => x.type).ToList();
    }

    /// <summary>
    /// 現在のインデックスの取得
    /// </summary>
    public int CurrentIndex => m_currentIndex;
}
