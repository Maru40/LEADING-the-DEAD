using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager_ZombieChild : StatusManagerBase
{
    [Header("ダメージを受けるタイプ"), SerializeField]
    public List<DamageType> m_damageTypes = new List<DamageType>() { DamageType.Fire };

    private DamageManager_ZombieChild m_damageManager;

    private void Awake()
    {
        m_damageManager = new DamageManager_ZombieChild(gameObject);
    }

    public override void Damage(DamageData data)
    {
        if(data.IsType(m_damageTypes.ToArray()))
        {
            m_damageManager.Damaged(data);
        }
    }
}
