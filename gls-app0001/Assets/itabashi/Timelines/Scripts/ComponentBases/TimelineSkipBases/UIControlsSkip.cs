using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlsSkip : TimelineSkipBase
{
    private UIControls m_uiControls;

    private void Awake()
    {
        RegisterUIControls();
    }

    private void RegisterUIControls()
    {
        if(m_uiControls != null)
        {
            return;
        }

        m_uiControls = new UIControls();
        this.RegisterController(m_uiControls);
    }

    public override bool IsSkip()
    {
        RegisterUIControls();

        return m_uiControls.UI.Submit.IsPressed();
    }
}
