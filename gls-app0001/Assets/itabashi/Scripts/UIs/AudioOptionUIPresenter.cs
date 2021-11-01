using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AudioOptionUIPresenter : MonoBehaviour
{
    [SerializeField]
    private Slider m_bgmSlider = null;

    [SerializeField]
    private Slider m_seSlider = null;

    [SerializeField]
    private Button m_backButton = null;

    [SerializeField]
    private AudioOptioner m_audioOptioner = null;

    private void Awake()
    {
        m_bgmSlider.onValueChanged.AsObservable()
            .Subscribe(value => m_audioOptioner.BGMVolume = value / m_bgmSlider.maxValue)
            .AddTo(this);

        m_seSlider.onValueChanged.AsObservable()
            .Subscribe(value => m_audioOptioner.SEVolume = value / m_seSlider.maxValue)
            .AddTo(this);

        m_backButton.onClick.AsObservable()
            .Subscribe(_ =>
            {
                GameFocusManager.PopFocus();
                gameObject.SetActive(false);
            })
            .AddTo(this);
    }

    public void IsFocus()
    {
        GameFocusManager.PushFocus(m_bgmSlider.gameObject);
    }
}
