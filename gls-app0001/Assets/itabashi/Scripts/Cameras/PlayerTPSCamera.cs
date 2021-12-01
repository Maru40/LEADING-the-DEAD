using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTPSCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook m_freeLookCamera;

    [SerializeField]
    private float m_range = 5.0f;

    [SerializeField]
    private float m_maxRotateX = 40.0f;

    [SerializeField]
    private float m_minRotateX = -40.0f;

    private void Reset()
    {
        
    }

    private void OnValidate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        for (int i = 0; i < 3; ++i)
        {
            m_freeLookCamera.GetRig(i).m_Lens.FieldOfView = m_range;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
