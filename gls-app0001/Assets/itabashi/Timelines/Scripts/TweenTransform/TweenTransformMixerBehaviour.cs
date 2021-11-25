using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.TweenTransform
{
    public class TweenTransformMixerBehaviour : PlayableBehaviour
    {
        public TimelineClip[] Clips { get; set; }
        public PlayableDirector Director { get; set; }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var transform = playerData as Transform;

            if (!transform)
            {
                return;
            }

            double time = Director.time;
            Vector3 localPosition = Vector3.zero;
            Vector3 localEulerAngles = Vector3.zero;
            Vector3 localScale = Vector3.zero;

            for (int i = 0; i < Clips.Length; ++i)
            {
                var clip = Clips[i];
                var clipAsset = clip.asset as TweenTransformClip;
                var behaviour = clipAsset.behaviour;
                var clipWeight = playable.GetInputWeight(i);
                var clipProgress = (float)((time - clip.start) / clip.duration);

                if (clipProgress >= 0.0f && clipProgress <= 1.0f)
                {
                    localPosition += (behaviour.animatePosition ? behaviour.positionCurve.GetValueToTime(clipProgress,transform.localPosition) : transform.localPosition) * clipWeight;
                    localEulerAngles += (behaviour.animateRotation ? behaviour.rotationCurve.GetValueToTime(clipProgress,transform.localEulerAngles) : transform.localEulerAngles) * clipWeight;
                    localScale += (behaviour.animatePosition ? behaviour.scaleCurve.GetValueToTime(clipProgress,transform.localScale) : transform.localScale) * clipWeight;
                }
            }

            transform.localPosition = localPosition;
            transform.localEulerAngles = localEulerAngles;
            transform.localScale = localScale;
        }
    }
}