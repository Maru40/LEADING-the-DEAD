using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackNodeBase : MonoBehaviour
{
    [SerializeField]
    protected AttackParametorBase m_baseParam = new AttackParametorBase(new AttributeObject.DamageData(1.0f), 1.5f);

    public void SetBaseParam(AttackParametorBase param)
    {
        m_baseParam = param;
    }
    public AttackParametorBase GetBaseParam()
    {
        return m_baseParam;
    }

    public void AddBaseParam(AttackParametorBase param)
    {
        m_baseParam.damageData.damageValue += param.damageData.damageValue;
        m_baseParam.startRange += param.startRange;
    }

    /// <summary>
    /// 攻撃力
    /// </summary>
    /// <returns>攻撃力</returns>
    public float GetDamageValue()
    {
        return m_baseParam.damageData.damageValue;
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
    /// 攻撃開始処理
    /// </summary>
    public abstract void AttackStart();

    /// <summary>
    /// 攻撃処理(アニメーションに合わせる)
    /// </summary>
    public abstract void AttackHitStart();

    /// <summary>
    /// 攻撃判定の終了を呼ぶ処理
    /// </summary>
    public abstract void AttackHitEnd();

    /// <summary>
    /// アニメーションの終了時に呼び出す関数
    /// </summary>
    public abstract void EndAnimationEvent();
}
