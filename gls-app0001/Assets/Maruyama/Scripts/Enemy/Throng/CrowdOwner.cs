using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CrowdOnwerParametor
{
    public Rigidbody rigidbody;

    public CrowdOnwerParametor(Rigidbody rigid)
    {
        this.rigidbody = rigid;
    }
}

public class CrowdOwner : MonoBehaviour
{
    CrowdOnwerParametor m_param;

    List<CrowdChild> m_children = new List<CrowdChild>();


    void Start()
    {
        var rigid = GetComponent<Rigidbody>();

        m_param = new CrowdOnwerParametor(rigid);
    }

    void Update()
    {
        
    }

    public CrowdOnwerParametor GetCrowdOnwerParametor()
    {
        return m_param;
    }

    public List<CrowdChild> GetChildren()
    {
        return m_children;
    }
}
