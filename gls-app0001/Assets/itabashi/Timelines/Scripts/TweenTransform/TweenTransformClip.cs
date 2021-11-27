using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.TweenTransform
{
    public class TweenTransformClip : PlayableAsset, ITimelineClipAsset
    {
        public TweenTransformBehaviour behaviour = new TweenTransformBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.SpeedMultiplier;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<TweenTransformBehaviour>.Create(graph);
        }
    }
}