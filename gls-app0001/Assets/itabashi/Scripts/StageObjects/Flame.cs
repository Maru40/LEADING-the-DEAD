using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField]
    private float m_touchDamage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        var takeDamageObject = other.gameObject.GetComponent<AttributeObject.TakeDamageObject>();

        takeDamageObject?.TakeDamage(new AttributeObject.DamageData(m_touchDamage));
    }
}
