using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TaskActionParametor = TaskNodeBase_Ex<EnemyBase>.ActionParametor;

public class WallAttack_ZombieNormal : AttackNodeBase
{
    [System.Serializable]
    public struct Parametor 
    {
        [Header("壁攻撃パラメータ")]
        public Task_WallAttack.Parametor wallAttackParam;
        [Header("待機パラメータ")]
        public Task_Wait.Parametor waitParam;

        public Parametor(Task_WallAttack.Parametor wallAttackParam, Task_Wait.Parametor waitParam)
        {
            this.wallAttackParam = wallAttackParam;
            this.waitParam = waitParam;
        }
    }

    private enum TaskEnum { 
        Attack,
        Wait
    }

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    [SerializeField]
    private Parametor m_param = new Parametor();

    private AnimatorManager_ZombieNormal m_animatorManager;
    private TargetManager m_targetManager;
    private EyeSearchRange m_eye;
    private AttackNodeManagerBase m_attackManager;
    private Stator_ZombieNormal m_stator;
    private EnemyVelocityManager m_velocityManager;

    private bool m_isPutAttack = false; //壁などに噛みつき攻撃をするかどうか

    private void Awake()
    {
        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
        m_targetManager = GetComponent<TargetManager>();
        m_eye = GetComponent<EyeSearchRange>();
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_velocityManager = GetComponent<EnemyVelocityManager>();
    }

    private void Start()
    {
        DefineTask();
        enabled = false;
    }

    private void Update()
    {
        m_taskList.UpdateTask();

        if (m_isPutAttack) //噛みつき攻撃をするかどうか
        {
            PutAttack();
        }
    }

    private void DefineTask()
    {
        var enemy = GetComponent<EnemyBase>();

        //m_param.wallAttackParam.enterAnimation = () => m_animatorManager.CrossFadeWallAttack();
        m_taskList.DefineTask(TaskEnum.Attack, 
            new Task_WallAttack(enemy, m_param.wallAttackParam,
                new TaskActionParametor(() => m_animatorManager.CrossFadeWallAttack(), null, null)));

        m_param.waitParam.exit = () => EndAnimationEvent();
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));
    }

    private void SelectTask()
    {
        TaskEnum[] types = {
            TaskEnum.Attack,
            TaskEnum.Wait,
        };

        foreach(var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        var targetPosition = m_targetManager.GetNowTargetPosition();
        if (targetPosition != null) {
            var position = (Vector3)targetPosition;
            position.y = transform.position.y;
            return m_eye.IsInEyeRange(position, range);
        }

        return false;
    }

    public override void AttackStart()
    {
        if (m_stator.GetNowStateType() == ZombieNormalState.Attack) {
            return;
        }

        SelectTask();
        m_stator.GetTransitionMember().attackTrigger.Fire();
        enabled = true;
    }

    public override void EndAnimationEvent()
    {
        enabled = false;
        m_attackManager.EndAnimationEvent();
    }

    //攻撃ヒット時に行いたい処理
    public void HitAction(Collider other)
    {
        if(other.gameObject == gameObject) {
            return;
        }

        if(other.gameObject.tag == "T_Wall")
        {
            m_isPutAttack = true;
            m_taskList.AbsoluteReset();

            //アニメーションの遷移
            m_animatorManager.CrossFadePutWallAttack();
        }
    }

    /// <summary>
    /// 噛みつき攻撃
    /// </summary>
    private void PutAttack()
    {
        if (m_stator.GetNowStateType() != ZombieNormalState.Attack)
        {
            m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.UpperLayerIndex);
            m_velocityManager.ResetAll();
            m_isPutAttack = false;
        }
    }

    //アクセッサ---------------------------------------------------------------------------------------

    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }
}
