using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���[�W�̃f�[�^
/// </summary>
public struct DamageData
{
    /// <summary>
    /// �_���[�W��
    /// </summary>
    public int damage;

    public DamageData(int damage)
    {
        this.damage = damage;
    }
}

/// <summary>
/// �_���[�W���󂯂���C���^�[�t�F�[�X
/// </summary>
public interface I_TakeDamage
{
    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="damageData">�󂯂�_���[�W�f�[�^</param>
    void TakeDamage(DamageData damageData);
}
