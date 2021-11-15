using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Manager;

public class GameSceneTransitioner : MonoBehaviour
{
    [SerializeField]
    List<SceneObject> m_sceneObjects;

    [SerializeField]
    private SceneObject m_stageSelectScene;

    private void Update()
    {
    }

    public void Retry()
    {
        GameSceneManager.Instance.AddSceneChangedOneEvent(() => GameTimeManager.UnPause());

        GameSceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
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

    public void BackSelectScene()
    {
        GameSceneManager.Instance.AddSceneChangedOneEvent(() => GameTimeManager.UnPause());

        GameSceneManager.Instance.LoadScene(m_stageSelectScene);
    }
}
