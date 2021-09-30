using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct AttackParametorBase
{
    public float power;
    public float startRange;  //攻撃開始範囲
    public float moveSpeed;   //攻撃時の移動スピード

    public AttackParametorBase(float power, float startRange, float moveSpeed)
    {
        this.power = power;
        this.startRange = startRange;
        this.moveSpeed = moveSpeed;
    }
}

public abstract class AttackBase : MonoBehaviour
{
    [SerializeField]
    private AttackParametorBase m_baseParam = new AttackParametorBase(10.0f, 1.0f, 3.0f);


    public void SetBaseParam(AttackParametorBase param)
    {
        m_baseParam = param;
    }
    public AttackParametorBase GetBaseParam()
    {
        return m_baseParam; 
    }

    /// <summary>
    /// 攻撃力
    /// </summary>
    /// <returns>攻撃力</returns>
    public float GetPower()
    {
        return m_baseParam.power;
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
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    public abstract bool IsAttackStartRange();

    /// <summary>
    /// 攻撃処理(アニメーションに合わせる)
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// 攻撃判定の終了を呼ぶ処理
    /// </summary>
    public abstract void AttackHitEnd();

    /// <summary>
    /// アニメーションの終了時に呼び出す関数
    /// </summary>
    public abstract void EndAnimationEvent();
}
