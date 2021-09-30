using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    private Image m_gaugeFillImage;

    [SerializeField]
    private Image m_backGroundImage;

    [Range(0.0f,1.0f)]
    [SerializeField]
    private float m_gaugeFillAmount = 1.0f;

    public float fillAmount
    {
        set
        {
            m_gaugeFillAmount = Mathf.Clamp(value, 0.0f, 1.0f);
            if(m_gaugeFillImage)
            {
                m_gaugeFillImage.fillAmount = m_gaugeFillAmount;
            }
            
        }

        get { return m_gaugeFillAmount; }
    }

    private void OnValidate()
    {
        fillAmount = m_gaugeFillAmount;
    }
}
