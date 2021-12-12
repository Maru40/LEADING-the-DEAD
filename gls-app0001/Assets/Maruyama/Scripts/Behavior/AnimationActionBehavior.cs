using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Animations;

public class AnimationActionBehavior : StateMachineBehaviour
{
    [Serializable]
    class AnimatorTimerParametor
    {
        public float time;
        public Action action;
        public bool isEnd;

        public AnimatorTimerParametor(float time, Action action)
        {
            this.time = time;
            this.action = action;
            this.isEnd = false;
        }
    }

    [SerializeField]
    List<AnimatorTimerParametor> m_timerParam = new List<AnimatorTimerParametor>();
    float m_beforeTime = 0.0f;

    Action m_enterAction;
    Action m_updateAction; //アップデート時に呼んで欲しいイベント
    Action m_exitAction;

    //遷移完了時に呼んで欲しいActionが呼ばれたかどうか
    bool m_isFirstInTransition;
    //初めて遷移が完了したタイミングで呼んで欲しいエベント
    Action m_firstTransitionAciton = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnterReset();

        m_enterAction?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FirstTransitionManager(animator, layerIndex);  //初期遷移時に呼び出したいイベント管理

        UpdateTimeActionManager(stateInfo);  //TimeActionの管理

        m_updateAction?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);

        m_exitAction?.Invoke();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void EnterReset()
    {
        m_isFirstInTransition = true;
    }

    /// <summary>
    /// 初期遷移管理
    /// </summary>
    /// <param name="animator"></param>
    private void FirstTransitionManager(Animator animator, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex)) //遷移が完了したら。
        {
            if (m_isFirstInTransition)  //遷移完了が初めてだったら。
            {
                m_firstTransitionAciton?.Invoke();
                m_isFirstInTransition = false;
            }
        }
    }

    /// <summary>
    /// タイムイベント管理
    /// </summary>
    /// <param name="stateInfo"></param>
    private void UpdateTimeActionManager(AnimatorStateInfo stateInfo)
    {
        if (m_isFirstInTransition) {
            return;  //遷移が完了していないなら処理を行わない
        }

        var animeTime = stateInfo.normalizedTime % 1.0f;

        PlayTimeActions(animeTime);

        if (m_beforeTime > animeTime) //前フレームの方が小さかったら再生が最初に戻る。
        {
            ReturnAnimation();
        }
        m_beforeTime = animeTime;
    }

    /// <summary>
    /// アニメーションのイベント再生
    /// </summary>
    /// <param name="time">現在の再生時間</param>
    private void PlayTimeActions(float time)
    {
        foreach (var param in m_timerParam)
        {
            if (param.isEnd) {
                continue;
            }

            if (param.time <= time)
            {
                param.action?.Invoke();
                param.isEnd = true;
            }
        }
    }

    /// <summary>
    /// アニメーションのループが入った時
    /// </summary>
    private void ReturnAnimation()
    {
        foreach (var param in m_timerParam)
        {
            param.isEnd = false;
        }
    }

    //アクセッサ・プロパティ---------------------------------------------------------------------------

    public void AddEnterAction(Action action)
    {
        m_enterAction += action;
    }

    /// <summary>
    /// Update時に呼びたいAction
    /// </summary>
    /// <param name="action">アクション</param>
    public void AddUpdateAction(Action action)
    {
        m_updateAction += action;
    }

    public void AddExitAction(Action action)
    {
        m_exitAction += action;
    }

    /// <summary>
    /// 遷移完了時に行いたいアクション
    /// </summary>
    /// <param name="action">アクション</param>
    public void AddFirstTransitionAction(Action action)
    {
        m_firstTransitionAciton += action;
    }
}
