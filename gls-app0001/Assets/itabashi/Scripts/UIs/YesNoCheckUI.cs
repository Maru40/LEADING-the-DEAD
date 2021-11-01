using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class YesNoCheckUI : MonoBehaviour
{
    [SerializeField]
    private Button m_yesButton;

    [SerializeField]
    private Button m_noButton;

    private void Awake()
    {
        m_yesButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                GameFocusManager.PopFocus();
                gameObject.SetActive(false);
            })
            .AddTo(this);

        m_noButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                GameFocusManager.PopFocus();
                gameObject.SetActive(false);
            })
            .AddTo(this);
    }
}
