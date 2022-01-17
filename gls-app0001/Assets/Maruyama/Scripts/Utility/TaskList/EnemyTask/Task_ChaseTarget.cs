using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Task_ChaseTarget : TaskNodeBase_Ex<EnemyBase>
{
    [System.Serializable]
    public struct Parametor 
    {
        public float maxSpeed;
        [Header("ターゲットとのフォワードの差がこの数字よりより小さければ、予測タックルにする。")]
        public float subPursuitTargetForward;
        public float nearRange;  //対象に追いついたと思う距離
        public float turningPower;  //曲がる力
        public float chaseRange; //追いかける距離
        //public System.Action enterAnimation;

        public Parametor(float maxSpeed, float subPursuitTargetForward,
            float nearRange, float turningPower, float chaseRange
            //System.Action action
            )
        {
            this.maxSpeed = maxSpeed;
            this.subPursuitTargetForward = subPursuitTargetForward;
            this.nearRange = nearRange;
            this.turningPower = turningPower;
            this.chaseRange = chaseRange;
            //this.enterAnimation = action;
        }
    }

    private Parametor m_param = new Parametor();

    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private EnemyRotationCtrl m_rotationController;
    private EyeSearchRange m_eye;

    public Task_ChaseTarget(EnemyBase owner, Parametor param)
        :this(owner, param, new BaseParametor())
    { }

    public Task_ChaseTarget(EnemyBase owner, Parametor param, BaseParametor baseParametor)
        : base(owner, baseParametor)
    {
        m_param = param;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_eye = owner.GetComponent<EyeSearchRange>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        //m_param.enterAnimation?.Invoke();
    }

    public override bool OnUpdate()
    {
        base.OnUpdate();

        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return true;
        }

        Move();
        Rotation();

        return IsEnd;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void Move()
    {
        var owner = GetOwner();

        var target = m_targetManager.GetNowTarget();
        var toVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //相手と自分のフォワードの差(dot)
        var relativeHeading = Vector3.Dot(owner.transform.forward, target.transform.forward);

        //前方にいて、かつ、自分と相手のフォワードの差が開いていなかったら、通常Seek
        //Dotは差が開いている程、値が小さくなる。
        var velocity = m_velocityManager.velocity;
        var force = Vector3.zero;
        if (IsFront(toVec) && IsMinSubForward(relativeHeading, m_param.subPursuitTargetForward))
        {
            force = CalcuVelocity.CalucSeekVec(velocity, toVec, m_param.maxSpeed);
        }
        else
        {   //フォワードの差が開いていたら、予測Seek
            Debug.Log("予測Seek");
            force = CalcuVelocity.CalcuPursuitForce(
                velocity, toVec, m_param.maxSpeed, GetOwner().gameObject, target.GetComponent<Rigidbody>(), m_param.turningPower);
        }

        m_velocityManager.AddForce(force);
    }

    private void Rotation()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return;
        }

        var direct = (Vector3)m_targetManager.GetToNowTargetVector();
        m_rotationController.SetDirect(direct.normalized);
    }

    private bool IsEnd
    {
        get
        {
            //正面から大きく外れたら 又は 近くにいたら
            if (!IsEyeRad() || IsNearRange())
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// ターゲットが正面にいるかどうか
    /// </summary>
    /// <param name="toTargetVec">ターゲットの方向</param>
    /// <returns>正面ならtrue</returns>
    private bool IsFront(Vector3 toTargetVec)
    {
        return Vector3.Dot(toTargetVec, GetOwner().transform.forward) > 0 ? true : false;
    }

    /// <summary>
    /// 自分と相手のフォワードの差が開いていなかったら
    /// </summary>
    /// <param name="relativeHeading">自分と相手のフォワードのdot</param>
    /// <returns>差が開いていなかったらtrue</returns>
    private bool IsMinSubForward(float relativeHeading, float sub)
    {
        return Mathf.Abs(relativeHeading) > sub ? true : false;
    }

    private bool IsNearRange()
    {
        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //ターゲットが近くにいたら。
        return toTargetVec.magnitude < m_param.nearRange ? true : false;
    }

    /// <summary>
    /// 視界内かどうか
    /// </summary>
    /// <returns></returns>
    private bool IsEyeRad()
    {
        if (m_eye.IsInEyeRange(m_targetManager.GetNowTarget().gameObject, m_param.chaseRange))
        {
            return true;
        }

        return false;
    }
}
