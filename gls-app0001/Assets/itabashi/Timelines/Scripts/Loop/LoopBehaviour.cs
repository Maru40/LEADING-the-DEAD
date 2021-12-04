using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Timelines.Playables.Loop
{
    public class LoopBehaviour : PlayableBehaviour
    {
        private PlayableDirector m_director;

        private double m_startTime = 0.0f;

        public override void OnPlayableCreate(Playable playable)
        {
            m_director = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            m_startTime = m_director.time;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (m_director)
            {
                m_director.time = m_startTime;
            }
        }
    }
}