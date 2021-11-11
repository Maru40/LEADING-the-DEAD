using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class AnimationActionBehavior : StateMachineBehaviour
{
    //遷移完了時に呼んで欲しいActionが呼ばれたかどうか
    bool m_isFirstInTransition;

    //アップデート時に呼んで欲しいイベント
    Action m_updateAction;
    //初めて遷移が完了したタイミングで呼んで欲しいエベント
    Action m_firstTransitionAciton = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Resete();
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

    void Resete()
    {
        m_isFirstInTransition = true;
    }

    //アクセッサ・プロパティ---------------------------------------------------------------------------

    /// <summary>
    /// Update時に呼びたいAction
    /// </summary>
    /// <param name="action">アクション</param>
    public void AddUpdateAction(Action action)
    {
        m_updateAction += action;
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
