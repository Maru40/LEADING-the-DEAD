using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityRandomPosition
{

    /// <summary>
    /// �����_���ȃ|�W�V������Ԃ�
    /// </summary>
    /// <param name="random">�����_���N���X</param>
    /// <param name="maxRange">�����_���ɂ���͈�</param>
    /// <param name="centerPosition">��ƂȂ钆�S�̃|�W�V����</param>
    /// <returns>�����_���ȃ|�W�V����</returns>
    public static Vector3 CalcuRandom(System.Random random, Vector3 maxRange , Vector3 centerPosition)
    {
        Vector3 minVec = -maxRange;
        Vector3 maxVec =  maxRange;
        Vector3 randomPosition = Vector3.zero;

        randomPosition.x = random.Next((int)minVec.x, (int)maxVec.x);
        randomPosition.y = random.Next((int)minVec.y, (int)maxVec.y);
        randomPosition.z = random.Next((int)minVec.z, (int)maxVec.z);
        return centerPosition + randomPosition;
    }

    /// <summary>
    /// �^�[�Q�b�g������͈͊O�̃|�W�V������Ԃ��B
    /// </summary>
    /// <param name="target">�^�[�Q�b�g</param>
    /// <param name="outRange">�^�[�Q�b�g���痣���ė~��������</param>
    /// <param name="maxRange">�����_���ɂ���͈�</param>
    /// <param name="centerPosition">��ƂȂ钆�S�̃|�W�V����</param>
    /// <returns>�����_���ȃ|�W�V����</returns>
    public static Vector3 CalcuOutRangeOfTarget(GameObject target, float outRange, Vector3 maxRange, Vector3 centerPosition = new Vector3())
    {
        const int numLoop = 100;
        var random = new System.Random(System.DateTime.Now.Millisecond);
        Vector3 returnPosition = Vector3.zero;
        for (int i = 0; i < numLoop; i++)
        {
            var position = CalcuRandom(random, maxRange, centerPosition);

            var toVec = target.transform.position - position;
            if (toVec.magnitude > outRange)
            {  //target��艓��������
                returnPosition = position;
                break;
            }
        }

        return returnPosition;
    }

}
