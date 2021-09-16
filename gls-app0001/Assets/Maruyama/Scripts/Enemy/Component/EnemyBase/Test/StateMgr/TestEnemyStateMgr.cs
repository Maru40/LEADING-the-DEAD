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
        //�����o�֐��ɓ����ꍇ
        m_stateMachine.AddEdge(TestEnemyState.From, TestEnemyState.To , ToTransitionTrigger);

        //�����_���g�p����ꍇ
        m_stateMachine.AddEdge(TestEnemyState.To, TestEnemyState.From,  (TestEnemyTransitionMember member) => { return member.fromTrigger.Get(); });
    }

    //�A�N�Z�b�T-----------------------------------------------------------------------------

    /// <summary>
    /// �X�e�[�g�}�V���̎擾
    /// </summary>
    /// <returns>�X�e�[�g�}�V��</returns>
    public TEStateMachine GetStateMachine()
    {
        return m_stateMachine;
    }

    /// <summary>
    /// To�ɑJ�ڂ������
    /// </summary>
    /// <param name="member">�J�ڏ����̃����o</param>
    /// <returns>Trigger��On�Ȃ�true</returns>
    bool ToTransitionTrigger(TestEnemyTransitionMember member) { return member.toTrigger.Get(); }

    /// <summary>
    /// From�ɑJ�ڂ������
    /// </summary>
    /// <param name="member">�J�ڏ����̃����o</param>
    /// <returns>Trigger��On�Ȃ�true</returns>
    bool FromTransitionTrigger(TestEnemyTransitionMember member) { return member.fromTrigger.Get(); }
}
