using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameCameraManager : MonoBehaviour
    {
        private static Camera m_currentCamera = null;

        private static float m_currentDepth = 10;

        private static float m_beforeDepth = 0.0f;

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
                    m_currentCamera.depth = m_beforeDepth;
                }

                m_currentCamera = value;

                if (m_currentCamera != null)
                {
                    m_beforeDepth = m_currentCamera.depth;
                    m_currentCamera.depth = m_currentDepth;
                }
            }

            get
            {
                if (m_currentCamera == null)
                {
                    m_currentCamera = Camera.main;

                    m_beforeDepth = m_currentCamera == null ? 0.0f : m_currentCamera.depth;
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