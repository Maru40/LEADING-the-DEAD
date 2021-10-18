using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;

public class MyAnimationBehavior : StateMachineBehaviour
{
    [Serializable]
    struct AnimatorTimerParametor
    {
        public float time;
        public UnityEvent eventFunc;
        public bool isEnd;

        public AnimatorTimerParametor(float time, UnityEvent eventFunc)
        {
            this.time = time;
            this.eventFunc = eventFunc;
            this.isEnd = false;
        }
    }

    [SerializeField]
    UnityEvent m_enterEvent = null;

    [SerializeField]
    UnityEvent m_exitEvent = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_enterEvent?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_exitEvent?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
