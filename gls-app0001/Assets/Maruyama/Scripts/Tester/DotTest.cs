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
            toVec.y = 0;
            var testDot = Vector3.Dot(transform.forward, toVec.normalized);
            var cross = Vector3.Cross(transform.forward, toVec.normalized);
            var rad = Mathf.Acos(testDot);
            //Debug.Log("DotTest: " + rad * Mathf.Rad2Deg );
            //Debug.Log("DotTest: " + Vector3.Angle(transform.forward, toVec));
            //Debug.Log("DotTest: " + Vector3.SignedAngle(transform.forward, toVec, Vector3.up));
            //Debug.Log("TestCross" + cross);
        }
    }
}
