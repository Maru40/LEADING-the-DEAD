using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectablesParentUI : Selectable
{

    [SerializeField]
    private Selectable m_firstSelectable;

    protected override void OnValidate()
    {
        base.OnValidate();

        if(m_firstSelectable == null || m_firstSelectable == this)
        {
            m_firstSelectable = null;
            return;
        }

        if(!transform.IsChildOf(m_firstSelectable.transform))
        {
            m_firstSelectable = null;
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        m_firstSelectable.OnSelect(eventData);
        m_firstSelectable.Select();
    }
}
