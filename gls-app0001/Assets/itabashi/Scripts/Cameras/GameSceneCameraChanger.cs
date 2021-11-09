using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCameraChanger : MonoBehaviour
{
    [SerializeField]
    private Camera m_mainCamera = null;
    [SerializeField]
    private Camera m_overLookingCamera = null;

    private void Awake()
    {
        Manager.GameCameraManager.current = m_overLookingCamera;
    }

    private void Start()
    {
        Debug.Log(Manager.GameCameraManager.current);
    }

    public void ChangeCamera()
    {
        if (m_mainCamera.enabled)
        {
            Manager.GameCameraManager.current = m_overLookingCamera;
        }
        else
        {
            Manager.GameCameraManager.current = m_mainCamera;
        }
    }

}
