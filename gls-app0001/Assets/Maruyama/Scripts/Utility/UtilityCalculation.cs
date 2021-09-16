using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCalculation
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
}
