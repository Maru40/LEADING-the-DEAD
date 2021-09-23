using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �^�[�Q�b�g�͈̔͊O�Ƀ��X�|�[�����鏈��
/// </summary>
public class RespawnRandom_OutRangeOfTarget : EnemyRespawnBase
{
    [SerializeField]
    GameObject m_target = null;

    //�^�[�Q�b�g����ǂꂾ��������
    [SerializeField]
    float m_outRangeOfTarget = 10.0f;

    [SerializeField]
    EnemyGenerator m_generator = null;

    StatorBase m_stator;

    void Start()
    {
        StartTargetNullCheck();
        StartGeneratorNullCheck();

        m_stator = GetComponent<StatorBase>();
    }

    public void Respawn()
    {
        if(m_target == null || m_generator == null) {
            StartTargetNullCheck();
            StartGeneratorNullCheck();
        }

        var respawnPosition = CalcuRespawnRandomPosition();
        transform.position = respawnPosition;

        m_stator.Reset();  //�X�e�[�g�̃��Z�b�g
    }

    /// <summary>
    /// ���X�|�[������ꏊ�̌v�Z
    /// </summary>
    /// <returns>���X�|�[������ꏊ</returns>
    Vector3 CalcuRespawnRandomPosition()
    {
        const int numLoop = 100;
        var random = new System.Random(System.DateTime.Now.Millisecond);
        Vector3 respawnPosition = Vector3.zero;
        for (int i = 0; i < numLoop; i++)
        {
            var position = m_generator.CalcuRandomPosition(random);

            var toVec = m_target.transform.position - position;
            if (toVec.magnitude > m_outRangeOfTarget)
            {  //target��艓��������
                respawnPosition = position;
                break;
            }
        }

        return respawnPosition;
    }

    //StartNullCheck----------------------------------------------------

    void StartTargetNullCheck()
    {
        if(m_target == null) {
            m_target = GameObject.Find("Player");
        }
    }

    void StartGeneratorNullCheck()
    {
        if(m_generator != null) { //null�o�Ȃ��Ȃ珈�������Ȃ�
            return;
        }

        var generators = GameObject.FindObjectsOfType<EnemyGenerator>();

        foreach (var generator in generators)
        {
            if(generator.IsEqualCreateObject(gameObject))
            {
                m_generator = generator;
                return;
            }
        }
    }
}
