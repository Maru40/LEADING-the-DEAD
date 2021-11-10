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
        m_overLookingCamera.gameObject.SetActive(true);
    }

    private void Start()
    {
        Debug.Log(Manager.GameCameraManager.current);
    }

    public void ChangeCamera()
    {
        m_overLookingCamera.gameObject.SetActive(!m_overLookingCamera.gameObject.activeInHierarchy);
        //if (m_mainCamera == Manager.GameCameraManager.current)
        //{
        //    Manager.GameCameraManager.current = m_overLookingCamera;
        //}
        //else
        //{
        //    Manager.GameCameraManager.current = m_mainCamera;
        //}
    }

}
