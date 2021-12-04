using UnityEngine;
using UnityEngine.Playables;

public abstract class MarkerReceiverBase : MonoBehaviour, INotificationReceiver
{
    public abstract void OnNotify(Playable origin, INotification notification, object context);
}
