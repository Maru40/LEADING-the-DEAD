using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace Manager
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance { private set; get; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(UnloadEnumerator(sceneName));
        }

        private IEnumerator UnloadEnumerator(string sceneName)
        {
            var activeScene = SceneManager.GetActiveScene();
            var asyncLoad = SceneManager.UnloadSceneAsync(activeScene);

            asyncLoad.completed += _ => StartCoroutine(LoadEnumerator(sceneName));

            yield return asyncLoad;
        }

        private IEnumerator LoadEnumerator(string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            asyncLoad.completed += _ => SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            yield return asyncLoad;
        }
    }
}