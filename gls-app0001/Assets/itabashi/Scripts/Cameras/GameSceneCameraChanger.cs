using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameSceneCameraChanger : MonoBehaviour
{
    [SerializeField]
    private Camera m_mainCamera = null;
    [SerializeField]
    private CinemachineBlendListCamera m_overLookingListCamera = null;

    private void Awake()
    {
        m_overLookingListCamera.gameObject.SetActive(true);
    }

    private void Start()
    {
        Debug.Log(Manager.GameCameraManager.current);
    }

    public void StartGameCamera()
    {
        m_overLookingListCamera.gameObject.SetActive(false);
    }

    public void ChangeCamera()
    {
        m_overLookingListCamera.gameObject.SetActive(!m_overLookingListCamera.gameObject.activeInHierarchy);
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
