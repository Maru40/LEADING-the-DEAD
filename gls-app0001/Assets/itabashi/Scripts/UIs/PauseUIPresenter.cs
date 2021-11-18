using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PauseUIPresenter : MonoBehaviour
{
    [SerializeField]
    private FocusChangeToPush m_focusChangeToPush;

    private void Awake()
    {
        GameTimeManager.isPauseOnChanged
            .Where(isPause => isPause)
            .Subscribe(_ => m_focusChangeToPush.NextPushFocus())
            .AddTo(this);

        GameTimeManager.isPauseOnChanged
            .Where(isPause => !isPause)
            .Subscribe(_ =>
            {
                m_focusChangeToPush.pushActiveObject.SetActive(false);
                GameFocusManager.PopFocus();
            })
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
