using UnityEngine.Playables;

namespace Timelines.Playables.UIInputToSkip
{
    public class UIInputToSkipBehaviour : PlayableBehaviour
    {
        private PlayableDirector m_director;

        public override void OnPlayableCreate(Playable playable)
        {
            m_director = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var skipBase = playerData as TimelineSkipBase;

            if(skipBase == null)
            {
                return;
            }

            if (skipBase.IsSkip())
            {
                var diff = playable.GetDuration() - playable.GetTime();
                m_director.time += diff;
            }
        }
    }
}