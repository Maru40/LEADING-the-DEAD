using UnityEngine;
using UnityEngine.UI;

public class HPGauge : Gauge
{
    [SerializeField]
    private Image m_redGaugeImage;

    [SerializeField, Min(0.0f)]
    private float m_redTime = 0.5f;

    [SerializeField]
    private float m_leapTime = 0.1f;

    private float m_damageFillAmount = 0;

    private float m_beforeFillAmount = 0;

    private float m_countTime = 0.0f;

    private bool m_isDamaging = false;

    // Start is called before the first frame update
    void Start()
    {
        m_beforeFillAmount = fillAmount;
    }

    private void LateUpdate()
    {
        DamagingCheck();

        DamagingProcess();
    }

    private void DamagingCheck()
    {
        if (fillAmount < m_beforeFillAmount)
        {
            if (!m_isDamaging)
            {
                m_damageFillAmount = m_beforeFillAmount;
            }

            m_isDamaging = true;

            m_countTime = 0.0f;
        }

        m_beforeFillAmount = fillAmount;
    }

    private void DamagingProcess()
    {
        if(!m_isDamaging)
        {
            return;
        }

        m_countTime += Time.deltaTime;

        if (m_countTime < m_redTime)
        {
            return;
        }

        float nowLeapTime = m_countTime - m_redTime;

        nowLeapTime = Mathf.Min(nowLeapTime, m_leapTime);

        float leapValue = nowLeapTime / m_leapTime ;

        m_redGaugeImage.fillAmount = Mathf.SmoothStep(m_damageFillAmount, fillAmount, leapValue);

        if(nowLeapTime == m_leapTime)
        {
            m_isDamaging = false;
        }
    }
}
