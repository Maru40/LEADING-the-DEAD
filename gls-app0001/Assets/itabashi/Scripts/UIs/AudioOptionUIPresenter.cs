using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Manager;

public class AudioOptionUIPresenter : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_seTestClip;

    [SerializeField]
    private Slider m_bgmSlider = null;

    [SerializeField]
    private Slider m_seSlider = null;

    [SerializeField]
    private Button m_backButton = null;

    private void Awake()
    {
        m_bgmSlider.value = GameAudioManager.Instance.BGMVolume;

        m_seSlider.value = GameAudioManager.Instance.SEVolume;

        m_bgmSlider.onValueChanged.AsObservable()
            .Subscribe(value =>
            {
                GameAudioManager.Instance.BGMVolume = value / m_bgmSlider.maxValue;
                GameAudioManager.Instance.SEPlayOneShot(m_seTestClip);
            })
            .AddTo(this);

        m_seSlider.onValueChanged.AsObservable()
            .Subscribe(value =>
            {
                GameAudioManager.Instance.SEVolume = value / m_seSlider.maxValue;
                GameAudioManager.Instance.SEPlayOneShot(m_seTestClip);
            })
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
