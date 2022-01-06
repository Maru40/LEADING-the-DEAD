using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public class CalcuCamera
    {
        /// <summary>
        /// カメラの中にいるかどうか
        /// </summary>
        /// <param name="position">ポジション</param>
        /// <param name="camera">カメラ</param>
        /// <returns></returns>
        public static bool IsInCamera(Vector3 position, Camera camera)
        {
            var viewportPosition = camera.WorldToViewportPoint(position);

            if (0.0f <= viewportPosition.x && viewportPosition.x <= 1.0f
                && 0.0f <= viewportPosition.y && viewportPosition.y <= 1.0f
                )
            {
                return true;
            }

            return false;
        }
    }
}


