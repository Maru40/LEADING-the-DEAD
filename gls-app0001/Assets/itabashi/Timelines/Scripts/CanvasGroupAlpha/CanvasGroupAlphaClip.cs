using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.CanvasGroupAlpha
{
    public class CanvasGroupAlphaClip : PlayableAsset, ITimelineClipAsset
    {
        public CanvasGroupAlphaBehabiour behaviour = new CanvasGroupAlphaBehabiour();

        public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.SpeedMultiplier;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CanvasGroupAlphaBehabiour>.Create(graph);
        }
    }
}