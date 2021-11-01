﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

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
    }

    /// <summary>
    /// 定義されたタスク
    /// </summary>
    Dictionary<EnumType, Task> m_defineTaskDictionary = new Dictionary<EnumType, Task>();

    /// <summary>
    /// 現在積まれているタスク
    /// </summary>
    List<Task> m_currentTasks = new List<Task>();
    /// <summary>
    /// 現在動作しているタスク
    /// </summary>
    Task m_currentTask = null;
    /// <summary>
    /// 現在動作しているタスクのIndex
    /// </summary>
    int m_currentIndex = 0;

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
        if (isEndOneTask)
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
    public int CurentIndex => m_currentIndex;
}
