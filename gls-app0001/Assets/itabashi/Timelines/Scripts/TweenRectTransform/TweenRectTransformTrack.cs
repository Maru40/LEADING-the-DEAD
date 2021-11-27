using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Timelines.Playables.TweenRectTransform;
using System.Linq;

namespace Timelines.Playables.Itabashi
{
    [TrackBindingType(typeof(RectTransform))]
    [TrackColor(0,1,0)]
    [TrackClipType(typeof(TweenRectTransformClip))]
    public class TweenRectTransformTrack : TrackAsset
    {
        public enum AnimationType
        {
            Absolute,
            Relative
        }

        public AnimationType animationType = AnimationType.Relative;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<TweenRectTransformMixBehaviour>.Create(graph, inputCount);
            mixer.GetBehaviour().Clips = GetClips().ToArray();
            mixer.GetBehaviour().Director = go.GetComponent<PlayableDirector>();

            return mixer;
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var trackBinding = director.GetGenericBinding(this) as RectTransform;

            if (!trackBinding)
            {
                return;
            }

            driver.AddFromName<Transform>(trackBinding.gameObject, "m_AnchoredPosition");
            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalPosition");
            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalRotation");
            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalScale");
#endif
        }

    }
}