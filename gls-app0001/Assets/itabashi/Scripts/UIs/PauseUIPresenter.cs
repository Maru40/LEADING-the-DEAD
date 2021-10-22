using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PauseUIPresenter : MonoBehaviour
{
    [SerializeField]
    private PauseUIView m_pauseUIView;

    private void Awake()
    {
        GameTimeManager.isPauseOnChanged
            .Subscribe(isPause => m_pauseUIView.gameObject.SetActive(isPause))
            .AddTo(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
