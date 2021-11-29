using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public class SelectDirectionAnimatorManager : MonoBehaviour
{
    [SerializeField]
    private Image m_leftImage;

    [SerializeField]
    private Image m_rightImage;

    [SerializeField, Min(0.0f)]
    private float m_fadeSecond = 0.1f;

    private bool m_leftHide = false;
    private bool m_rightHide = false;

    private void Awake()
    {
        m_leftHide = m_leftImage.color.a == 0;
        m_rightHide = m_rightImage.color.a == 0;
    }

    private void Start()
    {
        ColorReset();
    }

    public void ColorReset()
    {
        m_leftHide = !GameStageManager.Instance.CanDecrement();
        m_rightHide = !GameStageManager.Instance.CanIncrement();

        var color = m_leftImage.color;
        m_leftImage.color = new Color(color.r, color.g, color.b, m_leftHide ? 0.0f : 1.0f);
        color = m_rightImage.color;
        m_rightImage.color = new Color(color.r, color.g, color.b, m_rightHide ? 0.0f : 1.0f);
    }

    public void OnStageChanged()
    {
        if(!GameStageManager.Instance.CanDecrement())
        {
            m_leftHide = true;
            StartCoroutine(Fadeout(m_leftImage));
            return;
        }

        if(!GameStageManager.Instance.CanIncrement())
        {

            m_rightHide = true;
            StartCoroutine(Fadeout(m_rightImage));
            return;
        }

        if(m_leftHide)
        {
            m_leftHide = false;
            StartCoroutine(FadeIn(m_leftImage));
        }

        if(m_rightHide)
        {
            m_rightHide = false;
            StartCoroutine(FadeIn(m_rightImage));
        }
    }

    private IEnumerator Fadeout(Image image)
    {
        float alpha = 1.0f;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime / m_fadeSecond;
            alpha = Mathf.Max(alpha, 0.0f);
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeIn(Image image)
    {
        float alpha = 0.0f;

        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime / m_fadeSecond;
            alpha = Mathf.Min(alpha, 1.0f);
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
}
