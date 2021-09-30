using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public class CalcuCamera
    {
        public static bool IsInCamera(Vector3 position, Camera camera)
        {
            var viewportPosition = camera.WorldToViewportPoint(position);

            if (0f <= viewportPosition.x && viewportPosition.x <= 1f
                && 0f <= viewportPosition.y && viewportPosition.y <= 1f
                )
            {
                return true;
            }

            return false;
        }
    }
}


