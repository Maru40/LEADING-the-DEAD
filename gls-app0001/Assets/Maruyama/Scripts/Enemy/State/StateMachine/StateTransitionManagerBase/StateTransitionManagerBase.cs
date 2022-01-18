using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public abstract class StateTransitionManagerBase<EnemyType, TransitionParametorType>
    where EnemyType : class
{
    private EnemyType m_owner;
    protected EnemyType GetOwner() => m_owner;

    private Action m_transitionAction = null; //遷移時に呼び出したい関数
    protected Action TransitionAction => m_transitionAction;

    public StateTransitionManagerBase(EnemyType owner, Action transitionAction)
    {
        m_owner = owner;
        m_transitionAction = transitionAction;
    }

    public abstract bool IsTransition(TransitionParametorType parametor);
}
