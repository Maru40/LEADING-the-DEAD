using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class StateNode_ZombieNormal_Find : EnemyStateNodeBase<EnemyBase>
{
    [Serializable]
    public struct Parametor
    {
        public float maxFindWaitTime;

        public Parametor(float maxFindWaitTime)
        {
            this.maxFindWaitTime = maxFindWaitTime;
        }
    }

    enum TaskEnum
    {
        LookRotation,  //見たい方法を見る。
        SeeWait,
    }

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    Parametor m_param = new Parametor();

    Stator_ZombieNormal m_stator;

    public StateNode_ZombieNormal_Find(EnemyBase owner, Parametor param)
        :base(owner)
    {
        m_param = param;

        m_stator = owner.GetComponent<Stator_ZombieNormal>();

        DefineTask();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ThrongManager>(), false, true);
        AddChangeComp(owner.GetComponent<ChaseTarget>(), false, false);
    }

    public override void OnStart()
    {               
        base.OnStart();

        SelectTask();
    }

    public override void OnUpdate()
    {
        Debug.Log("FindState");

        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            ChangeState();  //ステートの切替
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    void DefineTask()
    {
        var enemy = GetOwner().GetComponent<EnemyBase>();

        m_taskList.DefineTask(TaskEnum.LookRotation, new Task_LookTargetRotation(enemy));
        m_taskList.DefineTask(TaskEnum.SeeWait, new Task_SeeWait(enemy, m_param.maxFindWaitTime));
    }

    void SelectTask()
    {
        m_taskList.AddTask(TaskEnum.LookRotation);
        m_taskList.AddTask(TaskEnum.SeeWait);
    }

    void ChangeState()
    {
        var owner = GetOwner();

        m_stator.GetTransitionMember().chaseTrigger.Fire();
    }

    //タスクの定義----------------------------------------------------------------

    /// <summary>
    /// ターゲットの方向を見ながら待機
    /// </summary>
    class Task_SeeWait : TaskNodeBase<EnemyBase>
    {
        float m_maxWatiTime;

        GameTimer m_timer = new GameTimer();

        EnemyRotationCtrl m_enemyRotationController;
        TargetManager m_targetManager;
        EnemyVelocityMgr m_velocityManager;

        public Task_SeeWait(EnemyBase owner, float maxWaitTime)
            :base(owner)
        {
            m_maxWatiTime = maxWaitTime;

            m_enemyRotationController = owner.GetComponent<EnemyRotationCtrl>();
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
        }

        public override void OnEnter()
        {
            var time = UnityEngine.Random.value * m_maxWatiTime;
            m_timer.ResetTimer(time);
            m_velocityManager.StartDeseleration();
        }

        public override bool OnUpdate()
        {
            m_timer.UpdateTimer();

            Rotation();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            m_velocityManager.SetIsDeseleration(false);
        }

        void Rotation()
        {
            var positionCheck = m_targetManager.GetToNowTargetVector();
            if(positionCheck == null) {
                return;
            }
            var toTargetVector = (Vector3)positionCheck;
            //var toVec = position - GetOwner().transform.position;
            //toVec.y = 0;

            //GetOwner().transform.forward = toTargetVector.normalized;
            m_enemyRotationController.SetDirect(toTargetVector);
        }
    }


    //ターゲットの方向を向く処理
    class Task_LookTargetRotation : TaskNodeBase<EnemyBase>
    {
        float m_speed = 2.0f;

        float m_saveRotationSpeed = 0.0f;
        bool m_saveRotationCompEnable = false;  //ローテーションの

        EnemyRotationCtrl m_rotationController;
        TargetManager m_targetManager;
        EnemyVelocityMgr m_velocityManager;

        public Task_LookTargetRotation(EnemyBase owner)
            :base(owner)
        {
            //m_speed = speed;
            m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
        }

        public override void OnEnter()
        {
            m_saveRotationSpeed = m_rotationController.GetSpeed();
            m_saveRotationCompEnable = m_rotationController.enabled;
            m_rotationController.enabled = true;
            m_rotationController.SetSpeed(m_speed);

            m_velocityManager.StartDeseleration();
        }

        public override bool OnUpdate()
        {
            Rotation();

            return IsEnd();
        }

        public override void OnExit()
        {
            m_rotationController.SetSpeed(m_saveRotationSpeed);
            m_rotationController.enabled = m_saveRotationCompEnable;
        }

        void Rotation()
        {
            var positionCheck = m_targetManager.GetToNowTargetVector();
            if(positionCheck == null) {
                return;
            }
            var toTargetVec = (Vector3)positionCheck;

            m_rotationController.SetDirect(toTargetVec);
        }

        bool IsEnd()
        {
            //方向とフォワードの差が少なかったら。
            var positionCheck = m_targetManager.GetToNowTargetVector();
            if (positionCheck == null){
                return true;
            }
            var toTargetVec = (Vector3)positionCheck;

            float dot = Vector3.Dot(toTargetVec.normalized, GetOwner().transform.forward);
            
            if(dot >= 0.9f)  //内積が0.9f以上ならそっちの方向を向いたことになる。
            {
                return true;
            }

            return false;
        }
    }
}
