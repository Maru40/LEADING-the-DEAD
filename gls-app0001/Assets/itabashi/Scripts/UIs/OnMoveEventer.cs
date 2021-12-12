using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMoveEventer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<AxisEventData> m_OnMoveEvent;

    public void OnMove(BaseEventData baseEventData)
    {
        if(baseEventData is AxisEventData axisEventData)
        {
            m_OnMoveEvent?.Invoke(axisEventData);
        }
    }
}
