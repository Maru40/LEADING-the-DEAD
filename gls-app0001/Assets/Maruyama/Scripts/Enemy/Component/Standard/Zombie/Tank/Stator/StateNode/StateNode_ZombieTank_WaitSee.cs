using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

/// <summary>
/// 様子見ステート
/// </summary>
public class StateNode_ZombieTank_WaitSee : EnemyStateNodeBase<EnemyBase>
{
    enum TaskEnum
    {
        HorizontalMove,
        CircleMove,
    }

    TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    AttackNodeManagerBase m_attackManager;
    
    public StateNode_ZombieTank_WaitSee(EnemyBase owner)
        :base(owner)
    {
        m_attackManager = owner.GetComponent<AttackNodeManagerBase>();

        DifineTask();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ChaseTarget>(), false, true);
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        SelectTask();
    }

    public override void OnUpdate()
    {
        Debug.Log("様子見中");

        m_taskList.UpdateTask();

        if (m_taskList.IsEnd)
        {
            Debug.Log("WaitSee終わり");
            //まだ様子を見るか、攻撃するか選ぶ
            m_attackManager.AttackStart();
        }
    }

    void DifineTask()
    {
        m_taskList.DefineTask(TaskEnum.HorizontalMove,
            new Task_HorizontalMove(
                GetOwner(), 
                new Task_HorizontalMove.Parametor(new RandomRange(2.0f, 3.0f), 1.0f))
            );

        m_taskList.DefineTask(TaskEnum.CircleMove, 
            new Task_CircleMove(
                GetOwner(),
                new Task_CircleMove.Parametor(new RandomRange(30.0f,30.0f), 3.0f))
            );
    }

    void SelectTask()
    {
        m_taskList.AddTask(TaskEnum.HorizontalMove);
    }

    //タスク一覧----------------------------------------------------------------------------

    //HorizontalMove--------------------------

    class Task_HorizontalMove : TaskNodeBase<EnemyBase>
    {
        public struct Parametor
        {
            public RandomRange timeRange;
            public float speed;

            public Vector3 direct;

            public Parametor(RandomRange timeRange, float speed)
            {
                this.timeRange = timeRange;
                this.speed = speed;

                this.direct = Vector3.zero;
            }
        }

        Parametor m_param = new Parametor();
        GameTimer m_timer = new GameTimer();

        //コンポーネント系----------------------------------------------

        EnemyVelocityMgr m_velocityManager;
        TargetManager m_targetManager;

        public Task_HorizontalMove(EnemyBase owner)
            :this(owner, new Parametor())
        { }

        public Task_HorizontalMove(EnemyBase owner, Parametor param)
            :base(owner)
        {
            m_param = param;

            m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override void OnEnter()
        {
            var timeRange = m_param.timeRange;
            float time = Random.Range(timeRange.min, timeRange.max);
            m_timer.ResetTimer(time);

            m_param.direct = CalcuRandomMoveDirectVector();
            m_velocityManager.ResetAll();
        }

        public override bool OnUpdate()
        {
            Debug.Log("Task_HorizontalMove");

            m_timer.UpdateTimer();

            //MoveUpdate();
            RotationUpdate();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            m_velocityManager.ResetAll();
        }

        void MoveUpdate()
        {
            Vector3 moveVec = m_param.direct * m_param.speed;

            m_velocityManager.velocity = moveVec;
        }

        void RotationUpdate()
        {
            var target = m_targetManager.GetNowTarget();
            if (target)
            {
                var toTarget = target.transform.position - Owner.transform.position;
                toTarget.y = 0.0f;
                Owner.transform.forward = toTarget.normalized;
            }
        }

        /// <summary>
        /// 移動方向を返す(右か左)
        /// </summary>
        /// <returns></returns>
        Vector3 CalcuRandomMoveDirectVector()
        {
            Vector3[] directs = { Vector3.right, Vector3.left };
            int index = Random.Range(0, directs.Length);
            return directs[index];
        }
    }

    //サークルMove----------------------------

    class Task_CircleMove : TaskNodeBase<EnemyBase>
    {
        public struct Parametor
        {
            public RandomRange timeRange;  //ランダムなタイマーレンジ
            public float speed;

            public float direct;
            public float elapsedMove;
            public float toTargetLength; 

            public Parametor(RandomRange timeRange, float speed)
            {
                this.timeRange = timeRange;
                this.speed = speed;

                this.direct = 0.0f;
                this.elapsedMove = 0.0f;
                this.toTargetLength = 0.0f;
            }
        }

        Parametor m_param = new Parametor();
        GameTimer m_timer = new GameTimer();

        //コンポ―ネント系-------------------------------------------------------------------

        EnemyVelocityMgr m_velcoityManager;
        TargetManager m_targetManager;

        public Task_CircleMove(EnemyBase owner)
            :this(owner, new Parametor())
        { }

        public Task_CircleMove(EnemyBase owner, Parametor param)
            : base(owner)
        {
            m_param = param;

            m_velcoityManager = owner.GetComponent<EnemyVelocityMgr>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override void OnEnter()
        {
            TimerReset();  //タイマーリセット
            m_velcoityManager.ResetAll();

            m_param.direct = CalcuRandomMoveDirectInt();  //移動する方向を決める。
        }

        public override bool OnUpdate()
        {
            Debug.Log("Task_WaitSee");

            m_timer.UpdateTimer();   //タイマーカウント

            MoveUpdate();  //移動
            RotationUpdate(); //回転

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            m_param.elapsedMove = 0.0f;
        }

        void TimerReset()
        {
            var timeRange = m_param.timeRange;
            float time = Random.Range(timeRange.min, timeRange.max);
            m_timer.ResetTimer(time);
        }

        void MoveUpdate()
        {
            var moveVec = CalcuMoveVec();

            m_velcoityManager.velocity = moveVec.normalized * m_param.speed;
        }

        void RotationUpdate()
        {
            var target = m_targetManager.GetNowTarget();
            if (target)
            {
                var toTarget = target.transform.position - Owner.transform.position;
                toTarget.y = 0.0f;
                Owner.transform.forward = toTarget.normalized;
            }
        }

        Vector3 CalcuMoveVec()
        {
            m_param.elapsedMove += Time.deltaTime * m_param.direct;
            var elapsed = m_param.elapsedMove;

            var x = Mathf.Cos(elapsed);
            var z = Mathf.Sin(elapsed);
            return new Vector3(x, 0, z);
        }

        /// <summary>
        /// 移動方向を返す(右か左)
        /// </summary>
        /// <returns></returns>
        int CalcuRandomMoveDirectInt()
        {
            const int RIGHT = +1;
            const int LEFT  = -1;
            
            int[] directs = { RIGHT, LEFT };
            int index = Random.Range(0, directs.Length);
            return directs[index];
        }
    }
}
