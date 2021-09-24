using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility 
{
    public class Calculation
    {
        /// <summary>
        /// ターゲットに向かったベクトルを返す
        /// </summary>
        /// <param name="selfTrans">自分自身のTransform</param>
        /// <param name="targetTrans">ターゲットのトランスフォーム</param>
        /// <returns>ターゲット方向のベクトル</returns>
        public static Vector3 CalcuToTargetVec(Transform selfTrans, Transform targetTrans)
        {
            return targetTrans.position - selfTrans.position;
        }

        /// <summary>
        /// 目的地に到達したかどうか
        /// </summary>
        /// <param name="nearRange">誤差範囲</param>
        /// <param name="selfPosition">自分のポジション</param>
        /// <param name="targetPosition">目的地</param>
        /// <returns>目的地ならtrue</returns>
        public static bool IsArrivalPosition(float nearRange, Vector3 selfPosition, Vector3 targetPosition)
        {
            var toVec = targetPosition - selfPosition;
            return toVec.magnitude < nearRange ? true : false;
        }

        public static bool IsRange(GameObject selfObj, GameObject targetObj, float range)
        {
            var toVec = targetObj.transform.position - selfObj.transform.position;
            return toVec.magnitude < range ? true : false;
        }
    }
}



