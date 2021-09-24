using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{

    public class RandomPosition
    {
        //Random�ɂ��邽�߂�static��
        static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond);

        /// <summary>
        /// �����_���ȃ|�W�V������Ԃ�
        /// </summary>
        /// <param name="random">�����_���N���X</param>
        /// <param name="maxRange">�����_���ɂ���͈�</param>
        /// <param name="centerPosition">��ƂȂ钆�S�̃|�W�V����</param>
        /// <returns>�����_���ȃ|�W�V����</returns>
        public static Vector3 CalcuPosition(Vector3 maxRange, Vector3 centerPosition)
        {
            Vector3 minVec = -maxRange;
            Vector3 maxVec = maxRange;
            Vector3 randomPosition = Vector3.zero;

            randomPosition.x = sm_random.Next((int)minVec.x, (int)maxVec.x);
            randomPosition.y = sm_random.Next((int)minVec.y, (int)maxVec.y);
            randomPosition.z = sm_random.Next((int)minVec.z, (int)maxVec.z);
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
        public static Vector3 OutRangeOfTarget(GameObject target, float outRange, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            const int numLoop = 100;
            Vector3 returnPosition = Vector3.zero;
            for (int i = 0; i < numLoop; i++)
            {
                var position = CalcuPosition(maxRange, centerPosition);

                var toVec = target.transform.position - position;
                if (toVec.magnitude > outRange)
                {  //target��艓��������
                    returnPosition = position;
                    break;
                }
            }

            return returnPosition;
        }

        /// <summary>
        /// �J�����̊O�̃|�W�V�����������_���ɕԂ�
        /// </summary>
        /// <param name="camera">�J����</param>
        /// <param name="maxRange">�����_���ɂ���͈�</param>
        /// <param name="centerPosition">��ƂȂ钆�S�̃|�W�V����</param>
        /// <returns>�����_���ȃ|�W�V����</returns>
        public static Vector3 OutCameraOfTarget(Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            const int numLoop = 100;
            for (int i = 0; i < numLoop; i++)
            {
                var positon = CalcuPosition(maxRange, centerPosition);
                if (!CalcuCamera.IsInCamera(positon, camera))
                {
                    return positon;
                }
            }

            return Vector3.zero;
        }
    }

}


