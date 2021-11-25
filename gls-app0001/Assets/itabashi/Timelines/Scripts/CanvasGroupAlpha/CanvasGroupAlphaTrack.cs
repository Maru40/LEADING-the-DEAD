using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Timelines.Playables.CanvasGroupAlpha;

namespace Timelines.Playables.Itabashi
{
    [TrackBindingType(typeof(CanvasGroup))]
    [TrackColor(0,1,0)]
    [TrackClipType(typeof(CanvasGroupAlphaClip))]
    public class CanvasGroupAlphaTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<CanvasGroupAlphaMixerBehabiour>.Create(graph, inputCount);
            mixer.GetBehaviour().Clips = GetClips().ToArray();
            mixer.GetBehaviour().Director = go.GetComponent<PlayableDirector>();

            return mixer;
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var trackBinding = director.GetGenericBinding(this) as CanvasGroup;
            
            if(!trackBinding)
            {
                return;
            }

            driver.AddFromName<CanvasGroup>(trackBinding.gameObject, "m_Alpha");
#endif
        }
    }
}