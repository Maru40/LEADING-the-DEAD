using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Random�ɜp�j����R���|�[�l���g
/// </summary>
public class RandomPlowlingMove : MonoBehaviour
{
    //�p�j����ꏊ�����߂锼�a
    [SerializeField]
    float m_randomPositionRadius = 15.0f;

    //�ړ��X�s�[�h
    [SerializeField]
    float m_speed = 3.0f;                                             

    //�ړI�n�ɒ������Ɣ��f�����,�ړI�n�Ƃ̑��΋���
    //����������Ɣ��f�ł��Ȃ��Ȃ邽�ߒ���
    [SerializeField]
    float m_targetNearRange = 0.3f;

    //�ړI�n�ɂ��ǂ蒅�����Ƃ��̃X�s�[�h
    //[SerializeField]
    //float m_arrivalSpeed = 0.3f;

    //�ړI�n�ɂ����Ƃ��A�����~�܂�ő�̎���
    [SerializeField]
    float m_maxWaitCalucRouteTime = 3.0f;

    //�ړI�̏ꏊ
    Vector3 m_targetPosition;

    Rigidbody m_ridgid;
    WaitTimer m_waitTimer;
    
    void Start()
    {
        //�R���|�[�l���g�̎擾
        m_ridgid = GetComponent<Rigidbody>();
        m_waitTimer = GetComponent<WaitTimer>();

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
        var velocity = m_ridgid.velocity;
        var toVec = m_targetPosition - transform.position;

        var newForce = UtilityVelocity.CalucArriveVec(velocity, toVec, m_speed);
        m_ridgid.AddForce(newForce);

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

        return range <= m_targetNearRange ? true : false;
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
        m_ridgid.velocity = Vector3.zero;  //���x�̃��Z�b�g

        //�ҋ@��Ԃ̐ݒ�
        var waitTime = Random.value * m_maxWaitCalucRouteTime;
        m_waitTimer.AddWaitTimer(GetType(), waitTime);
    }

    /// <summary>
    /// �����_���ȖړI�n��ݒ�
    /// </summary>
    void SetRandomTargetPosition()
    {
        m_targetPosition = CalucRandomTargetPosition();
    }

    /// <summary>
    /// �����_���ȕ������v�Z���ĕԂ��B
    /// </summary>
    /// <returns>�����_���ȕ���</returns>
    Vector3 CalucRandomTargetPosition()
    {
        float directX = CalucRandomDirect();
        float directZ = CalucRandomDirect();

        float x = Random.value * m_randomPositionRadius * directX;
        float y = transform.position.y;
        float z = Random.value * m_randomPositionRadius * directZ;

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
        SetRandomTargetPosition();
    }

    private void OnCollisionStay(Collision collision)
    {
        SetRandomTargetPosition();
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
