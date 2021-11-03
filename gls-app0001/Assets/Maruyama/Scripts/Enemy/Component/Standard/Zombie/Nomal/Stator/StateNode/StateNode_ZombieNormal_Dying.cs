using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DyingTypeEnum = StatusManager_ZombieNormal.DyingTypeEnum;

public class StateNode_ZombieNormal_Dying : EnemyStateNodeBase<EnemyBase>
{
    enum TaskEnum
    {
        Fire,
        Cutting,
        PlayDeathAnimation,
    }

    public class Parametor
    {
        public float fireTime = 0.0f;  //炎に包まれながら動く時間
        public float deathAnimationTime;  //死亡アニメーション終了時間

        public Parametor()
        { }

        public Parametor(float fireTime)
        {
            this.fireTime = fireTime;
        }

        public Parametor(Parametor param)
        {
            this.fireTime = param.fireTime;
        }
    }

    Parametor m_param = new Parametor()
    {
        fireTime = 0.5f,
        deathAnimationTime = 1.5f,
    };  

    Dictionary<TaskEnum, GameTimer> m_timerDictionary = new Dictionary<TaskEnum, GameTimer>();

    StatusManager_ZombieNormal m_statusManager;
    Stator_ZombieNormal m_stator;
    AnimatorManager_ZombieNormal m_animatorManager;
    EnemyVelocityMgr m_velocityManager;

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    public StateNode_ZombieNormal_Dying(EnemyBase owner, Parametor param = null)
        :base(owner)
    {
        m_statusManager = owner.GetComponent<StatusManager_ZombieNormal>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();

        //タイマーの初期化
        m_timerDictionary[TaskEnum.Fire] = new GameTimer(m_param.fireTime);
        m_timerDictionary[TaskEnum.PlayDeathAnimation] = new GameTimer(m_param.deathAnimationTime);

        if (param != null) {  //渡されたパラメータがnull出なかったら値を更新
            m_param = param;
        }

        //タスクの定義
        DefineTask();
    }

    protected override void ReserveChangeComponents()
    {
        //特になし
    }

    public override void OnStart()
    {
        base.OnStart();

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
        m_taskList.DefineTask(TaskEnum.PlayDeathAnimation,
            OnTaskPlayDeathAnimationEnter, OnTaskPlayDeathAnimationUpdate, OnTaskPlayDeathAnimationExit);
        m_taskList.DefineTask(TaskEnum.Cutting, new Task_CuttilingDeath());  //切断死亡
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
            DyingTypeEnum.Cutting => new TaskEnum[] { TaskEnum.Cutting, TaskEnum.PlayDeathAnimation },  //切断されたとき
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

    //DeathAnimatinの再生-----------------------------------------------------------------------------------------

    void OnTaskPlayDeathAnimationEnter()
    {
        m_animatorManager.CrossFadeDeathAnimatiron();

        m_timerDictionary[TaskEnum.PlayDeathAnimation].ResetTimer(m_param.deathAnimationTime);
    }

    bool OnTaskPlayDeathAnimationUpdate()
    {
        var timer = m_timerDictionary[TaskEnum.PlayDeathAnimation];
        timer.UpdateTimer();

        return timer.IsTimeUp;
    }

    void OnTaskPlayDeathAnimationExit()
    {
        m_animatorManager.CrossFadeIdleAnimation();
    }

    //ファイアー--------------------------------------------------------------------------------------

    void OnTaskFireEnter()
    {
        m_timerDictionary[TaskEnum.Fire].ResetTimer(m_param.fireTime);  //Timer初期化

        m_velocityManager.StartDeseleration();  //減速開始
    }

    bool OnTaskFireUpdate()
    {
        GameTimer timer;
        bool exist = m_timerDictionary.TryGetValue(TaskEnum.Fire,out timer);
        if (exist == false) {  //存在しないなら処理を飛ばす。
            return true;
        }

        timer.UpdateTimer();  //時間計測
        return timer.IsTimeUp;
    }

    void OnTaskFireExit()
    {
        m_velocityManager.SetIsDeseleration(false);  //減速終了
    }

    //Cutting----------------------------------------------------------------

    class Task_CuttilingDeath : TaskNodeBase
    {
        float m_time = 0.1f;
        GameTimer m_timer = new GameTimer();

        public Task_CuttilingDeath()
            :this(0.1f)
        { }
        public Task_CuttilingDeath(float time)
        {
            m_time = time;
        }

        public override void OnEnter()
        {
            m_timer.ResetTimer(m_time);

            //血しぶきを上げる

            //肉体が出る
        }

        public override bool OnUpdate()
        {
            m_timer.UpdateTimer();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        { }
    }

    //アクセッサ-------------------------------------------------------------------

    public Parametor Param
    {
        get { return new Parametor(m_param); }
        set { m_param = value; }
    }
}
