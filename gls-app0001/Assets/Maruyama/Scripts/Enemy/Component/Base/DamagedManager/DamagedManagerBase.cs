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

    public abstract void Damaged(AttributeObject.DamageData data);

    //アクセッサ------------------------------------------------------------

    public GameObject GetOwner()
    {
        return m_owner;
    }

}
