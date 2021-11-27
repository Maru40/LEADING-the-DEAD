using UnityEngine.Timeline;
using Timelines.Playables.UIInputToSkip;

namespace Timelines.Playables.Itabashi
{
    [TrackBindingType(typeof(TimelineSkipBase))]
    [TrackColor(255.0f/255.0f, 20.0f/255.0f, 147.0f/255.0f)]
    [TrackClipType(typeof(UIInputToSkipClip))]
    public class UIInputToSkipTrack : TrackAsset
    {
    }
}