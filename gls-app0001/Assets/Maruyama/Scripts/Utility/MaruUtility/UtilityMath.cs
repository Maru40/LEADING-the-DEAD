using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public class UtilityMath
    {
        /// <summary>
        /// 正面にいるかどうか
        /// </summary>
        /// <param name="forward">自分自身のフォワード</param>
        /// <param name="toTargetVector">ターゲットの方向</param>
        /// <param name="frontDegree">正面とする範囲(最大90度)</param>
        /// <returns>正面にいるならtrue</returns>
        public static bool IsFront(Vector3 selfForward, Vector3 toTargetVector, float frontDegree = 90.0f)
        {
            var frontRad = frontDegree * Mathf.Deg2Rad;

            var fDot = Vector3.Dot(selfForward, toTargetVector.normalized);
            if(fDot < 0.0f) {  //0以下なら正面でない
                return false;
            }

            var rad = Mathf.Acos(fDot);

            //指定した角度より小さかったら正面判定
            return rad <= frontRad ? true : false;
        }

        /// <summary>
        /// NaNかどうかの判断
        /// </summary>
        /// <param name="vector">判断したいVector</param>
        /// <returns>NaNならture</returns>
        public static bool IsNaN(Vector3 vector)
        {
            if(float.IsNaN(vector.x) ||
                float.IsNaN(vector.y) ||
                float.IsNaN(vector.z))
            {
                return true;
            }

            return false;
        }
    }
}


