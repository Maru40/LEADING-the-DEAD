using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateTableNodeBase<EnemyType> : EnemyStateNodeBase<EnemyType>
    where EnemyType : class
{
    bool m_isEnd = false;

    public StateTableNodeBase(EnemyType owner)
        : base(owner)
    { }

    protected abstract override void ReserveChangeComponents();

    public override void OnStart()
    {
        base.OnStart();
        m_isEnd = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        m_isEnd = false;
    }

    //アクセッサ-----------------------------------------------------------------------

    /// <summary>
    /// 終了したい時に呼ぶ。
    /// </summary>
    protected void End()
    {
        m_isEnd = true;
    }

    protected bool isEnd
    {
        set { m_isEnd = value; }
        get { return m_isEnd; }
    }

}
