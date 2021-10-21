using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameSceneTransitioner : MonoBehaviour
{
    [SerializeField]
    List<SceneObject> m_sceneObjects;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Exit();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoNextScene()
    {
        string nowSceneName = SceneManager.GetActiveScene().name;

        int index = m_sceneObjects.IndexOf(nowSceneName);

        if (index + 1 >= m_sceneObjects.Count)
        {
            return;
        }

        SceneManager.LoadScene(m_sceneObjects[index + 1]);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif        
    }
}
