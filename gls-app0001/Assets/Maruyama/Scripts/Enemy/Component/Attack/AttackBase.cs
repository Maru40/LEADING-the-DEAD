using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct AttackParametorBase
{
    public float power;
    public float startRange;  //攻撃開始範囲
    public float damageRange; //攻撃ダメージ範囲

    public AttackParametorBase(float power, float startRange, float damageRange)
    {
        this.power = power;
        this.startRange = startRange;
        this.damageRange = damageRange;
    }
}

public abstract class AttackBase : MonoBehaviour
{
    [SerializeField]
    private AttackParametorBase m_baseParam = new AttackParametorBase(1.0f, 2.0f, 1.0f);

    public AttackParametorBase GetBaseParam()
    {
        return m_baseParam; 
    }


    /// <summary>
    /// 攻撃モーション開始の距離
    /// </summary>
    /// <returns>距離</returns>
    public float GetAttackStartRange()
    {
        return m_baseParam.startRange;
    }

    /// <summary>
    /// 攻撃処理(アニメーションに合わせる)
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// アニメーションの終了時に呼び出す関数
    /// </summary>
    public abstract void EndAnimationEvent();
}
