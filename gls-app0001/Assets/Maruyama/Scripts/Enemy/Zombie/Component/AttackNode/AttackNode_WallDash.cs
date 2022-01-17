using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode_WallDash : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("追いかけるパラメータ")]
        public Task_ChaseTarget.Parametor chaseParam;
        [Header("壁攻撃パラメータ")]
        public Task_WallAttack.Parametor wallAttackParam;
        [Header("待機パラメータ")]
        public Task_Wait.Parametor waitParam;
        [Header("壁噛みつきパラメータ")]
        public Task_PutWall.Parametor putWallParam;

        public Parametor(Task_ChaseTarget.Parametor chaseParam,
            Task_WallAttack.Parametor wallAttackParam,
            Task_Wait.Parametor waitParam,
            Task_PutWall.Parametor putWallParam)
        {
            this.chaseParam = chaseParam;
            this.wallAttackParam = wallAttackParam;
            this.waitParam = waitParam;
            this.putWallParam = putWallParam;
        }
    }

    private enum TaskEnum
    {
        Chase,
        Attack,
        Wait,
        PutWall,
    }

    private Parametor m_param = new Parametor();

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private AnimatorManager_ZombieNormal m_animatorManager;
    private CollisionAction m_collisionAction;

    public AttackNode_WallDash(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_collisionAction = owner.GetComponent<CollisionAction>();

        m_collisionAction.AddEnterAction(HitWall);

        DefineTask();
    }

    public override void OnEnter()
    {
        //base.OnStart();

        SelectTask();
    }

    public override bool OnUpdate()
    {
        m_taskList.UpdateTask();

        return m_taskList.IsEnd;
    }

    public override void OnExit()
    {
        //base.OnExit();

        m_taskList.AbsoluteReset();
        m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.UpperLayerIndex);
        //m_attackManager.EndAnimationEvent();
    }

    private void DefineTask()
    {
        var enemy = GetOwner();

        m_taskList.DefineTask(TaskEnum.Chase, new Task_ChaseTarget(enemy, m_param.chaseParam));

        m_param.wallAttackParam.enterAnimation = () => m_animatorManager.CrossFadeWallAttack();
        m_taskList.DefineTask(TaskEnum.Attack, new Task_WallAttack(enemy, m_param.wallAttackParam));

        //m_param.waitParam.exit = () => m_attackManager.EndAnimationEvent();
        m_taskList.DefineTask(TaskEnum.Wait, new Task_Wait(m_param.waitParam));

        m_taskList.DefineTask(TaskEnum.PutWall, new Task_PutWall(enemy, m_param.putWallParam));
    }

    private void SelectTask()
    {
        TaskEnum[] types = {
            TaskEnum.Chase,
            TaskEnum.Attack,
            TaskEnum.Wait,
        };

        foreach (var type in types)
        {
            m_taskList.AddTask(type);
        }
    }

    private void HitWall(Collision collision)
    {
        if (m_taskList.IsEnd) { 
            return; 
        }

        if (collision.gameObject == GetOwner().gameObject) {
            return;
        }

        if (collision.gameObject.tag == "T_Wall")
        {
            m_taskList.AbsoluteReset();
            m_taskList.AddTask(TaskEnum.PutWall);

            //アニメーションの遷移
            m_animatorManager.CrossFadePutWallAttack();
        }
    }
}
