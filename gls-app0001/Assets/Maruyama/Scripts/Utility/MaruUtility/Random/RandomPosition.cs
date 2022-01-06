using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{

    public class RandomPosition
    {
        //Randomにするためにstatic化
        private static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond);

        public static Vector3 CalcuVec(Vector3 maxRangeBase)
        {
            var maxRange = new Vector3(Mathf.Abs(maxRangeBase.x), Mathf.Abs(maxRangeBase.y), Mathf.Abs(maxRangeBase.z));

            Vector3 minVec = -maxRange;
            Vector3 maxVec = maxRange;

            Vector3 randomPosition = Vector3.zero;

            randomPosition.x = Random.Range(minVec.x, maxVec.x);
            randomPosition.y = Random.Range(minVec.y, maxVec.y);
            randomPosition.z = Random.Range(minVec.z, maxVec.z);

            return randomPosition;
        }

        /// <summary>
        /// ランダムなポジションを返す
        /// </summary>
        /// <param name="random">ランダムクラス</param>
        /// <param name="maxRange">ランダムにする範囲</param>
        /// <param name="centerPosition">基準となる中心のポジション</param>
        /// <returns>ランダムなポジション</returns>
        public static Vector3 CalcuPosition(Vector3 maxRange, Vector3 centerPosition)
        {
            var randomPosition = CalcuVec(maxRange);
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
                var position = CalcuPosition(maxRange, centerPosition);
                var targetPosition = target.transform.position;
                //カメラの範囲外、且つ、ターゲットの範囲外なら
                if(!CalcuCamera.IsInCamera(position, camera) && !Calculation.IsRange(position, targetPosition, outRange))
                {
                    return position;
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// カメラの外で、障害物の外で、複数のターゲットから離れた距離の、ランダムに返す。
        /// </summary>
        /// <param name="targets">ターゲット達</param>
        /// <param name="outRange">離す距離</param>
        /// <param name="camera">カメラ</param>
        /// <param name="maxRange">ランダムに生成する範囲</param>
        /// <param name="centerPosition">ランダムに生成する中心位置</param>
        /// <returns>ランダムな位置</returns>
        public static Vector3 OutCameraAndOutObstacleAndOutRangeOfTargets(List<GameObject> targets, float outRange,
            Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3(), params string[] layerNames)
        {
            List<OutOfTargetData> datas = new List<OutOfTargetData>();

            foreach(var target in targets)
            {
                datas.Add(new OutOfTargetData(target, outRange));
            }

            return OutCameraAndOutObstacleAndOutRangeOfTargets(datas, camera, maxRange, centerPosition, layerNames);
        }

        /// <summary>
        /// カメラの外で、障害物の外で、複数のターゲットから離れた距離の、ランダムに返す。
        /// </summary>
        /// <param name="datas">ターゲットと話したい距離のデータ</param>
        /// <param name="camera">カメラ</param>
        /// <param name="maxRange">ランダムに生成する範囲</param>
        /// <param name="centerPosition">ランダムに生成する中心位置</param>
        /// <returns>ランダムな位置</returns>
        public static Vector3 OutCameraAndOutObstacleAndOutRangeOfTargets(List<OutOfTargetData> datas,
            Camera camera, Vector3 maxRange, Vector3 centerPosition = new Vector3(), params string[] layerNames)
        {
            const int numLoop = 100;
            for (int i = 0; i < numLoop; i++)
            {
                if(i > numLoop - 2) {
                    Debug.Log("ループオーバー");
                }

                var position = CalcuPosition(maxRange, centerPosition);

                //カメラと比較
                if (CalcuCamera.IsInCamera(position, camera))  //カメラ内なら処理を飛ばす。
                {
                    continue;
                }

                //ポジションが障害物の中だったら
                if (IsInObstacle(position, layerNames))  
                {
                    continue;
                }

                //ターゲットの全てと比較して、離れていたら
                if (IsOutRangeOfTargets(position, datas))
                {
                    return position;  //全ての条件をクリアしたら、positionを返す。
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 障害物の外で、複数のターゲットから離れている場所
        /// </summary>
        /// <param name="datas">ターゲットと話したい距離のデータ</param>
        /// <param name="maxRange">ランダムに生成する範囲</param>
        /// <param name="maxRange">ランダムに生成する範囲</param>
        /// <param name="centerPosition">ランダムに生成する中心位置</param>
        /// <returns>ランダムな位置</returns>
        public static Vector3 OutObstacleAndOutRangeOfTargets(List<OutOfTargetData> datas,
            Vector3 maxRange, Vector3 centerPosition = new Vector3(), params string[] layerNames)
        {
            const int numLoop = 100;
            for (int i = 0; i < numLoop; i++)
            {
                var position = CalcuPosition(maxRange, centerPosition);

                //ポジションが障害物の中だったら
                if (IsInObstacle(position, layerNames))
                {
                    continue;
                }

                //ターゲットの全てと比較して、離れていたら
                if (IsOutRangeOfTargets(position, datas))
                {
                    return position;  //全ての条件をクリアしたら、positionを返す。
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 障害物の中だったらtrue
        /// </summary>
        /// <param name="position">生成する場所</param>
        /// <param name="layerNames">障害物レイヤーの設定</param>
        /// <returns>障害物の中だったらtrue</returns>
        public static bool IsInObstacle(Vector3 position, params string[] layerNames)
        {
            const float Radius = 1.0f;
            int layerIndex = LayerMask.GetMask(layerNames);
            var colliders = Physics.OverlapSphere(position, Radius, layerIndex);

            return colliders.Length != 0 ? true : false;
        }

        /// <summary>
        /// 全てのターゲットから離れているかどうか
        /// </summary>
        /// <param name="selfPosition">自分のポジション</param>
        /// <param name="targets">ターゲット群</param>
        /// <param name="range">離れてほしい距離</param>
        /// <returns>離れていたらtrue</returns>
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
        /// 全てのターゲットから離れているかどうか
        /// </summary>
        /// <param name="selfPosition">自分自身のポジション</param>
        /// <param name="datas">ターゲットと話したい距離のデータ</param>
        /// <returns>ランダムなポジション</returns>
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


