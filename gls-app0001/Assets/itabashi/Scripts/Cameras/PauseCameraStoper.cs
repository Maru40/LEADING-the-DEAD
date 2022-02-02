using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UniRx;

public class PauseCameraStoper : MonoBehaviour
{
    [SerializeField]
    private GameStateManager m_gameStateManager;

    [SerializeField]
    private CinemachineFreeLook m_virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        m_gameStateManager.OnChangedGameState.Subscribe(gameState =>
        {
            m_virtualCamera.enabled = gameState != GameState.Pause;
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
