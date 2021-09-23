using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using System;

/// <summary>
/// Random�ɜp�j����R���|�[�l���g
/// </summary>
public class RandomPlowlingMove : MonoBehaviour
{
    [Serializable]
    public struct Parametor 
    {
        public float randomPositionRadius;  //�p�j����ꏊ�����߂锼�a
        public float maxSpeed;              //�ő�X�s�[�h
        public float turningPower;          //���񂷂��
        public float targetNearRange;       //�ړI�n�ɒ������Ɣ��f�����,�ړI�n�Ƃ̑��΋���(����������Ɣ��f�ł��Ȃ��Ȃ邽�ߒ���)
        public float maxWaitCalcuRouteTime; //�ړI�n�ɂ����Ƃ��A�����~�܂�ő�̎���
        public float inThrongRange;         //�W�c�ƔF������͈�

        public Parametor(float randomPositionRadius, float maxSpeed, float turningPower,
            float targetNearRange, float maxWaitCalcuRouteTime, float inThrongRange)
        {
            this.randomPositionRadius = randomPositionRadius;
            this.maxSpeed = maxSpeed;
            this.turningPower = turningPower;
            this.targetNearRange = targetNearRange;
            this.maxWaitCalcuRouteTime = maxWaitCalcuRouteTime;
            this.inThrongRange = inThrongRange;
        }
    }


    //member�ϐ�-------------------------------------------------------------------------------------------

    [SerializeField]
    Parametor m_param = new Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);

    /// <summary>
    /// Ray�̏�Q������Layer�̔z��
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    Vector3 m_targetPosition;    //�ړI�̏ꏊ

    EnemyVelocityMgr m_velocityMgr;
    WaitTimer m_waitTimer;
    ThrongManager m_throngMgr;
    
    void Start()
    {
        //�R���|�[�l���g�̎擾
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_throngMgr = GetComponent<ThrongManager>();

        //�V�[�h�l
        Random.InitState(System.DateTime.Now.Millisecond);

        SetRandomTargetPosition();
    }
    
    void Update()
    {
        MoveProcess();
    }

    void MoveProcess()
    {
        //�ҋ@��ԂȂ珈�������Ȃ��B
        if (m_waitTimer.IsWait(GetType())){
            return;
        }

        //������͂̌v�Z
        var toVec = m_targetPosition - transform.position;
        m_throngMgr.AvoidNearThrong(m_velocityMgr, toVec, m_param.maxSpeed, m_param.turningPower);

        var newTargetPosition = m_throngMgr.CalcuRandomPlowlingMovePositonIntegrated(this);  //�����_���ȕ������W�c�ɍ��킹��B
        SetTargetPositon(newTargetPosition);

        //var newForce = UtilityVelocity.CalucArriveVec(velocity, toVec, m_speed);
        //m_rigid.AddForce(newForce);

        if (IsRouteEnd())
        {
            RouteEndProcess();
        }
    }

    /// <summary>
    /// ���[�g�̏I���𔻒f�B
    /// </summary>
    /// <returns>�ړI�n�ɂ�����true</returns>
    bool IsRouteEnd()
    {
        var toVec = m_targetPosition - transform.position;
        float range = toVec.magnitude;

        return range <= m_param.targetNearRange ? true : false;
    }

    /// <summary>
    /// �ړI�n�ɂ��ǂ蒅�������ɍs������
    /// </summary>
    void RouteEndProcess()
    {
        if (m_waitTimer.IsWait(GetType())){
            return;
        }

        SetRandomTargetPosition();
        m_velocityMgr.ResetVelocity();  //���x�̃��Z�b�g

        //�ҋ@��Ԃ̐ݒ�
        var waitTime = UnityEngine.Random.value * m_param.maxWaitCalcuRouteTime;
        m_waitTimer.AddWaitTimer(GetType(), waitTime);
    }

    /// <summary>
    /// �����_���ȖړI�n��ݒ�
    /// </summary>
    void SetRandomTargetPosition()
    {
        int numLoop = 100; //�������[�v��h������100�����x�ɂ���B
        for(int i = 0; i < numLoop; i++)
        {
            m_targetPosition = CalucRandomTargetPosition();

            var toVec = m_targetPosition - transform.position;
            int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
            if (!Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer)) {
                break;  //��Q�����Ȃ�������
            }
        }
    }

    /// <summary>
    /// �����_���ȕ������v�Z���ĕԂ��B
    /// </summary>
    /// <returns>�����_���ȕ���</returns>
    Vector3 CalucRandomTargetPosition()
    {
        float directX = CalucRandomDirect();
        float directZ = CalucRandomDirect();

        float x = Random.value * m_param.randomPositionRadius * directX;
        float y = transform.position.y;
        float z = Random.value * m_param.randomPositionRadius * directZ;

        var toVec = new Vector3(x,y,z);
        var newPosition = transform.position + toVec;

        return newPosition;
    }

    /// <summary>
    /// �����_����1��-1�ɂ��ĕԂ��B
    /// </summary>
    /// <returns>1,-1�̂ǂ��炩</returns>
    int CalucRandomDirect()
    {
        float halfValue = 0.5f;
        float random = Random.value;

        return random < halfValue ? 1 : -1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsObstract(collision.gameObject))
        {
            SetRandomTargetPosition();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsObstract(collision.gameObject))
        {
            SetRandomTargetPosition();
        }
    }

    bool IsObstract(GameObject gameObj)
    {
        foreach(var str in m_rayObstacleLayerStrings)
        {
            int obstacleLayer = LayerMask.NameToLayer(str);
            if(gameObj.layer == obstacleLayer) {
                return true;
            }
        }

        return false;
    }


    //�A�N�Z�b�T-----------------------------------------------------

    public void SetTargetPositon(Vector3 position)
    {
        m_targetPosition = position;
    }
    public Vector3 GetTargetPosition()
    {
        return m_targetPosition;
    }

    public void SetInThrongRange(float range)
    {
        m_param.inThrongRange = range;
    }
    public  float GetInThrongRange()
    {
        return m_param.inThrongRange;
    }

    public void SetParametor(Parametor parametor)
    {
        m_param = parametor;
    }

    //���ݎg�p���Ă��Ȃ�
    //bool IsRayHit(Vector3 position)
    //{
    //    RaycastHit hitData;
    //    var direction = position - transform.position;
    //    //var direction = transform.position - position;
    //    float range = direction.magnitude;

    //    if (Physics.Raycast(transform.position, direction, out hitData, range))
    //    //if (Physics.Raycast(position, direction, out hitData, range))
    //    {
    //        Debug.Log(transform.position - hitData.transform.position);
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

}
