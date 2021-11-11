using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class FadeImageObject : FadeObject
{
    private enum FadeType
    {
        FadeOut,
        FadeIn
    }

    [SerializeField]
    private Image m_image;

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

        while(countTime < fadeTime)
        {
            countTime += Time.deltaTime;

            float setAlpha = countTime / fadeTime;
            
            if(m_fadeType == FadeType.FadeIn)
            {
                setAlpha = 1.0f - setAlpha;
            }

            m_image.color = ChangeAlpha(m_image.color, setAlpha);

            yield return null;
        }

        float alpha = m_fadeType == FadeType.FadeOut ? 1.0f : 0.0f;

        m_image.color = ChangeAlpha(m_image.color, alpha);

        m_isFinish = true;

        m_isFading = false;
    }

    private Color ChangeAlpha(in Color color,float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    private void Reset()
    {
        m_image = GetComponent<Image>();
    }

    private void Awake()
    {
        if(m_playOnAwake)
        {
            FadeStart();
        }
    }
}
