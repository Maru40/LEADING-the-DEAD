using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージのデータ
/// </summary>
public struct DamageData
{
    /// <summary>
    /// ダメージ量
    /// </summary>
    public int damage;

    public DamageData(int damage)
    {
        this.damage = damage;
    }
}

/// <summary>
/// ダメージが受けられるインターフェース
/// </summary>
public interface I_TakeDamage
{
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damageData">受けるダメージデータ</param>
    void TakeDamage(DamageData damageData);
}
