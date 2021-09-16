using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MyTrigger
{
    bool m_isTrigger;

    /// <summary>
    /// Trigger��true�ɂ���B
    /// </summary>
    public void Fire()
    {
        m_isTrigger = true;
    }

    /// <summary>
    /// ���݂̃g���K�[���擾���A�g���K�[��off�ɂ���B
    /// </summary>
    /// <returns>�g���K�[�̒l(bool�^)</returns>
    public bool Get()
    {
        bool isTrigger = m_isTrigger;
        m_isTrigger = false;
        return isTrigger;
    }
}
