using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.ConditionLoop
{
    [System.Serializable]
    public class ConditionLoopClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps { get; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ConditionLoopBehaviour>.Create(graph);
            ConditionLoopBehaviour behaviour = playable.GetBehaviour();

            return playable;
        }
    }
}