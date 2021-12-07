using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardRotate : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;

    private void LateUpdate()
    {
        transform.rotation = m_camera.transform.rotation;
    }
}
