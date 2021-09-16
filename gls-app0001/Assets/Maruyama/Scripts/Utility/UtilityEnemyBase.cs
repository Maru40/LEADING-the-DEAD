using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy用のUtilityの作成ベース
/// </summary>
public abstract class UtilityEnemyBase
{
    GameObject m_owner = null;

    public UtilityEnemyBase(GameObject owner)
    {
        m_owner = owner;
    }

    //アクセッサ-----------------------------------------------

    protected GameObject GetOwner()
    {
        return m_owner;
    }
}
