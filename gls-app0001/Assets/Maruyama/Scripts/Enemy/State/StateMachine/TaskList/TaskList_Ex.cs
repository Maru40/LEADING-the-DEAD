using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public class TaskList_Ex<EnumType>
{
    /// <summary>
    /// タスク
    /// </summary>
    public class Task
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

    //それぞれのタスク
    public class TaskNode
    {
        public class Parametor
        {
            readonly public EnumType type;
            public Func<bool> IsTransition = null;
            public int priority = 0;       //優先度
            public float weight = 0.0f;    //重要度
            public bool isEndExit = false; //終了時にExitするかどうか

            public Parametor(EnumType type, Func<bool> isTransition, bool isEndExit = false)
                : this(type, isTransition, 0, 0.0f, isEndExit)
            { }

            public Parametor(EnumType type, Func<bool> IsTransition, int priority, float weight, bool isEndExit)
            {
                this.type = type;
                this.IsTransition = IsTransition;
                this.priority = priority;
                this.weight = weight;
                this.isEndExit = isEndExit;
            }
        }

        readonly public Task task = null;
        private Parametor parametor = null;

        public TaskNode(Task task, Func<bool> IsTransition)
            :this(task, IsTransition, 0, 0.0f, false)
        {}

        public TaskNode(Task task, Func<bool> IsTransition, int priority, float weight, bool isEndExit)
            :this(task, new Parametor(task.type ,IsTransition, priority, weight, isEndExit))
        { }

        public TaskNode(Task task, Parametor parametor)
        {
            this.task = task;
            this.parametor = parametor;
        }

        public bool IsTransition(bool isEndTask = false)
        {
            if (parametor.isEndExit) //終了待ちなら
            {
                return isEndTask ? parametor.IsTransition() : false;
            }

            return parametor.IsTransition();
        }
        public int Priority => parametor.priority;
        public float Weight => parametor.weight;
        public bool IsEndExit => parametor.isEndExit;
    }

    public class TaskPach
    {
        List<TaskNode> m_taskNodes;
    }


    /// <summary>
    /// 定義されたタスク
    /// </summary>
    private Dictionary<EnumType, Task> m_defineTaskDictionary = new Dictionary<EnumType, Task>();

    /// <summary>
    /// 現在積まれているタスク
    /// </summary>
    private List<List<TaskNode>> m_currentTasks = new List<List<TaskNode>>();
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
        if (IsEnd)
        {  //終了状態なら処理を行わない
            return;
        }

        //タスクのUpdate
        bool isEndOneTask = m_currentTask.update();

        StateCheck(isEndOneTask);
    }

    //ステートチェック
    private void StateCheck(bool isEndOneTask)
    {
        var task = SelectTask(NextTaskNodes, isEndOneTask);

        if(task != null)
        {
            NextState(task);
            return;
        }

        if (isEndOneTask && !IsNextTaskNodes) //タスクの終了で、次のタスクリストが存在しない場合
        {
            m_currentTask.exit?.Invoke();  //現在のタスクのExit

            m_currentIndex = 0;
            m_currentTask = null;
            m_currentTasks.Clear();
        }
    }

    private void NextState(Task task)
    {
        m_currentTask.exit?.Invoke();

        m_currentIndex++;

        m_currentTask = task;
        m_currentTask.enter?.Invoke(); //次のタスクのEnter
    }

    //タスクを選択する。
    private Task SelectTask(List<TaskNode> taskNodes, bool isEndOneTask = false)
    {
        if(taskNodes.Count == 0) {
            return null;
        }

        //アクティブなタスクの取得
        var activeTasks = taskNodes.Where(task => task.IsTransition(isEndOneTask)).ToList();
        if(activeTasks.Count == 0) {
            return null;
        }

        //プライオリティが一番高いタスクの取得
        int maxPriority = activeTasks.Max(task => task.Priority);
        var priorityTasks = activeTasks.Where(task => task.Priority == maxPriority).ToList();
        if (priorityTasks.Count == 0) {
            return null;
        }

        //その中から重要度の高いタスクを選択
        var task = CalcuRandomSelectTask(priorityTasks);

        return task;
    }

    //重要度の高いタスクからランダムに取得する
    private Task CalcuRandomSelectTask(List<TaskNode> taskNodes)
    {
        var total = taskNodes.Sum(task => task.Weight);
        var randomWeight = UnityEngine.Random.value * total;

        float sumWeight = 0;
        foreach(var node in taskNodes)
        {
            sumWeight += node.Weight;
            if(sumWeight >= randomWeight)
            {
                return node.task;
            }
        }

        return null;
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
    /// <param name="parametors"></param>
    public void AddTask(params TaskNode.Parametor[] parametors)
    {
        var tasks = new List<TaskNode>();
        foreach(var param in parametors)
        {
            if (!m_defineTaskDictionary.ContainsKey(param.type))
            {
                Debug.Log("タスクが登録されていません");
                continue;
            }

            var task = m_defineTaskDictionary[param.type];
            tasks.Add(new TaskNode(task, param));
        }

        if(tasks.Count == 0) {  //タスクが存在しないなら処理をしない
            return; 
        }

        m_currentTasks.Add(tasks);

        if(m_currentTask == null) //カレントタスクが無いなら
        {
            m_currentTask = tasks[0].task;
            m_currentTask.enter?.Invoke();
        }
    }

    /// <summary>
    /// 強制終了
    /// </summary>
    public void AbsoluteStop()
    {
        if (m_currentTask != null)
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
    //public void AbsoluteNextTask()
    //{
    //    EndOneTask();
    //}

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
    //public List<EnumType> CurrentTaskTypeList
    //{
    //    get => m_currentTasks.Select(x => x.type).ToList();
    //}

    /// <summary>
    /// 現在のインデックスの取得
    /// </summary>
    public int CurrentIndex => m_currentIndex;

    //次のタスクがあるかどうか
    private bool IsNextTaskNodes => m_currentTasks.Count > m_currentIndex + 1;

    //次のタスクノードを取得
    private List<TaskNode> NextTaskNodes
    {
        get
        {
            if (IsNextTaskNodes)
            {
                return m_currentTasks[m_currentIndex + 1];
            }

            return new List<TaskNode>();
        }
    }

    //.Where((TaskNode task) => { return task.IsTransition(); })
    //.OrderBy(task => task.priority); //並び変え
    //activeTasks.ToList().Sort((TaskNode a, TaskNode b) => { return b.priority - a.priority; });
}
