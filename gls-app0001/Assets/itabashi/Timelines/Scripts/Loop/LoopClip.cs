using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.Loop
{
    public class LoopClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps { get; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<LoopBehaviour>.Create(graph);
        }
    }
}