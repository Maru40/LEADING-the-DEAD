using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Manager;

public class GameSceneTransitioner : MonoBehaviour
{
    [SerializeField]
    private SceneObject m_stageSelectScene;

    public void Retry()
    {
        GameSceneManager.Instance.AddSceneChangedOneEvent(() => GameTimeManager.UnPause());

        GameSceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoNextScene()
    {
        if(!GameStageManager.Instance.CanIncrement())
        {
            return;
        }

        GameStageManager.Instance.Increment();

        GameSceneManager.Instance.LoadScene(GameStageManager.Instance.currentStageData.sceneObject);
    }

    public void BackSelectScene()
    {
        GameSceneManager.Instance.AddSceneChangedOneEvent(() => GameTimeManager.UnPause());

        GameSceneManager.Instance.LoadScene(m_stageSelectScene);
    }
}
