using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Chase
{
    /// <summary>
    /// ステートの変更
    /// </summary>
    void ChangeState();

    /// <summary>
    /// ターゲットを見失った時
    /// </summary>
    void TargetLost();
}
