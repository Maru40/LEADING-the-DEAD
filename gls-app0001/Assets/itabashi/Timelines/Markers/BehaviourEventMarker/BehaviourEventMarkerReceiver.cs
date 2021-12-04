using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class BehaviourEventMarkerReceiver : MarkerReceiverBase
{
    [SerializeField]
    private UnityEvent m_markerEvent;

    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        if(notification is BehaviourEventMarker marker)
        {
            m_markerEvent?.Invoke();
        }
    }
}
