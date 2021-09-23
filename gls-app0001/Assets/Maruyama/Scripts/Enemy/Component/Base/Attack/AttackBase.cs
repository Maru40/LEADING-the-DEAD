using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct AttackParametorBase
{
    public float power;
    public float startRange;  //�U���J�n�͈�

    public AttackParametorBase(float power, float startRange)
    {
        this.power = power;
        this.startRange = startRange;
    }
}

public abstract class AttackBase : MonoBehaviour
{
    [SerializeField]
    private AttackParametorBase m_baseParam = new AttackParametorBase(10.0f, 1.0f);


    public void SetBaseParam(AttackParametorBase param)
    {
        m_baseParam = param;
    }
    public AttackParametorBase GetBaseParam()
    {
        return m_baseParam; 
    }

    /// <summary>
    /// �U����
    /// </summary>
    /// <returns>�U����</returns>
    public float GetPower()
    {
        return m_baseParam.power;
    }

    /// <summary>
    /// �U�����[�V�����J�n�̋���
    /// </summary>
    /// <returns>����</returns>
    public float GetAttackStartRange()
    {
        return m_baseParam.startRange;
    }

    /// <summary>
    /// �U������(�A�j���[�V�����ɍ��킹��)
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// �U������̏I�����Ăԏ���
    /// </summary>
    public abstract void AttackHitEnd();

    /// <summary>
    /// �A�j���[�V�����̏I�����ɌĂяo���֐�
    /// </summary>
    public abstract void EndAnimationEvent();
}
