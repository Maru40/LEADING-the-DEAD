using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Timelines.Playables.ConditionLoop
{
    public class ConditionLoopBehaviour : PlayableBehaviour
    {
        private PlayableDirector m_director;

        private TimelineLoopBase m_loopBase;

        public override void OnPlayableCreate(Playable playable)
        {
            m_director = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            m_loopBase = playerData as TimelineLoopBase;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!m_loopBase || !m_loopBase.IsLoop())
            {
                return;
            }

            m_director.time -= playable.GetDuration();
        }
    }
}