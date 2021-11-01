using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectHelperUI : Selectable
{
    [SerializeField]
    private Selectable m_manageSelectable;

    public override void OnMove(AxisEventData eventData) => m_manageSelectable.OnMove(eventData);
}
