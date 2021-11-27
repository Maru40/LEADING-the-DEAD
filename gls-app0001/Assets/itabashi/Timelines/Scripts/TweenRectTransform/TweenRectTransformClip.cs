using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.TweenRectTransform
{
    public class TweenRectTransformClip : PlayableAsset,ITimelineClipAsset
    {
        public TweenRectTransformBehaviour behaviour = new TweenRectTransformBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.SpeedMultiplier;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<TweenRectTransformBehaviour>.Create(graph);
        }
    }
}