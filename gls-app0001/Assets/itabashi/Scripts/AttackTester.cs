using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var takeDamager = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();

        takeDamager?.TakeDamage(new AttributeObject.DamageData(10));
    }
}
