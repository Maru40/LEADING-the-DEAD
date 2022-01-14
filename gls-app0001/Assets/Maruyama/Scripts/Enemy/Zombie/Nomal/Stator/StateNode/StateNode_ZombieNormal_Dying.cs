using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DyingTypeEnum = StatusManager_ZombieNormal.DyingTypeEnum;

public class StateNode_ZombieNormal_Dying : EnemyStateNodeBase<EnemyBase>
{
    private enum TaskEnum
    {
        Fire,
        RenderFadeOut,
        Cutting,
        PlayDeathAnimation,
    }

    [System.Serializable]
    public class Parametor
    {
        public float fireTime = 0.5f;  //炎に包まれながら動く時間
        public float deathAnimationTime = 1.5f;  //死亡アニメーション終了時間
        public AudioManager audioManager = null; //死亡時の声を上げるパラメータ

        public Parametor()
        { }

        public Parametor(float fireTime)
            :this(fireTime, 1.5f)
        { }

        public Parametor(float fireTime, float deathAnimationTime)
        {
            this.fireTime = fireTime;
            this.deathAnimationTime = deathAnimationTime;
        }

        public Parametor(Parametor param)
        {
            this.fireTime = param.fireTime;
        }
    }

    private Parametor m_param = new Parametor()
    {
        fireTime = 0.5f,
        deathAnimationTime = 1.5f,
    };

    private Dictionary<TaskEnum, GameTimer> m_timerDictionary = new Dictionary<TaskEnum, GameTimer>();

    private StatusManager_ZombieNormal m_statusManager;
    private Stator_ZombieNormal m_stator;
    private AnimatorManager_ZombieNormal m_animatorManager;
    private EnemyVelocityManager m_velocityManager;
    private TargetManager m_targetManager = null;

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    public StateNode_ZombieNormal_Dying(EnemyBase owner, Parametor param = null)
        :base(owner)
    {
        m_statusManager = owner.GetComponent<StatusManager_ZombieNormal>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_targetManager = owner.GetComponent<TargetManager>();

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
        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<KnockBackManager>(),false, true);
    }

    public override void OnStart()
    {
        base.OnStart();

        //上半身アニメーションレイヤーの削除
        m_animatorManager.Dying();
        m_targetManager.SetNowTarget(GetType(), null);

        //タスクのセレクト
        SelectTask();

        m_param.audioManager?.PlayRandomClipOneShot();  //声の再生。
    }

    public override void OnUpdate()
    {
        m_taskList.UpdateTask();
        //m_animatorManager.CrossFadeIdleAnimation("Upper Layer");

        if (m_taskList.IsEnd)  //タスクが終了したら。
        {
            //死亡状態に変更
            m_stator.GetTransitionMember().deathTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        m_animatorManager.Respawn();
    }

    /// <summary>
    /// タスク定義
    /// </summary>
    private void DefineTask()
    {
        m_taskList.DefineTask(TaskEnum.Fire, OnTaskFireEnter, OnTaskFireUpdate, OnTaskFireExit);
        m_taskList.DefineTask(TaskEnum.RenderFadeOut, new Task_RenderFadeOut(GetOwner()));
        m_taskList.DefineTask(TaskEnum.PlayDeathAnimation,
            OnTaskPlayDeathAnimationEnter, OnTaskPlayDeathAnimationUpdate, OnTaskPlayDeathAnimationExit);
        m_taskList.DefineTask(TaskEnum.Cutting, new Task_CuttilingDeath());  //切断死亡
    }

    /// <summary>
    /// タスクの選択
    /// </summary>
    private void SelectTask()
    {
        var dyingType = m_statusManager.DyingType;

        var types = dyingType switch
        {
            DyingTypeEnum.Fire => new TaskEnum[] { TaskEnum.Fire }, //炎に包まれた時
            DyingTypeEnum.Cutting => new TaskEnum[] { TaskEnum.Cutting, TaskEnum.PlayDeathAnimation, TaskEnum.RenderFadeOut},  //切断されたとき
            _ => new TaskEnum[] { }
        };

        foreach(var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    //フェードして消える-----------------------------------------------------------------------------------------

    private class Task_RenderFadeOut : TaskNodeBase<EnemyBase>
    {
        private List<RenderFadeManager> m_fadeManagers = new List<RenderFadeManager>();

        public Task_RenderFadeOut(EnemyBase owner)
            :base(owner)
        {
            m_fadeManagers = new List<RenderFadeManager>(GetOwner().GetComponentsInChildren<RenderFadeManager>());
        }

        public override void OnEnter()
        {
            m_fadeManagers = new List<RenderFadeManager>(GetOwner().GetComponentsInChildren<RenderFadeManager>());

            foreach (var fade in m_fadeManagers)
            {
                fade?.FadeStart();
            }
        }

        public override bool OnUpdate()
        {
            foreach (var fade in m_fadeManagers)
            {
                if (fade.IsEnd)
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnExit()
        {

        }
    }

    //DeathAnimatinの再生-----------------------------------------------------------------------------------------

    private void OnTaskPlayDeathAnimationEnter()
    {
        m_animatorManager.CrossFadeDeathAnimatiron();

        m_timerDictionary[TaskEnum.PlayDeathAnimation].ResetTimer(m_param.deathAnimationTime);
    }

    private bool OnTaskPlayDeathAnimationUpdate()
    {
        var timer = m_timerDictionary[TaskEnum.PlayDeathAnimation];
        timer.UpdateTimer();

        return timer.IsTimeUp;
    }

    private void OnTaskPlayDeathAnimationExit()
    {
        //m_animatorManager.CrossFadeIdleAnimation();
    }

    //ファイアー--------------------------------------------------------------------------------------

    private void OnTaskFireEnter()
    {
        m_timerDictionary[TaskEnum.Fire].ResetTimer(m_param.fireTime);  //Timer初期化

        m_velocityManager.StartDeseleration();  //減速開始
    }

    private bool OnTaskFireUpdate()
    {
        GameTimer timer;
        bool exist = m_timerDictionary.TryGetValue(TaskEnum.Fire,out timer);
        if (exist == false) {  //存在しないなら処理を飛ばす。
            return true;
        }

        timer.UpdateTimer();  //時間計測
        return timer.IsTimeUp;
    }

    private void OnTaskFireExit()
    {
        m_velocityManager.SetIsDeseleration(false);  //減速終了
    }

    //Cutting----------------------------------------------------------------

    private class Task_CuttilingDeath : TaskNodeBase
    {
        private float m_time = 0.1f;
        private GameTimer m_timer = new GameTimer();

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
