using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class TestInCamera : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CalcuCamera.IsInCamera(transform.position,Camera.main))
        {
            Debug.Log("CameraIn");
        }
    }
}
