using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class DashAttack : AttackNodeBase
{
    enum TaskEnum {
        Preliminary,
        Chase,
        Attack,
        Wait,
    }

    [System.Serializable]
    struct Parametor
    {
        [Header("予備動作パラメータ")]
        public PreliminaryParametor preliminaryParam;
        [Header("追うパラメータ")]
        public Task_ChaseTarget.Parametor chaseParam;
        [Header("攻撃パラメータ")]
        public Task_WallAttack.Parametor attackParam;
        [Header("待機状態パラメータ")]
        public Task_Wait.Parametor waitParam;
        [Header("行動確率")]
        public float probability;
        [Header("行動始める距離")]
        public float startRange;
        [Header("確率計算インターバル")]
        public float probabilityInterbalTime;
    }

    [SerializeField]
    Parametor m_param = new Parametor();

    TargetManager m_targetManager;
    EyeSearchRange m_eye;
    AttackNodeManagerBase m_attackManager;
    AnimatorManager_ZombieNormal m_animatorManager;
    EnemyVelocityMgr m_velocityManager;
    Stator_ZombieNormal m_stator;

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();
    GameTimer m_timer = new GameTimer();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_eye = GetComponent<EyeSearchRange>();
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_stator = GetComponent<Stator_ZombieNormal>();
    }

    private void Start()
    {
        DefineTask();
        m_timer.ResetTimer(m_param.probabilityInterbalTime);
    }

    void Update()
    {
        UpdateTask();

        if (m_stator.GetNowStateType() == ZombieNormalState.Attack) {
            return;
        }

        if (m_timer.UpdateTimer())
        {
            m_timer.ResetTimer(m_param.probabilityInterbalTime);
            ProbabilityAttack();
        }
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    void DefineTask()
    {
        var enemy = GetComponent<EnemyBase>();

        m_param.preliminaryParam.enterAnimation = () => m_animatorManager.CrossFadePreliminaryNormalAttackAniamtion();
        m_taskList.DefineTask(TaskEnum.Preliminary, new Task_Preliminary(enemy, m_param.preliminaryParam));

        m_param.chaseParam.enterAnimation = () => m_animatorManager.CrossFadeDashAttackMove();
        m_taskList.DefineTask(TaskEnum.Chase, new Task_ChaseTarget(enemy, m_param.chaseParam));

        m_param.attackParam.enterAnimation = () => m_animatorManager.CrossFadeDashAttack();
        m_taskList.DefineTask(TaskEnum.Attack, new Task_WallAttack(enemy, m_param.attackParam));

        m_param.waitParam.enter = () => m_velocityManager.StartDeseleration();
        m_param.waitParam.exit = () => { 
            m_velocityManager.SetIsDeseleration(false);
            EndAnimationEvent();
        };
        //m_param.waitParam.enter = () => m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.UpperLayerIndex);
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));
    }

    void SelectTask()
    {
        TaskEnum[] types = { 
            TaskEnum.Preliminary, //予備動作
            TaskEnum.Chase,  //追いかける
            TaskEnum.Attack, //攻撃
            TaskEnum.Wait,   //待機
        }; 

        foreach(var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    void UpdateTask()
    {
        m_taskList.UpdateTask();

        if (!m_taskList.IsMoveTask) {  //タスクが動いていないなら動かさない。
            return;
        }
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        var position = m_targetManager.GetNowTargetPosition();
        if (position != null) {
            return m_eye.IsInEyeRange((Vector3)position, range);
        }

        return false;
    }

    public override void AttackStart()
    {
        SelectTask();
        m_stator.GetTransitionMember().attackTrigger.Fire();
    }

    public override void EndAnimationEvent()
    {
        m_timer.ResetTimer(m_param.probabilityInterbalTime);
        m_attackManager.EndAnimationEvent();
    }

    void ProbabilityAttack()
    {
        Debug.Log("◆◆Probability");

        if (IsAttackStart())
        {
            AttackStart();
        }
    }

    bool IsAttackStart()
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        if(m_targetManager.GetNowTargetType() != FoundObject.FoundType.Player) { //Playerでなかったら攻撃をしない。
            return false;
        }

        bool isProbability = MyRandom.RandomProbability(m_param.probability);

        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //確率内で、近くにいるとき
        if(isProbability && m_param.startRange > toTargetVec.magnitude)
        {
            return true;
        }

        return false;
    }
}
