using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigateScrollView : MonoBehaviour
{
    [SerializeField]
    private ScrollRect m_scrollRect;

    [SerializeField]
    private float m_scrollSpeed = 1.0f;

    private const float SCROLL_SPEED_STANDARD = 0.001f;

    private UIControls m_uiControls;

    private void Start()
    {
        m_uiControls = new UIControls();

        this.RegisterController(m_uiControls);

        m_uiControls.UI.Cancel.performed += _ =>
        {
            GameFocusManager.PopFocus();
            gameObject.SetActive(false);
        };
    }

    public void NavigateScroll()
    {
        Vector2 moveVector = m_uiControls.UI.Stick.ReadValue<Vector2>();

        if(m_scrollRect.horizontal && Mathf.Abs(moveVector.x) > 0)
        {
            m_scrollRect.horizontalNormalizedPosition += moveVector.x * m_scrollSpeed * SCROLL_SPEED_STANDARD;
        }

        if(m_scrollRect.vertical && Mathf.Abs(moveVector.y) > 0)
        {
            m_scrollRect.verticalNormalizedPosition += moveVector.y * m_scrollSpeed * SCROLL_SPEED_STANDARD;
        }
        
    }

    private void Update()
    {
        if(!m_scrollRect)
        {
            return;
        }

        NavigateScroll();
    }
}
