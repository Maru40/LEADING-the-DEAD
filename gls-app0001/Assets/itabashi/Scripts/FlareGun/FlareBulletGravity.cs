using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareBulletGravity : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_rigidbody.velocity.y < 0)
        {
            m_rigidbody.useGravity = false;
            m_rigidbody.velocity = new Vector3();
        }
    }
}
