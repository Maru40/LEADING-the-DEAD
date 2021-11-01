using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField]
    private SceneObject m_nextSceneObject;

    public void GoNextScene()
    {
        GoNextScene(m_nextSceneObject);
    }

    public void GoNextScene(SceneObject sceneObject)
    {
        SceneManager.LoadScene(sceneObject);
    }
}
