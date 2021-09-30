using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CymbalMonkeyDamager : MonoBehaviour, I_TakeDamage
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(DamageData damageData)
    {
        Debug.Log("猿に触れました");
    }
}
