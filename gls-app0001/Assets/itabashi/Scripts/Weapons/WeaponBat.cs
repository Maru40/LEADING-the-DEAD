using AttributeObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBat : WeaponBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnDamageTheOpponent(TakeDamageObject takeDamageObject, DamageData baseDamageData)
    {
        takeDamageObject.TakeDamage(baseDamageData);
    }
}
