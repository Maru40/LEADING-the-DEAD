using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAttack_ZombieNormal : AttackNodeBase
{
    [System.Serializable]
    struct Parametor 
    {
        public Task_WallAttack.Parametor wallAttackParam;
        public Task_Wait.Parametor waitParam;
    }

    enum TaskEnum { 
        Attack,
        Wait
    }

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    [SerializeField]
    Parametor m_param = new Parametor();

    AnimatorManager_ZombieNormal m_animatorManager;
    TargetManager m_targetManager;
    EyeSearchRange m_eye;
    AttackNodeManagerBase m_attackManager;
    Stator_ZombieNormal m_stator;
    EnemyVelocityManager m_velocityManager;

    bool m_isPutAttack = false;

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

        if (m_isPutAttack)
        {
            PutAttackCheck();
        }
    }

    void DefineTask()
    {
        var enemy = GetComponent<EnemyBase>();

        m_param.wallAttackParam.enterAnimation = () => m_animatorManager.CrossFadeWallAttack();
        m_taskList.DefineTask(TaskEnum.Attack, new Task_WallAttack(enemy, m_param.wallAttackParam));

        m_param.waitParam.exit = () => EndAnimationEvent();
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));
    }

    void SelectTask()
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

    private void PutAttackCheck()
    {
        if (m_stator.GetNowStateType() != ZombieNormalState.Attack)
        {
            m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.UpperLayerIndex);
            m_velocityManager.ResetAll();
            m_isPutAttack = false;
        }
    }
}
