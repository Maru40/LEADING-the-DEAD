using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueSliderUI : MonoBehaviour
{
    [SerializeField]
    private Slider m_slider = null;

    private float m_beforeValue = 0.0f;

    public void OnSelect()
    {
        m_beforeValue = m_slider.value;
    }

    public void OnDeselect()
    {
    }

    public void OnDecision()
    {
        GameFocusManager.PopFocus();
    }

    public void OnCancel()
    {
        m_slider.value = m_beforeValue;

        GameFocusManager.PopFocus();
    }
}
