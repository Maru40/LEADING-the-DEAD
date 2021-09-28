using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility 
{
    public class Calculation
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

        /// <summary>
        /// �ړI�n�ɓ��B�������ǂ���
        /// </summary>
        /// <param name="nearRange">�덷�͈�</param>
        /// <param name="selfPosition">�����̃|�W�V����</param>
        /// <param name="targetPosition">�ړI�n</param>
        /// <returns>�ړI�n�Ȃ�true</returns>
        public static bool IsArrivalPosition(float nearRange, Vector3 selfPosition, Vector3 targetPosition)
        {
            var toVec = targetPosition - selfPosition;
            return toVec.magnitude < nearRange ? true : false;
        }

        public static bool IsRange(GameObject selfObj, GameObject targetObj, float range)
        {
            return IsRange(selfObj.transform.position, targetObj.transform.position, range);
        }

        public static bool IsRange(Vector3 selfPosition, GameObject targetObj, float range)
        {
            if(targetObj == null) {
                return false;
            }

            return IsRange(selfPosition, targetObj.transform.position, range);
        }

        public static bool IsRange(GameObject selfObj, Vector3 targetPosition, float range)
        {
            return IsRange(selfObj.transform.position, targetPosition, range);
        }

        public static bool IsRange(Vector3 selfPosition, Vector3 targetPosition, float range)
        {
            var toVec = targetPosition - selfPosition;
            return toVec.magnitude < range ? true : false;
        }
    }
}



