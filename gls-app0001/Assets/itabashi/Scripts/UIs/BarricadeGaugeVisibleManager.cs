using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class BarricadeGaugeVisibleManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_canvasGroup;

    private BoolReactiveProperty m_isVisible = new BoolReactiveProperty(false);

    public bool IsVisible { set => m_isVisible.Value = value; get => m_isVisible.Value; }

    private BoolReactiveProperty m_isChanging = new BoolReactiveProperty(false);

    public bool IsChanging { set => m_isChanging.Value = value; get => m_isChanging.Value; }


    public bool IsDraw => IsVisible || IsChanging;

    private void Awake()
    {
        m_isVisible.Where(_ => IsDraw)
            .Subscribe(_ => m_canvasGroup.alpha = 1.0f)
            .AddTo(this);

        m_isVisible.Where(_ => !IsDraw)
            .Subscribe(_ => m_canvasGroup.alpha = 0.0f)
            .AddTo(this);

        m_isChanging.Where(_ => IsDraw)
            .Subscribe(_ => m_canvasGroup.alpha = 1.0f)
            .AddTo(this);

        m_isChanging.Where(_ => !IsDraw)
            .Subscribe(_ => m_canvasGroup.alpha = 0.0f)
            .AddTo(this);
    }
}
