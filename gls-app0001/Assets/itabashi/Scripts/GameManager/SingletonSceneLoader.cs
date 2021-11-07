using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SingletonSceneLoader
    {
        public const string m_loadSceneName = "SingletonManagerScene";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SingletonSceneLoad()
        {
            SceneManager.LoadScene(m_loadSceneName, LoadSceneMode.Additive);
        }
    }
}