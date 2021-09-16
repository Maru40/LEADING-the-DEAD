using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy�p��Utility�̍쐬�x�[�X
/// </summary>
public abstract class UtilityEnemyBase
{
    GameObject m_owner = null;

    public UtilityEnemyBase(GameObject owner)
    {
        m_owner = owner;
    }

    //�A�N�Z�b�T-----------------------------------------------

    protected GameObject GetOwner()
    {
        return m_owner;
    }
}
