using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BehaviourEventMarker : Marker, INotification
{
    public PropertyName id => new PropertyName("BehaviourEventMarker");
}
