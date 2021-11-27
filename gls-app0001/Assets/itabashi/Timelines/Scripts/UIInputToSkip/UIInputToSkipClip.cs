using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace Timelines.Playables.UIInputToSkip
{
    public class UIInputToSkipClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps { get; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<UIInputToSkipBehaviour>.Create(graph);
        }
    }
}
