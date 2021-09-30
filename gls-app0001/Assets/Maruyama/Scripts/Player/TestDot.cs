using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDot : MonoBehaviour
{
    [SerializeField]
    GameObject obj = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var toVec = obj.transform.position - transform.position;

        var dot = Vector3.Dot(transform.forward, toVec);
        Debug.Log(dot * Mathf.Rad2Deg);
    }
}
