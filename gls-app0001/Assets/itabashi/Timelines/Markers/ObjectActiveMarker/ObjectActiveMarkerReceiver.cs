using UnityEngine;
using UnityEngine.Playables;

public class ObjectActiveMarkerReceiver : MarkerReceiverBase
{
    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        Debug.Log(gameObject.activeSelf);

        if(notification is ObjectActiveMarker marker)
        {
            gameObject.SetActive(marker.isActive);
            Debug.Log($"{gameObject.name}が{marker.isActive}になりました");
        }
    }
}
