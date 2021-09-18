using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct AttackParametorBase
{
    public float power;
    public float startRange;  //�U���J�n�͈�
    public float damageRange; //�U���_���[�W�͈�

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
    /// �A�j���[�V�����̏I�����ɌĂяo���֐�
    /// </summary>
    public abstract void EndAnimationEvent();
}
