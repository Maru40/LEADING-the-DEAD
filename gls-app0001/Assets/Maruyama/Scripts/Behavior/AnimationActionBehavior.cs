using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Animations;

public class AnimationActionBehavior : StateMachineBehaviour
{
    //遷移完了時に呼んで欲しいActionが呼ばれたかどうか
    bool m_isFirstInTransition;

    Action m_enterAction;
    Action m_exitAction;
    //アップデート時に呼んで欲しいイベント
    Action m_updateAction;
    //初めて遷移が完了したタイミングで呼んで欲しいエベント
    Action m_firstTransitionAciton = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Resete();

        m_enterAction?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!animator.IsInTransition(layerIndex)) //遷移が完了したら。
        {
            if(m_isFirstInTransition)  //遷移完了が初めてだったら。
            {
                m_firstTransitionAciton?.Invoke();
                m_isFirstInTransition = false;
            }
        }

        m_updateAction?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);

        m_exitAction?.Invoke();
    }

    void Resete()
    {
        m_isFirstInTransition = true;
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
