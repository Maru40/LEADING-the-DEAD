using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.Itabashi
{
    [TrackBindingType(typeof(TimelineLoopBase))]
    [TrackColor(255.0f / 255.0f, 20.0f / 255.0f, 147.0f / 255.0f)]
    [TrackClipType(typeof(ConditionLoop.ConditionLoopClip))]
    public class ConditionLoopTrack : TrackAsset
    {
    }
}