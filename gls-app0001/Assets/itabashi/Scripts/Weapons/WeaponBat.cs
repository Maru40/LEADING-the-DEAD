using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBat : WeaponBase
{
    [SerializeField]
    private GameObject m_hitEffectPrefab; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnDamageTheOpponent(TakeDamageObject takeDamageObject, DamageData baseDamageData, Vector3 hitPosition)
    {
        var hitEffect = Instantiate(m_hitEffectPrefab, hitPosition, Quaternion.identity);

        takeDamageObject.TakeDamage(baseDamageData);
    }
}
