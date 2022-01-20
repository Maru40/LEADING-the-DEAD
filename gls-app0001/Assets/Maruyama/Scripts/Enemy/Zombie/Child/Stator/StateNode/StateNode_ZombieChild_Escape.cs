using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

using StateMachine =
    EnemyMainStateMachine<EnemyBase, StateNode_ZombieChild_Escape.StateType, StateNode_ZombieChild_Escape.TransitionMember>;

public class StateNode_ZombieChild_Escape : EnemyStateNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("逃げ切ったと判断する距離")]
        public float escapeRange;
        [Header("逃げ切ってから逃げ続ける時間")]
        public float escapeLoopTime;
    }

    public enum StateType
    {
        Escape,     //逃げている状態
        Deferment,  //猶予
    }

    public struct TransitionMember
    {

    }

    private Parametor m_param = new Parametor();

    private StateMachine m_stateMachine;

    private Stator_ZombieChild m_stator = null;
    private TargetManager m_targetManager = null;

    public StateNode_ZombieChild_Escape(EnemyBase owner, Parametor parametor)
        : base(owner)
    {
        m_param = parametor;

        m_stator = owner.GetComponent<Stator_ZombieChild>();
        m_targetManager = owner.GetComponent<TargetManager>();

        m_stateMachine = new StateMachine();
        CreateNode();
        CreateEdge();
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        Debug.Log("△△ Escape");

        m_stateMachine.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        m_targetManager.SetNowTarget(GetType(), null);
    }

    private void CreateNode()
    {
        var enemy = GetOwner();

        m_stateMachine.AddNode(StateType.Escape, new Escape(enemy, new Escape.Parametor()));
        m_stateMachine.AddNode(StateType.Deferment, new Deferment(enemy, new Deferment.Parametor(m_param.escapeLoopTime)));
    }

    private void CreateEdge()
    {
        //逃げている状態
        m_stateMachine.AddEdge(StateType.Escape, StateType.Deferment, IsDeferment);

        //猶予状態
        m_stateMachine.AddEdge(StateType.Deferment, StateType.Escape, IsEscape);
    }

    //遷移条件----------------------------------------------------------------------------------------------------

    private bool IsDeferment(ref TransitionMember member)
    {
        //ターゲットが範囲外ならtrue
        return !IsTargetRange() ? true : false;
    }

    private bool IsEscape(ref TransitionMember member)
    {
        //ターゲットが範囲内にいるなら逃げ続ける
        return IsTargetRange() ? true : false;
    }

    /// <summary>
    /// ターゲットが範囲内にいるかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsTargetRange()
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        var owner = GetOwner();
        var target = m_targetManager.GetNowTarget();

        return Calculation.IsRange(owner.gameObject, target.gameObject, m_param.escapeRange) ? true : false;
    }

    //StateNode-------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 逃げる状態
    /// </summary>
    private class Escape : NodeBase<EnemyBase>
    {
        [System.Serializable]
        public struct Parametor
        {

        }

        private Parametor m_param = new Parametor();

        public Escape(EnemyBase owner, Parametor parametor)
            :base(owner)
        {
            m_param = parametor;
        }

        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }

    /// <summary>
    /// 逃げ切ったと判断してからの猶予時間
    /// </summary>
    private class Deferment : NodeBase<EnemyBase>
    {
        [System.Serializable]
        public struct Parametor
        {
            public float time;

            public Parametor(float time)
            {
                this.time = time;
            }
        }

        private Parametor m_param = new Parametor();

        private GameTimer m_timer = new GameTimer();
        private Stator_ZombieChild m_stator;

        public Deferment(EnemyBase owner, Parametor parametor)
            :base(owner)
        {
            m_param = parametor;

            m_stator = owner.GetComponent<Stator_ZombieChild>();
        }

        public override void OnStart()
        {
            m_timer.ResetTimer(m_param.time);
        }

        public override void OnUpdate()
        {
            m_timer.UpdateTimer();

            if (m_timer.IsTimeUp)
            {
                m_stator.GetTransitionMember().plowlingTrigger.Fire();
            }
        }

        public override void OnExit()
        {

        }
    }

}
