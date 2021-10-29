using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagedManagerBase
{
    GameObject m_owner = null;

    public DamagedManagerBase(GameObject owner)
    {
        m_owner = owner;
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="data">ダメージデータ</param>
    public abstract void Damaged(AttributeObject.DamageData data);

    //アクセッサ------------------------------------------------------------

    public GameObject GetOwner()
    {
        return m_owner;
    }

}
