using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Test
{
    void A();
}

public class AttackTester : MonoBehaviour, Test
{
    [SerializeField]
    private AudioOptioner audioOptioner;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void A()
    {
        Debug.Log("成功");
    }

    private void OnCollisionEnter(Collision collision)
    {
        var takeDamager = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();

        takeDamager?.TakeDamage(new AttributeObject.DamageData(0,true));
    }
}
