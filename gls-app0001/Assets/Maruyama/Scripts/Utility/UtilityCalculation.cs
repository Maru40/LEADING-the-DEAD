using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCalculation
{
    /// <summary>
    /// �^�[�Q�b�g�Ɍ��������x�N�g����Ԃ�
    /// </summary>
    /// <param name="selfTrans">�������g��Transform</param>
    /// <param name="targetTrans">�^�[�Q�b�g�̃g�����X�t�H�[��</param>
    /// <returns>�^�[�Q�b�g�����̃x�N�g��</returns>
    public static Vector3 CalcuToTargetVec(Transform selfTrans, Transform targetTrans)
    {
        return targetTrans.position - selfTrans.position;
    }
}
