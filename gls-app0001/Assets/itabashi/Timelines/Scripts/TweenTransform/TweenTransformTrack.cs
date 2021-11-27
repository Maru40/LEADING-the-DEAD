using UnityEngine;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Timelines.Playables.TweenTransform;

namespace Timelines.Playables.Itabashi
{
    [TrackBindingType(typeof(Transform))]
    [TrackColor(0, 1, 0)]
    [TrackClipType(typeof(TweenTransformClip))]
    public class TweenTransformTrack : TrackAsset
    {
        public enum AnimationType
        {
            Absolute,
            Relative
        }

        public AnimationType animationType = AnimationType.Relative;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<TweenTransformMixerBehaviour>.Create(graph, inputCount);
            mixer.GetBehaviour().Clips = GetClips().ToArray();
            mixer.GetBehaviour().Director = go.GetComponent<PlayableDirector>();

            return mixer;
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var trackBinding = director.GetGenericBinding(this) as Transform;

            if (!trackBinding)
            {
                return;
            }

            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalPosition");
            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalRotation");
            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalScale");
#endif
        }
    }
}