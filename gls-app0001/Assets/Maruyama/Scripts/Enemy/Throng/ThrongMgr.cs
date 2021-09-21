using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
struct ThrongMgrParametor  //�Q�OMgr�̃p�����[�^
{
    public float inThrongRange;   //�Q�O�Ɣ��f����͈�
    public float outThrongRange;  //�Q�O����O�ꂽ�Ɣ��f���鋗��

    public float nearObjectRange; //�אl�Ɣ��f����鋗��
}

//���̌Q�O���g�����߂̃f�[�^
[Serializable]
public struct ThrongData
{
    public GameObject gameObject;
    public EnemyVelocityMgr velocityMgr;
    public TargetMgr targetMgr;  //�^�[�Q�b�g�Ǘ�
    public ThrongMgr throngMgr;  //�Q�O�Ǘ�
    public RandomPlowlingMove randomPlowlingMove;

    public ThrongData(EnemyVelocityMgr velocityMgr, TargetMgr targetMgr, ThrongMgr throngMgr,
        RandomPlowlingMove randomPlowlingMove)
    {
        this.gameObject = targetMgr.gameObject;
        this.velocityMgr = velocityMgr;
        this.targetMgr = targetMgr;
        this.throngMgr = throngMgr;
        this.randomPlowlingMove = randomPlowlingMove;
    }
}


/// <summary>
/// �Q�O�s���}�l�[�W���\
/// </summary>
public class ThrongMgr : MonoBehaviour
{

    [SerializeField]
    ThrongMgrParametor m_param = new ThrongMgrParametor();

    //List<ThrongData> m_throngDatas = new List<ThrongData>();  //�O���[�v�̃I�u�W�F�N�g�ꗗ

    //�R���|�[�l���g�n-----------------

    [SerializeField]
    EnemyGenerator m_generator = null;

    Rigidbody m_rigid;

    void Start()
    {
        SetSearchGenerator();
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }
    
    //�Q�O�̒��S�̃x�N�g��
    Vector3 CalcuCenterVector(ThrongData data)
    {
        return data.gameObject.transform.position;
    }

    //data�̐i�s����
    Vector3 CalcuDirectVector(ThrongData data)
    {
        return data.velocityMgr.velocity;
    }

