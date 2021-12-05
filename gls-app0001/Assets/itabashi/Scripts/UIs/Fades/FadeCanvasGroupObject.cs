using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvasGroupObject : FadeObject
{
    private enum FadeType
    {
        FadeOut,
        FadeIn
    }

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private float m_fadeTime = 0.25f;

    [SerializeField]
    private bool m_playOnAwake = false;

    [SerializeField]
    private FadeType m_fadeType = FadeType.FadeOut;

    private bool m_isFading = false;

    private bool m_isFinish = false;

    public override void FadeStart()
    {
        if (!m_isFading)
        {
            StartCoroutine(Fading(m_fadeTime));
        }
    }

    public override bool IsFinish() => m_isFinish;

    private IEnumerator Fading(float fadeTime)
    {
        float countTime = 0.0f;

        m_isFading = true;

        while (countTime < fadeTime)
        {
            countTime += Time.unscaledDeltaTime;

            float setAlpha = countTime / fadeTime;

            if (m_fadeType == FadeType.FadeIn)
            {
                setAlpha = 1.0f - setAlpha;
            }

            m_canvasGroup.alpha = setAlpha;

            yield return null;
        }

        float alpha = m_fadeType == FadeType.FadeOut ? 1.0f : 0.0f;

        m_canvasGroup.alpha = alpha;

        m_isFinish = true;

        m_isFading = false;
    }

    private void Reset()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        if (m_playOnAwake)
        {
            FadeStart();
        }
    }

    public void FadeReset()
    {
        m_isFading = false;
        m_isFinish = false;
        StopAllCoroutines();
    }
}
