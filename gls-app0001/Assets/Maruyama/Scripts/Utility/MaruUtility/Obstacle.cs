using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public class Obstacle
    {
        private static readonly string[] DEFAULT_OBSTACLE_STRING = new string[] { "L_Obstacle" };

        public static bool IsObstacle(GameObject gameObject)
        {
            return IsObstacle(gameObject, DEFAULT_OBSTACLE_STRING);
        }

        /// <summary>
        /// 障害物かどうか
        /// </summary>
        /// <param name="gameObject">当たった相手</param>
        /// <param name="layerNames">レイヤーネーム</param>
        /// <returns>障害物に当たったら</returns>
        public static bool IsObstacle(GameObject gameObject, params string[] layerNames)
        {
            foreach (var layerName in layerNames)
            {
                if(layerName == LayerMask.LayerToName(gameObject.layer))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 障害物があるかどうか
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns>障害物があるならtrue</returns>
        public static bool IsLineCastObstacle(Vector3 startPosition, Vector3 endPosition)
        {
            return IsLineCastObstacle(startPosition, endPosition, DEFAULT_OBSTACLE_STRING);
        }

        /// <summary>
        /// 障害物があるかどうか
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="layerNames"></param>
        /// <returns>障害物があるならtrue</returns>
        public static bool IsLineCastObstacle(Vector3 startPosition, Vector3 endPosition, params string[] layerNames)
        {
            int obstacleLayer = LayerMask.GetMask(layerNames);

            return Physics.Linecast(startPosition, endPosition, obstacleLayer) ? true : false;
        }
    }
}