    //�I�u�W�F�N�g���m�̉��
    Vector3 CalcuAvoidVector(ThrongData data)
    {
        //���肩�玩�����g�Ɍ������x�N�g��
        var toSelfVec = transform.position - data.gameObject.transform.position;

        if(toSelfVec.magnitude < m_param.nearObjectRange) {  //�אl�Ȃ�
            return toSelfVec;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// �߂��̏W�c��������鏈��
    /// </summary>
    /// <param name="rigid">���g�̃��W�b�h�{�f�B</param>
    /// <param name="moveDirect">���̃I�u�W�F�N�g����������������</param>
    /// /// <param name="maxSpeed">�ő�X�s�[�h</param>
    public void AvoidNearThrong(EnemyVelocityMgr velcoityMgr, Vector3 moveDirect, float maxSpeed)
    {
        var velocity = velcoityMgr.velocity;

        moveDirect += CalcuThrongVector();
        Vector3 force = UtilityVelocity.CalucSeekVec(velocity, moveDirect, maxSpeed);
        velcoityMgr.AddForce(force);

        var avoidVec = CalcuSumAvoidVector();
        if (avoidVec != Vector3.zero) //������K�v�Ȃ�
        {
            Vector3 avoidForce = UtilityVelocity.CalucSeekVec(velocity, CalcuSumAvoidVector(), CalcuAverageSpeed());
            velcoityMgr.AddForce(avoidForce);
        }
    }

    /// <summary>
    /// �W�c�ړ������鏈��(�܂�������)
    /// </summary>
    /// <param name="selfRigid">�������g�̃��W�b�h�{�f�B</param>
    public void ThrongMove(EnemyVelocityMgr velcoityMgr, Vector3 moveDirect, float maxSpeed)
    {
        var throngVec = CalcuThrongVector();
        if(throngVec == Vector3.zero)
        {
            return;
        }

        var force = UtilityVelocity.CalucSeekVec(velcoityMgr.velocity, throngVec, CalcuAverageSpeed());
        velcoityMgr.AddForce(force);
    }

    /// <summary>
    /// �����_���Ɉړ�����]���r���W�c�ɍ��킹�������ɓ�������B
    /// </summary>
    /// <param name="plowlingMove">�R���|�\�l���g���̂���</param>
    public Vector3 CalcuRandomPlowlingMovePositonIntegrated(RandomPlowlingMove plowlingMove)
    {
        var throngDatas = m_generator.GetThrongDatas();

        int throngSize = 0;
        Vector3 sumPosition = Vector3.zero;
        foreach (var data in throngDatas)
        {
            if(IsRange(data, m_param.inThrongRange))
            {
                sumPosition += data.randomPlowlingMove.GetTargetPosition();
                throngSize++;
            }
        }

        return sumPosition / throngSize;
    }

    /// <summary>
    /// �Q�O�s���x�N�g���̌v�Z(�܂�������)
    /// </summary>
    /// <returns></returns>
    Vector3 CalcuThrongVector()
    {
        var throngDatas = m_generator.GetThrongDatas();

        Vector3 centerPosition = Vector3.zero;
        Vector3 avoidVec = Vector3.zero;
        Vector3 directVec = Vector3.zero;
        float sumSpeed = 0.0f;

        int throngSize = 0;
        foreach (var data in throngDatas)
        {
            var toVec = data.gameObject.transform.position - transform.position;
            if (toVec.magnitude > m_param.inThrongRange)
            {  //����������������continue
                continue;
            }

            centerPosition += CalcuCenterVector(data);
            directVec += CalcuDirectVector(data); //�Q�O�̕��ϕ���
            avoidVec += CalcuAvoidVector(data);
            sumSpeed += data.velocityMgr.velocity.magnitude;

            throngSize++;
        }

        if (throngSize == 0)
        {  //���݂��Ȃ�������zero��Ԃ��B
            return Vector3.zero;
        }

        centerPosition /= throngSize;
        directVec /= throngSize;
        //directVec = (directVec - transform.position) / 8;

        //var reVec = (centerPosition + avoidVec) - transform.position;
        var reVec = (centerPosition + avoidVec + directVec) - transform.position;

        return reVec;
    }

    Vector3 CalcuSumAvoidVector()
    {
        var throngDatas = m_generator.GetThrongDatas();
        Vector3 avoidVector = Vector3.zero;

        foreach (var data in throngDatas)
        {
            avoidVector += CalcuAvoidVector(data);
        }

        return avoidVector;
    }

    float CalcuAverageSpeed()
    {
        var throngDatas = m_generator.GetThrongDatas();
        float sumSpeed = 0.0f;
        int throngSize = 0;

        foreach (var data in throngDatas)
        {
            if (IsRange(data, m_param.nearObjectRange))
            {
                sumSpeed += data.velocityMgr.velocity.magnitude;
                throngSize++;
            }
        }

        if(throngSize == 0)
        {
            return 0;
        }

        return sumSpeed / throngSize;
    }

    bool IsRange(ThrongData data ,float range)
    {
        var toVec = data.gameObject.transform.position - transform.position;

        return toVec.magnitude < range ? true : false;
    }

    //�A�N�Z�b�T-----------------------------------------------

    /// <summary>
    /// �Q�O�f�[�^���X�g��n���B
    /// </summary>
    /// <returns>�Q�O�f�[�^���X�g</returns>
    public List<ThrongData> GetThrongDatas()
    {
        return m_generator.GetThrongDatas();
    }
    


    //null���--------------------------

    void SetSearchGenerator()
    {
        if(m_generator != null) {  //null�o�Ȃ������珈�������Ȃ��B
            return;
        }

        var generators = FindObjectsOfType<EnemyGenerator>();

        foreach (var generator in generators)
        {
            var createObj = generator.GetCreateObject();
            //��������I�u�W�F�N�g�����̃I�u�W�F�N�g�Ɠ����Ȃ�
            if (createObj.GetType() == gameObject.GetType())
            {
                m_generator = generator;
                break;
            }
        }
    }


    //�{�c�f�[�^��

    //�Q�O�Ƃ��ĔF������I�u�W�F�N�g�̃Z�b�g
    //void SetCalcuThrongList()
    //{
    //    var datas = m_generator.GetThrongDatas();
    //    foreach (var data in datas)
    //    {
    //        if (this.gameObject == data.gameObject)
    //        {  //�������g�Ȃ�continue
    //            continue;
    //        }

    //        var toVec = data.gameObject.transform.position - transform.position;
    //        if (toVec.magnitude < m_param.inThrongRange)  //�Q�O�ɂȂ�͈͂ɂ�����
    //        {
    //            if (IsNewData(data))  //�V�K�̃f�[�^��������
    //            {
    //                m_throngDatas.Add(data);
    //            }
    //        }
    //    }
    //}

    //bool IsNewData(ThrongData newData)
    //{
    //    foreach (var data in m_throngDatas)
    //    {
    //        if (data.gameObject == newData.gameObject)
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}
}
