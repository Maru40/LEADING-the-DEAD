using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityRandomPosition
{

    /// <summary>
    /// ランダムなポジションを返す
    /// </summary>
    /// <param name="random">ランダムクラス</param>
    /// <param name="maxRange">ランダムにする範囲</param>
    /// <param name="centerPosition">基準となる中心のポジション</param>
    /// <returns>ランダムなポジション</returns>
    public static Vector3 CalcuRandom(System.Random random, Vector3 maxRange , Vector3 centerPosition)
    {
        Vector3 minVec = -maxRange;
        Vector3 maxVec =  maxRange;
        Vector3 randomPosition = Vector3.zero;

        randomPosition.x = random.Next((int)minVec.x, (int)maxVec.x);
        randomPosition.y = random.Next((int)minVec.y, (int)maxVec.y);
        randomPosition.z = random.Next((int)minVec.z, (int)maxVec.z);
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
    public static Vector3 CalcuOutRangeOfTarget(GameObject target, float outRange, Vector3 maxRange, Vector3 centerPosition = new Vector3())
    {
        const int numLoop = 100;
        var random = new System.Random(System.DateTime.Now.Millisecond);
        Vector3 returnPosition = Vector3.zero;
        for (int i = 0; i < numLoop; i++)
        {
            var position = CalcuRandom(random, maxRange, centerPosition);

            var toVec = target.transform.position - position;
            if (toVec.magnitude > outRange)
            {  //targetより遠かったら
                returnPosition = position;
                break;
            }
        }

        return returnPosition;
    }

}
