using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using Cinemachine;

public enum GameState
{
    Play,
    Pause,
    Clear,
    GameOver,
    OverLooking
}

[System.Serializable]
class GameStateReactiveProperty : ReactiveProperty<GameState>
{
    public GameStateReactiveProperty() : base() { }

    public GameStateReactiveProperty(GameState initGameState) : base(initGameState) { }
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    private GameStateReactiveProperty m_gameState = new GameStateReactiveProperty(GameState.Play);

    [SerializeField]
    private GameObject m_overLookingCameraObject;

    public GameState gameState { private set => m_gameState.Value = value; get => m_gameState.Value; }

    public System.IObservable<GameState> OnChangedGameState => m_gameState;

    [SerializeField]
    private UnityEvent m_gameStartEvent;

    [SerializeField]
    private UnityEvent m_gameClearEvent;

    [SerializeField]
    private UnityEvent m_gameOverEvent;

    private GameControls m_gameControls;

    [SerializeField]
    private void Awake()
    {
        m_gameControls = new GameControls();
        this.RegisterController(m_gameControls);

        m_gameControls.Player.Pause.performed += context => ChangePause();

        m_gameControls.Player.Select.performed += context => EndOverLooking();

        m_gameStartEvent.AddListener(() => Debug.Log("ゲーム開始"));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePause()
    {
        if(gameState != GameState.Play && gameState != GameState.Pause)
        {
            Debug.Log("今はポーズにすることはできません");
            return;
        }

        Debug.Log(gameState);

        if(gameState == GameState.Play)
        {
            Debug.Log("ポーズ状態");
            gameState = GameState.Pause;
            GameTimeManager.Pause();
            return;
        }

        if(gameState == GameState.Pause)
        {
            Debug.Log("ポーズ解除");
            gameState = GameState.Play;
            GameTimeManager.UnPause();
            return;
        }
    }

    public void Clear()
    {
        if(gameState != GameState.Play)
        {
            return;
        }

        gameState = GameState.Clear;

        m_gameClearEvent?.Invoke();
    }

    public void GameOver()
    {
        if(gameState != GameState.Play)
        {
            return;
        }
        
        gameState = GameState.GameOver;

        m_gameOverEvent?.Invoke();
    }

    private void EndOverLooking()
    {
        if(gameState != GameState.OverLooking)
        {
            return;
        }

        gameState = GameState.Play;

        m_overLookingCameraObject.gameObject.SetActive(false);

        m_gameStartEvent?.Invoke();
    }

    public void OnOverLookingEnd()
    {
        if(gameState != GameState.OverLooking)
        {
            return;
        }

        gameState = GameState.Play;

        m_gameStartEvent?.Invoke();
    }
}
