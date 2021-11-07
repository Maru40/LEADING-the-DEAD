using UnityEngine;
using Manager;

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
        GameSceneManager.Instance.LoadScene(sceneObject);
    }
}
