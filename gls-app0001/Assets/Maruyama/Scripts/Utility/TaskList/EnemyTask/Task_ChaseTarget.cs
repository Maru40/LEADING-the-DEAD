using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class Task_ChaseTarget : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor 
    {
        public float maxSpeed;
        [Header("ターゲットとのフォワードの差がこの数字よりより小さければ、予測タックルにする。")]
        public float subPursuitTargetForward;
        public float nearRange;  //対象に追いついたと思う距離
        public float turningPower;  //曲がる力
        public System.Action enterAnimation;
    }

    Parametor m_param = new Parametor();

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    EnemyRotationCtrl m_rotationController;
    EyeSearchRange m_eye;

    public Task_ChaseTarget(EnemyBase owner, Parametor param)
        :base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
        m_rotationController = owner.GetComponent<EnemyRotationCtrl>();
        m_eye = owner.GetComponent<EyeSearchRange>();

        m_param = param;
    }

    public override void OnEnter()
    {
        m_param.enterAnimation?.Invoke();
    }

    public override bool OnUpdate()
    {
        Debug.Log("△追いかける");

        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return true;
        }

        Move();
        Rotation();

        return IsEnd;
    }

    public override void OnExit()
    {

    }

    void Move()
    {
        var owner = GetOwner();

        var target = m_targetManager.GetNowTarget();
        var toVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //相手と自分のフォワードの差(dot)
        var relativeHeading = Vector3.Dot(owner.transform.forward, target.transform.forward);

        //予測シークの条件を満たせば、予測シーク。そうでなければ通常追従
        //var force = CalcuVelocity.CalcuConditionPursuitForce(m_velocityManager.velocity, toVec, m_param.maxSpeed,
        //    owner.gameObject, m_velocityManager.GetComponent<Rigidbody>(),
        //    relativeHeading, m_param.subPursuitTargetForward, m_param.turningPower);

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

    void Rotation()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットがnullなら
            return;
        }

        var direct = (Vector3)m_targetManager.GetToNowTargetVector();
        m_rotationController.SetDirect(direct.normalized);
    }

    bool IsEnd
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
    bool IsFront(Vector3 toTargetVec)
    {
        return Vector3.Dot(toTargetVec, GetOwner().transform.forward) > 0 ? true : false;
    }

    /// <summary>
    /// 自分と相手のフォワードの差が開いていなかったら
    /// </summary>
    /// <param name="relativeHeading">自分と相手のフォワードのdot</param>
    /// <returns>差が開いていなかったらtrue</returns>
    bool IsMinSubForward(float relativeHeading, float sub)
    {
        return Mathf.Abs(relativeHeading) > sub ? true : false;
    }

    bool IsNearRange()
    {
        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //ターゲットが近くにいたら。
        return toTargetVec.magnitude < m_param.nearRange ? true : false;
    }

    //視界内かどうか
    bool IsEyeRad()
    {
        if (m_eye.IsInEyeRange(m_targetManager.GetNowTarget().gameObject))
        {
            return true;
        }

        return false;
    }
}
