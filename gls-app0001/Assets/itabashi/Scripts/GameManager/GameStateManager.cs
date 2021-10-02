using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Play,
    Pause,
    Clear,
    GameOver
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    private GameState m_gameState = GameState.Play;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangePause()
    {
        if(m_gameState != GameState.Play && m_gameState != GameState.Pause)
        {
            Debug.Log("今はポーズにすることはできません");
            return;
        }

        if(m_gameState == GameState.Play)
        {
            Debug.Log("ポーズ状態");
            m_gameState = GameState.Pause;
            GameTimeManager.Pause();
            return;
        }

        if(m_gameState == GameState.Pause)
        {
            Debug.Log("ポーズ解除");
            m_gameState = GameState.Play;
            GameTimeManager.UnPause();
            return;
        }
    }

    public void Clear()
    {
        m_gameState = GameState.Clear;

        m_gameClearEvent?.Invoke();
    }

    public void GameOver()
    {
        m_gameState = GameState.GameOver;

        m_gameOverEvent?.Invoke();
    }
}
