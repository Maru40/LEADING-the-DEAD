using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timelines.Playables.TweenTransform
{
    public class TweenTransformMixerBehaviour : PlayableBehaviour
    {
        private class AnimateTransformData
        {
            public Vector3 position;
            public Vector3 eulerAngles;
            public Vector3 scale;

            public AnimateTransformData() :
                this(Vector3.zero, Vector3.zero, Vector3.zero)
            {

            }

            public AnimateTransformData(Transform transform) :
                this(transform.localPosition, transform.localEulerAngles, transform.localScale)
            {

            }

            public AnimateTransformData(Vector3 position, Vector3 eulerAngles, Vector3 scale)
            {
                this.position = position;
                this.eulerAngles = eulerAngles;
                this.scale = scale;
            }

            public static AnimateTransformData operator -(in AnimateTransformData data1, in AnimateTransformData data2)
            {
                return new AnimateTransformData(data1.position - data2.position, data1.eulerAngles - data2.eulerAngles, data1.scale - data2.scale);
            }
        }

        public TimelineClip[] Clips { get; set; }
        public PlayableDirector Director { get; set; }


        private AnimateTransformData m_beforeRelativeData = new AnimateTransformData();

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var transform = playerData as Transform;

            if (!transform || Clips.Length == 0)
            {
                m_beforeRelativeData = new AnimateTransformData();
                return;
            }

            double time = Director.time;

            var track = Clips[0].parentTrack as Itabashi.TweenTransformTrack;

            if(track == null)
            {
                return;
            }

            bool isAbsolute = track.animationType == Itabashi.TweenTransformTrack.AnimationType.Absolute;

            Vector3 relativePosition = Vector3.zero;
            Vector3 relativeEulerAngles = Vector3.zero;
            Vector3 relativeScale = Vector3.zero;

            for (int i = 0; i < Clips.Length; ++i)
            {
                var clip = Clips[i];
                var clipAsset = clip.asset as TweenTransformClip;
                var behaviour = clipAsset.behaviour;
                var clipWeight = playable.GetInputWeight(i);

                if(clipWeight == 0.0f)
                {
                    continue;
                }

                var clipProgress = (float)((time - clip.start) / clip.duration);

                var defaultData = isAbsolute ? new AnimateTransformData(transform) : new AnimateTransformData();

                if (clipProgress >= 0.0f && clipProgress <= 1.0f)
                {
                    relativePosition += (behaviour.animatePosition ? behaviour.positionCurve.GetValueToTime(clipProgress,defaultData.position) : defaultData.position) * clipWeight;
                    relativeEulerAngles += (behaviour.animateRotation ? behaviour.rotationCurve.GetValueToTime(clipProgress,defaultData.eulerAngles) : defaultData.eulerAngles) * clipWeight;
                    relativeScale += (behaviour.animateScale ? behaviour.scaleCurve.GetValueToTime(clipProgress,defaultData.scale) : defaultData.scale) * clipWeight;
                }
            }

            if(isAbsolute)
            {
                transform.localPosition = relativePosition;
                transform.localEulerAngles = relativeEulerAngles;
                transform.localScale = relativeScale;
            }
            else
            {
                transform.Translate(relativePosition - m_beforeRelativeData.position);
                transform.Rotate(-m_beforeRelativeData.eulerAngles);
                transform.Rotate(relativeEulerAngles);
                transform.localScale += relativeScale - m_beforeRelativeData.scale;

                m_beforeRelativeData = new AnimateTransformData(relativePosition, relativeEulerAngles, relativeScale);
            }
        }
    }
}