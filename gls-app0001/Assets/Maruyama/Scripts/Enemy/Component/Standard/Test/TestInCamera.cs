using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInCamera : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UtilityCamera.IsInCamera(transform.position,Camera.main))
        {
            Debug.Log("CameraIn");
        }
    }
}
