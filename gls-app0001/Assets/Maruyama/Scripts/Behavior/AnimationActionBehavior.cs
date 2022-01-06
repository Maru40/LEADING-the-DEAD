using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Animations;

public class AnimationActionBehavior : StateMachineBehaviour
{
    [Serializable]
    public class AnimatorTimerParametor
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
    private List<AnimatorTimerParametor> m_timerParams = new List<AnimatorTimerParametor>();
    private float m_beforeTime = 0.0f;

    private Action m_enterAction;
    private Action m_updateAction; //アップデート時に呼んで欲しいイベント
    private Action m_exitAction;

    //遷移完了時に呼んで欲しいActionが呼ばれたかどうか
    private bool m_isFirstInTransition = true;
    //初めて遷移が完了したタイミングで呼んで欲しいエベント
    private Action m_firstTransitionAciton = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //EnterReset();
        m_enterAction?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FirstTransitionManager(animator, layerIndex);  //初期遷移時に呼び出したいイベント管理

        UpdateTimeActionManager(animator, stateInfo, layerIndex);  //TimeActionの管理

        m_updateAction?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);

        m_exitAction?.Invoke();

        StateReset();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void StateReset()
    {
        m_isFirstInTransition = true;
        m_beforeTime = 0;
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
                //Debug.Log("◆◆遷移初めて");
                m_firstTransitionAciton?.Invoke();
                m_isFirstInTransition = false;
            }
        }
    }

    /// <summary>
    /// タイムイベント管理
    /// </summary>
    /// <param name="stateInfo"></param>
    private void UpdateTimeActionManager(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //遷移中で初めての遷移でない場合
        if (animator.IsInTransition(layerIndex) && m_isFirstInTransition) {
            return;
        }

        var animeTime = stateInfo.normalizedTime % 1.0f * stateInfo.length * stateInfo.speed;
        //Debug.Log("◆◆" + animeTime);

        if (float.IsInfinity(animeTime)) {
            m_beforeTime = 0;
            return;
        }

        PlayTimeActions(animeTime);

        if (m_beforeTime > animeTime) //前フレームの方が小さかったら再生が最初に戻る。
        {
            ReturnAnimation();
        }
        //Debug.Log("Before: " + m_beforeTime);
        //Debug.Log("Anime: " + animeTime);
        //Debug.Log("Normal: " + stateInfo.normalizedTime);
        //Debug.Break();
        m_beforeTime = animeTime;
    }

    /// <summary>
    /// アニメーションのイベント再生
    /// </summary>
    /// <param name="time">現在の再生時間</param>
    private void PlayTimeActions(float time)
    {
        foreach (var param in m_timerParams)
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
        foreach (var param in m_timerParams)
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

    /// <summary>
    /// タイムイベントの追加
    /// </summary>
    /// <param name="time">時間</param>
    /// <param name="action">イベント</param>
    public void AddTimeAction(float time, Action action)
    {
        m_timerParams.Add(new AnimatorTimerParametor(time, action));
    }
}
