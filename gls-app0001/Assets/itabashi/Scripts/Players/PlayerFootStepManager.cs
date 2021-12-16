using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerFootStepManager : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private FootStepSounder m_footStepSounder;

        [SerializeField]
        private float[] m_walkStepSoundSeconds;

        [SerializeField]
        private float[] m_dashStepSoundSeconds;

        private void Awake()
        {
            var moveTable = PlayerMotionsTable.BaseLayer.Move;

            var walkBehaviour = moveTable.Walk.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

            foreach(var walkStepSoundSecond in m_walkStepSoundSeconds)
            {
                walkBehaviour.onTimeEvent
                    .ClampWhere(walkStepSoundSecond)
                    .Subscribe(_ => m_footStepSounder.SoundPlay())
                    .AddTo(this);
            }

            var dashBehaviour = moveTable.Dash.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

            foreach(var dashStepSoundSecond in m_dashStepSoundSeconds)
            {
                dashBehaviour.onTimeEvent
                    .ClampWhere(dashStepSoundSecond)
                    .Subscribe(_ => {
                        m_footStepSounder.SoundPlay();
                        //Debug.Log($"before : {_.before}, after : {_.after}");
                        })
                    .AddTo(this);
            }
        }
    }
}
