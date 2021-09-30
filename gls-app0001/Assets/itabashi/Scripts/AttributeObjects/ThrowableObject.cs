using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ThrowingData
{
    public Vector3 throwVector;

    public ThrowingData(Vector3 throwVector)
    {
        this.throwVector = throwVector;
    }
}

[RequireComponent(typeof(Rigidbody))]

public class ThrowableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsUseGravity()
    {
        return m_rigidbody.useGravity;
    }

    public float GetMass()
    {
        return m_rigidbody.mass;
    }

    public void Takeing()
    {
        if(!m_rigidbody)
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }
        m_rigidbody.isKinematic = true;
    }

    public void Throwing(ThrowingData throwingData)
    {
        if (!m_rigidbody)
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        m_rigidbody.isKinematic = false;
        m_rigidbody.velocity = throwingData.throwVector;
    }
}
