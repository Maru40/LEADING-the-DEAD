using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private void LateUpdate()
    {
        var camera = Camera.main;
        if (camera)
        {
            transform.rotation = camera.transform.rotation;
        }
    }
        
}
