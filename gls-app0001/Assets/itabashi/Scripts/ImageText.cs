using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageText : MonoBehaviour
{
    [SerializeField]
    private Image m_backGround;

    [SerializeField]
    private Text m_text;

    public Image backGround { get { return m_backGround; } }

    public Text text { get { return m_text; } }

    private void UpdateActive(bool isActive)
    {
        m_backGround.gameObject.SetActive(isActive);
        m_text.gameObject.SetActive(isActive);
    }

    private void OnEnable()
    {
        UpdateActive(true);
    }

    private void OnDisable()
    {
        UpdateActive(false);
    }
}
