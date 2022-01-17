using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateNodeBase_Ex<EnemyType, EnumType, TransitionType> : EnemyStateNodeBase<EnemyType>
    where EnemyType : class
    where EnumType : System.Enum
    where TransitionType : class, new()
{
    private EnemyMainStateMachine<EnemyType, EnumType, TransitionType> m_stateMachine;

    public EnemyStateNodeBase_Ex(EnemyType owner, EnemyMainStateMachine<EnemyType, EnumType, TransitionType> stateMachine)
        :base(owner)
    {
        m_stateMachine = stateMachine;
    }

    protected EnemyMainStateMachine<EnemyType, EnumType, TransitionType> StateMachine
    {
        get => m_stateMachine;
        set => m_stateMachine = value;
    }

    protected EnemyMainStateMachine<EnemyType, EnumType, TransitionType> GetStateMachine()
    {
        return m_stateMachine;
    }
}
