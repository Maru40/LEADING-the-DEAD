using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Attack
{
    /// <summary>
    /// �U������(��ɃA�j���[�V�����C�x���g)
    /// </summary>
    public void Attack();

    /// <summary>
    /// �U���J�n���邩�ǂ���
    /// </summary>
    /// <returns>�U�����J�n����Ȃ�true</returns>
    public bool IsAttackStart();

    /// <summary>
    /// �Ώۂɓ����������ǂ���
    /// </summary>
    /// <returns>���������Ȃ�true</returns>
    public bool IsHit();
}
