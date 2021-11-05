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
        /// <param name="isDeleteHeight">高さのYを計算から除外するかどうか</param>
        /// <returns>目的地ならtrue</returns>
        public static bool IsArrivalPosition(float nearRange, Vector3 selfPosition, Vector3 targetPosition, bool isDeleteHeight = false)
        {
            if (isDeleteHeight) {  //高さ情報を削除したい場合
                selfPosition.y = 0;
                targetPosition.y = 0;
            }

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



