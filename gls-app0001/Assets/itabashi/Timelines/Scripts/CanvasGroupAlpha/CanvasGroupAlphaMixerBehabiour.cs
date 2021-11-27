using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Timelines.Playables.CanvasGroupAlpha
{
    public class CanvasGroupAlphaMixerBehabiour : PlayableBehaviour
    {
        public TimelineClip[] Clips { get; set; }
        public PlayableDirector Director { get; set; }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var canvasGroup = playerData as CanvasGroup;

            if(!canvasGroup)
            {
                return;
            }

            double time = Director.time;
            float alpha = 0;

            bool isNoneClip = true;

            for (int i = 0; i < Clips.Length; ++i)
            {
                var clip = Clips[i];
                var clipAsset = clip.asset as CanvasGroupAlphaClip;
                var clipWeight = playable.GetInputWeight(i);

                if(clipWeight == 0.0f)
                {
                    continue;
                }

                isNoneClip = false;

                var clipProgress = (float)((time - clip.start) / clip.duration);

                if (clipProgress >= 0.0f && clipProgress <= 1.0f)
                {
                    alpha += clipAsset.behaviour.alphaCurve.Evaluate(clipProgress) * clipWeight;
                }
            }

            canvasGroup.alpha = isNoneClip ? canvasGroup.alpha : alpha;
        }
    }
}