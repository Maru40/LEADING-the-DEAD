using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Attack
{
    /// <summary>
    /// 攻撃処理(主にアニメーションイベント)
    /// </summary>
    public void Attack();

    /// <summary>
    /// 攻撃開始するかどうか
    /// </summary>
    /// <returns>攻撃を開始するならtrue</returns>
    public bool IsAttackStart();

    /// <summary>
    /// 対象に当たったかどうか
    /// </summary>
    /// <returns>当たったならtrue</returns>
    public bool IsHit();
}
