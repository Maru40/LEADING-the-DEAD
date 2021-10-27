using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;


public class SelectSliderUI : MonoBehaviour
{
    [SerializeField]
    private Button m_button = null;

    [SerializeField]
    private Slider m_slider = null;

    private void Awake()
    {
        m_button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                GameFocusManager.PushFocus(m_slider.gameObject);
            })
            .AddTo(this);
    }
}
