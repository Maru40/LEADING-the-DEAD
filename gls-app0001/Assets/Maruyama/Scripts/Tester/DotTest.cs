using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTest : MonoBehaviour
{
    [SerializeField]
    GameObject m_target = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_target)
        {
            var toVec = m_target.transform.position - transform.position;
            var testDot = Vector3.Dot(toVec.normalized, transform.forward);
            Debug.Log("DotTest: " + testDot );
        }
    }
}
