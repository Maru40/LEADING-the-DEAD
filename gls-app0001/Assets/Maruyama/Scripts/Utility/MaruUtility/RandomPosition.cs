using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{

    public class RandomPosition
    {
        //Randomにするためにstatic化
        static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond);

        /// <summary>
        /// ランダムなポジションを返す
        /// </summary>
        /// <param name="random">ランダムクラス</param>
        /// <param name="maxRange">ランダムにする範囲</param>
        /// <param name="centerPosition">基準となる中心のポジション</param>
        /// <returns>ランダムなポジション</returns>
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
        /// ターゲットから一定範囲外のポジションを返す。
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="outRange">ターゲットから離して欲しい距離</param>
        /// <param name="maxRange">ランダムにする範囲</param>
        /// <param name="centerPosition">基準となる中心のポジション</param>
        /// <returns>ランダムなポジション</returns>
        public static Vector3 OutRangeOfTarget(GameObject target, float outRange, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            const int numLoop = 100;
            Vector3 returnPosition = Vector3.zero;
            for (int i = 0; i < numLoop; i++)
            {
                var position = CalcuPosition(maxRange, centerPosition);

                var toVec = target.transform.position - position;
                if (toVec.magnitude > outRange)
                {  //targetより遠かったら
                    returnPosition = position;
                    break;
                }
            }

            return returnPosition;
        }

        /// <summary>
        /// カメラの外のポジションをランダムに返す
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="maxRange">ランダムにする範囲</param>
        /// <param name="centerPosition">基準となる中心のポジション</param>
        /// <returns>ランダムなポジション</returns>
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
        /// カメラの外で、ターゲットの範囲外の時
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="outRange">範囲外判定距離</param>
        /// <param name="camera">カメラ</param>
        /// <param name="maxRange">ランダムな最大距離</param>
        /// <param name="centerPosition">基準の中心位置</param>
        /// <returns>ランダムな位置</returns>
        public static Vector3 OutCameraAndOutRangeOfTarget(GameObject target, float outRange,
            Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3())
        {
            if(target == null) {
                return Vector3.zero;
            }

            const int numLoop = 100;
            for (int i = 0; i < numLoop; i++)
            {
                var positon = CalcuPosition(maxRange, centerPosition);
                var targetPosition = target.transform.position;
                //カメラの範囲外、且つ、ターゲットの範囲外なら
                if(!CalcuCamera.IsInCamera(positon, camera) && !Calculation.IsRange(positon, targetPosition, outRange))
                {
                    return positon;
                }
            }

            return Vector3.zero;
        }
    }
}


