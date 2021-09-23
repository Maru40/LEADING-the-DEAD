using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TEStateMachine = EnemyMainStateMachine<TestEnemy, TestEnemyState, TestEnemyTransitionMember>;

public enum TestEnemyState 
{
    From,
    To,
}

public class TestEnemyTransitionMember 
{
    public MyTrigger fromTrigger;
    public MyTrigger toTrigger;
}

public class TestEnemyStateMgr : MonoBehaviour
{
    TestEnemy m_enemy;

    TEStateMachine m_stateMachine = new TEStateMachine();

    void Start()
    {
        m_enemy = GetComponent<TestEnemy>();

        CreateStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.OnUpdate();

        //testKey--------------------------------------
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    m_stateMachine.GetTransitionStructMember().fromTrigger.Fire();
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    m_stateMachine.GetTransitionStructMember().toTrigger.Fire();
        //}
    }

    void CreateStateMachine()
    {
        CreateNode();
        CreateEdge();
    }

    void CreateNode()
    {
        m_stateMachine.AddNode(TestEnemyState.From, new TestFromState(m_enemy));
        m_stateMachine.AddNode(TestEnemyState.To, new TestToState(m_enemy));
    }

    void CreateEdge()
    {
        //メンバ関数に入れる場合
        m_stateMachine.AddEdge(TestEnemyState.From, TestEnemyState.To , ToTransitionTrigger);

        //ラムダを使用する場合
        m_stateMachine.AddEdge(TestEnemyState.To, TestEnemyState.From,  (TestEnemyTransitionMember member) => { return member.fromTrigger.Get(); });
    }

    //アクセッサ-----------------------------------------------------------------------------

    /// <summary>
    /// ステートマシンの取得
    /// </summary>
    /// <returns>ステートマシン</returns>
    public TEStateMachine GetStateMachine()
    {
        return m_stateMachine;
    }

    /// <summary>
    /// Toに遷移する条件
    /// </summary>
    /// <param name="member">遷移条件のメンバ</param>
    /// <returns>TriggerがOnならtrue</returns>
    bool ToTransitionTrigger(TestEnemyTransitionMember member) { return member.toTrigger.Get(); }

    /// <summary>
    /// Fromに遷移する条件
    /// </summary>
    /// <param name="member">遷移条件のメンバ</param>
    /// <returns>TriggerがOnならtrue</returns>
    bool FromTransitionTrigger(TestEnemyTransitionMember member) { return member.fromTrigger.Get(); }
}
