using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Manager
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance { private set; get; }

        [SerializeField]
        private FadeObject m_defaultFadeOutObjectPrefab = null;

        [SerializeField]
        private FadeObject m_defaultFadeInObjectPrefab = null;

        [SerializeField]
        private GameObject m_loadDrawObject = null;

        [SerializeField]
        private Canvas m_canvas = null;

        [SerializeField]
        private Camera m_camera = null;

        [SerializeField]
        private UnityEvent m_sceneChangedOneEvent = new UnityEvent();

        public bool IsFeding { private set; get; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

        }

        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName, m_defaultFadeOutObjectPrefab, m_defaultFadeInObjectPrefab);
        }

        public void LoadScene(string sceneName, FadeObject fadeOutObjectPrefab, FadeObject fadeInObjectPrefab)
        {
            if (IsFeding)
            {
                Debug.LogError("遷移中に遷移はできません");
                return;
            }

            var fadeOutObject = Instantiate(fadeOutObjectPrefab, m_canvas.transform);
            var fadeInObject = Instantiate(fadeInObjectPrefab, m_canvas.transform);

            StartCoroutine(LoadEnumerator(sceneName, fadeOutObject, fadeInObject));
        }

        private IEnumerator LoadEnumerator(string sceneName, FadeObject fadeOutObject, FadeObject fadeInObject)
        {
            var unloadScene = SceneManager.GetActiveScene();

            fadeInObject.gameObject.SetActive(false);

            EventSystem.current.currentInputModule.enabled = false;

            Debug.Log("ステージ遷移開始");

            IsFeding = true;

            fadeOutObject.FadeStart();

            Debug.Log("フェードアウト開始");

            while (!fadeOutObject.IsFinish())
            {
                yield return null;
            }

            Debug.Log("フェードアウト終了");

            m_sceneChangedOneEvent?.Invoke();

            m_sceneChangedOneEvent.RemoveAllListeners();

            m_camera.gameObject.SetActive(true);

            Destroy(fadeOutObject.gameObject);

            m_loadDrawObject.SetActive(true);

            Debug.Log("ステージのアンロードを開始");

            var asyncUnload = SceneManager.UnloadSceneAsync(unloadScene);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            Debug.Log("ステージのアンロードが終了しました");

            Debug.Log("ステージのロード開始");

            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            Debug.Log("ロード準備が終わりました");

            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("ロードが終わりました");

            GameFocusManager.ClearFocus();

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            m_loadDrawObject.SetActive(false);
            fadeInObject.gameObject.SetActive(true);
            m_camera.gameObject.SetActive(false);

            Debug.Log("フェードイン開始");

            fadeInObject.FadeStart();

            while (!fadeInObject.IsFinish())
            {
                yield return null;
            }

            Debug.Log("フェードイン終了");

            Destroy(fadeInObject.gameObject);

            Debug.Log("ステージ遷移終了");

            IsFeding = false;
        }

        public void AddSceneChangedOneEvent(UnityAction changeOneEvent)
        {
            m_sceneChangedOneEvent.AddListener(changeOneEvent);
        }

    }

}