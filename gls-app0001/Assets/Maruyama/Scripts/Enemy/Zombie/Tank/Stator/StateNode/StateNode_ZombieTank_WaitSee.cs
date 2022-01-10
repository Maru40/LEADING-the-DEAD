using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

/// <summary>
/// 様子見ステート
/// </summary>
public class StateNode_ZombieTank_WaitSee : EnemyStateNodeBase<EnemyBase>
{
    private struct TaskParametor
    {
        public Task_HorizontalMove.Parametor horizontalMoveParam;   
    }

    private enum TaskEnum
    {
        HorizontalMove,
        CircleMove,
    }

    private TaskParametor m_taskParam = new TaskParametor()
    {
        horizontalMoveParam = new Task_HorizontalMove.Parametor()
        {
            timeRange = new RandomRange(2.0f, 3.0f),
            speed = 1.0f,
        },
    };
    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private AttackNodeManagerBase m_attackManager;
    
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
            //まだ様子を見るか、攻撃するか選ぶ
            m_attackManager.AttackStart();
        }
    }

    private void DifineTask()
    {
        m_taskList.DefineTask(TaskEnum.HorizontalMove,
            new Task_HorizontalMove(
                GetOwner(), 
                m_taskParam.horizontalMoveParam
            ));
    }

    private void SelectTask()
    {
        m_taskList.AddTask(TaskEnum.HorizontalMove);
    }

    //タスク一覧----------------------------------------------------------------------------

    //HorizontalMove--------------------------

    private class Task_HorizontalMove : TaskNodeBase<EnemyBase>
    {
        public struct Parametor
        {
            public RandomRange timeRange;
            public float speed;

            public float rotationDegree;  //Vector3を回転させる角度

            public Parametor(RandomRange timeRange, float speed)
            {
                this.timeRange = timeRange;
                this.speed = speed;

                this.rotationDegree = 0.0f;
            }
        }

        private Parametor m_param = new Parametor();
        private GameTimer m_timer = new GameTimer();

        //コンポーネント系----------------------------------------------

        private EnemyVelocityManager m_velocityManager;
        private TargetManager m_targetManager;

        public Task_HorizontalMove(EnemyBase owner)
            :this(owner, new Parametor())
        { }

        public Task_HorizontalMove(EnemyBase owner, Parametor param)
            :base(owner)
        {
            m_param = param;

            m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override void OnEnter()
        {
            var timeRange = m_param.timeRange;
            float time = Random.Range(timeRange.min, timeRange.max);
            m_timer.ResetTimer(time);

            m_param.rotationDegree = CalcuRotationDegree();
            m_velocityManager.ResetAll();
        }

        public override bool OnUpdate()
        {
            Debug.Log("Task_HorizontalMove");

            m_timer.UpdateTimer();

            MoveUpdate();
            RotationUpdate();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            m_velocityManager.ResetAll();
        }

        private void MoveUpdate()
        {
            var direct = Quaternion.AngleAxis(m_param.rotationDegree, Vector3.up) * CalcuToTargetVec();
            Vector3 moveVec = direct.normalized * m_param.speed;

            m_velocityManager.velocity = moveVec;
        }

        private void RotationUpdate()
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
        private float CalcuRotationDegree()
        {
            float[] directs = { +90.0f, -90.0f };
            int index = Random.Range(0, directs.Length);
            return directs[index];
        }

        /// <summary>
        /// ターゲット方向のベクトルを返す
        /// </summary>
        /// <returns></returns>
        private Vector3 CalcuToTargetVec()
        {
            var target = m_targetManager.GetNowTarget();
            if (target == null) {
                return Vector3.zero;
            }

            var toTargetVec = target.transform.position - Owner.transform.position;
            toTargetVec.y = 0.0f;
            return toTargetVec;
        }
    }

    //サークルMove----------------------------

    //使ってない
    private class Task_CircleMove : TaskNodeBase<EnemyBase>
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

        private Parametor m_param = new Parametor();
        private GameTimer m_timer = new GameTimer();

        //コンポ―ネント系-------------------------------------------------------------------

        private EnemyVelocityManager m_velcoityManager;
        private TargetManager m_targetManager;

        public Task_CircleMove(EnemyBase owner)
            :this(owner, new Parametor())
        { }

        public Task_CircleMove(EnemyBase owner, Parametor param)
            : base(owner)
        {
            m_param = param;

            m_velcoityManager = owner.GetComponent<EnemyVelocityManager>();
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
            m_timer.UpdateTimer();   //タイマーカウント

            MoveUpdate();  //移動
            RotationUpdate(); //回転

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            m_param.elapsedMove = 0.0f;
        }

        private void TimerReset()
        {
            var timeRange = m_param.timeRange;
            float time = Random.Range(timeRange.min, timeRange.max);
            m_timer.ResetTimer(time);
        }

        private void MoveUpdate()
        {
            var moveVec = CalcuMoveVec();

            m_velcoityManager.velocity = moveVec.normalized * m_param.speed;
        }

        private void RotationUpdate()
        {
            var target = m_targetManager.GetNowTarget();
            if (target)
            {
                var toTarget = target.transform.position - Owner.transform.position;
                toTarget.y = 0.0f;
                Owner.transform.forward = toTarget.normalized;
            }
        }

        private Vector3 CalcuMoveVec()
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
        private int CalcuRandomMoveDirectInt()
        {
            const int RIGHT = +1;
            const int LEFT  = -1;
            
            int[] directs = { RIGHT, LEFT };
            int index = Random.Range(0, directs.Length);
            return directs[index];
        }
    }
}
