using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable, DisplayName("ObjectActiveMarker")]
public class ObjectActiveMarker : Marker, INotification
{
    [SerializeField]
    private bool m_isActive;
    public bool isActive => m_isActive;

    public PropertyName id => new PropertyName("ObjectActiveMarker");
}
