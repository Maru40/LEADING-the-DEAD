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
        public static Vector3 OutCamera(Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3())
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

        /// <summary>
        /// �J�����̊O�ŁA�^�[�Q�b�g�͈̔͊O�̎�
        /// </summary>
        /// <param name="target">�^�[�Q�b�g</param>
        /// <param name="outRange">�͈͊O���苗��</param>
        /// <param name="camera">�J����</param>
        /// <param name="maxRange">�����_���ȍő勗��</param>
        /// <param name="centerPosition">��̒��S�ʒu</param>
        /// <returns>�����_���Ȉʒu</returns>
        public static Vector3 OutCameraAndOutRangeOfTarget(GameObject target, float outRange,
            Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            if(target == null) {
                return Vector3.zero;
            }

            const int numLoop = 100;
            for (int i = 0; i < numLoop; i++)
            {
                var position = CalcuPosition(maxRange, centerPosition);
                var targetPosition = target.transform.position;
                //�J�����͈̔͊O�A���A�^�[�Q�b�g�͈̔͊O�Ȃ�
                if(!CalcuCamera.IsInCamera(position, camera) && !Calculation.IsRange(position, targetPosition, outRange))
                {
                    return position;
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// �J�����̊O�ŕ����̃^�[�Q�b�g���痣�ꂽ�����������_���ɕԂ��B
        /// </summary>
        /// <param name="targets">�^�[�Q�b�g�B</param>
        /// <param name="outRange">��������</param>
        /// <param name="camera">�J����</param>
        /// <param name="maxRange">�����_���ɐ�������͈�</param>
        /// <param name="centerPosition">�����_���ɐ������钆�S�ʒu</param>
        /// <returns>�����_���Ȉʒu</returns>
        public static Vector3 OutCameraAndOutRangeOfTargets(List<GameObject> targets, float outRange,
            Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            List<OutOfTargetData> datas = new List<OutOfTargetData>();

            foreach(var target in targets)
            {
                datas.Add(new OutOfTargetData(target, outRange));
            }

            return OutCameraAndOutRangeOfTargets(datas, camera, maxRange, centerPosition);
        }

        /// <summary>
        /// �J�����̊O�ŕ����̃^�[�Q�b�g���痣�ꂽ�����������_���ɕԂ��B
        /// </summary>
        /// <param name="datas">�^�[�Q�b�g�Ƙb�����������̃f�[�^</param>
        /// <param name="camera">�J����</param>
        /// <param name="maxRange">�����_���ɐ�������͈�</param>
        /// <param name="centerPosition">�����_���ɐ������钆�S�ʒu</param>
        /// <returns>�����_���Ȉʒu</returns>
        public static Vector3 OutCameraAndOutRangeOfTargets(List<OutOfTargetData> datas,
            Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            const int numLoop = 100;
            for (int i = 0; i < numLoop; i++)
            {
                var position = CalcuPosition(maxRange, centerPosition);

                //�J�����Ɣ�r
                if (CalcuCamera.IsInCamera(position, camera))  //�J�������Ȃ珈�����΂��B
                {
                    continue;
                }

                //�^�[�Q�b�g�̑S�ĂƔ�r���āA����Ă�����
                if (IsOutRangeOfTargets(position, datas))
                {
                    return position;  //�S�Ă̏������N���A������Aposition��Ԃ��B
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// �S�Ẵ^�[�Q�b�g���痣��Ă��邩�ǂ���
        /// </summary>
        /// <param name="selfPosition">�����̃|�W�V����</param>
        /// <param name="targets">�^�[�Q�b�g�Q</param>
        /// <param name="range">����Ăق�������</param>
        /// <returns>����Ă�����true</returns>
        public static bool IsOutRangeOfTargets(Vector3 selfPosition, List<GameObject> targets, float range)
        {
            foreach (var target in targets)
            {
                if (Calculation.IsRange(selfPosition, target, range))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// �S�Ẵ^�[�Q�b�g���痣��Ă��邩�ǂ���
        /// </summary>
        /// <param name="selfPosition">�������g�̃|�W�V����</param>
        /// <param name="datas">�^�[�Q�b�g�Ƙb�����������̃f�[�^</param>
        /// <returns>�����_���ȃ|�W�V����</returns>
        public static bool IsOutRangeOfTargets(Vector3 selfPosition, List<OutOfTargetData> datas)
        {
            foreach (var data in datas)
            {
                if (Calculation.IsRange(selfPosition, data.target, data.range))
                {
                    return false;
                }
            }

            return true;
        }
    }
}


