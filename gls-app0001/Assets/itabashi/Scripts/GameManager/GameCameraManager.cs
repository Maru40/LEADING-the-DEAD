using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameCameraManager : MonoBehaviour
    {
        private static Camera m_currentCamera = null;

        public static Camera current
        {
            set
            {
                if(current == value)
                {
                    return;
                }

                if (current != null)
                {
                    m_currentCamera.enabled = false;
                }

                m_currentCamera = value;

                if (m_currentCamera != null)
                {
                    m_currentCamera.enabled = true;
                }
            }

            get
            {
                if (m_currentCamera == null)
                {
                    m_currentCamera = Camera.main;
                }

                return m_currentCamera;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            SceneManager.activeSceneChanged += (_1, _2) => m_currentCamera = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}