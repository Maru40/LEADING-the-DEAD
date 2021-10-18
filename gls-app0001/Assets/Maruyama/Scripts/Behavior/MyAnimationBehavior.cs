using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;

public class MyAnimationBehavior : StateMachineBehaviour
{
    [Serializable]
    class AnimatorTimerParametor
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
    List<AnimatorTimerParametor> m_timerParam = new List<AnimatorTimerParametor>();
    float m_beforeTime = 0.0f;

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
        var animeTime = stateInfo.normalizedTime % 1.0f;

        PlayAnimationEvent(animeTime);

        if(m_beforeTime > animeTime) //前フレームの方が小さかったら再生が最初に戻る。
        {
            ReturnAnimation();    
        }
        m_beforeTime = animeTime;
    }

    /// <summary>
    /// アニメーションのイベント再生
    /// </summary>
    /// <param name="time">現在の再生時間</param>
    void PlayAnimationEvent(float time)
    {
        foreach(var param in m_timerParam)
        {
            if (param.isEnd) {
                continue;
            }

            if(param.time <= time) {
                param.eventFunc?.Invoke();
                param.isEnd = true;
            }
        }
    }

    /// <summary>
    /// アニメーションのループが入った時
    /// </summary>
    void ReturnAnimation()
    {
        foreach(var param in m_timerParam)
        {
            param.isEnd = false;
        }
    }
}
