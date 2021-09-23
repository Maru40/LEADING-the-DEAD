using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EnemyMainStateMachine<NodeType, EnumType, TransitionType>
    where NodeType : class
    where EnumType : Enum  
    where TransitionType : class, new()
{
    //�X�e�[�g�}�V��
    GraphBase<NodeType, EnumType, TransitionType> m_stateMachine;

    //�J�ڏ����p�̃����o�[
    TransitionType m_transitionStruct = new TransitionType();

    public EnemyMainStateMachine()
    {
        m_stateMachine = new GraphBase<NodeType, EnumType, TransitionType>();
    }

    /// <summary>
    /// ���ݎg���m�[�h�̃^�C�v�̎擾
    /// </summary>
    /// <returns>�m�[�h�̃^�C�v</returns>
    public EnumType GetNowType()
    {
        return m_stateMachine.GetNowType();
    }

    /// <summary>
	/// ���ݎg���m�[�h�̎擾
	/// </summary>
	/// <returns>�m�[�h</returns>
    public NodeBase<NodeType> GetNowNode()
    {
        return m_stateMachine.GetNowNode();
    }

    /// <summary>
	/// �w�肵���m�[�h�̎擾
	/// </summary>
	/// <param name="type">�w�肵���m�[�h�̃^�C�v</param>
	/// <returns>�m�[�h</returns>
    public NodeBase<NodeType> GetNode(EnumType type)
    {
        return m_stateMachine.GetNode(type);
    }

    /// <summary>
    /// �w�肵���m�[�h��template�Ŏw�肵���^�ɃL���X�g���Ă��炤�B
    /// </summary>
    /// <typeparam name="T">�L���X�g����^</typeparam>
    /// <param name="type">�m�[�h�̃^�C�v</param>
    /// <returns>�L���X�g���ꂽ�m�[�h</returns>
    public T GetNode<T>(EnumType type) where T: class
    {
        var node = m_stateMachine.GetNode(type) as T;
        return node;
    }

    /// <summary>
    /// �m�[�h�̒ǉ�
    /// </summary>
    /// <param name="type">�m�[�g�̃^�C�v</param>
    /// <param name="node">�m�[�h�̖{��</param>
    public void AddNode(EnumType type, NodeBase<NodeType> node)
    {
        m_stateMachine.AddNode(type, node);
    }

    /// <summary>
    /// �G�b�W�̒ǉ�
    /// </summary>
    /// <param name="edge">�ǉ��������G�b�W</param>
    public void AddEdge(EdgeBase<EnumType, TransitionType> edge)
    {
        m_stateMachine.AddEdge(edge);
    }

    /// <summary>
    /// �G�b�W�̒ǉ�
    /// </summary>
    /// <param name="from">���̃^�C�v</param>
    /// <param name="to">�J�ڐ�̃^�C�v</param>
    /// <param name="isTransitionFunc">�J�ڏ���</param>
    public void AddEdge(EnumType from, EnumType to, Func<TransitionType, bool> isTransitionFunc)
    {
        m_stateMachine.AddEdge(from, to, isTransitionFunc);
    }


    /// <summary>
    /// �m�[�h���󂩂ǂ����𔻒f
    /// </summary>
    /// <returns>�m�[�h�̋�Ȃ�true</returns>
    public bool IsEmpty() {
	    return m_stateMachine.IsEmpty();
    }

    /// <summary>
    /// ���Z�b�g�֐�(�ŏ��̃X�e�[�g�ɕύX)
    /// </summary>
    public void Reset()
    {
        m_stateMachine.Reset();
    }

    /// <summary>
    /// �J�ڂɗ��p����\���̂��擾����B
    /// </summary>
    /// <returns>�\���̂̎Q�Ƃ�n��</returns>
    public TransitionType GetTransitionStructMember()
    {
        return m_transitionStruct;
    }

    /// <summary>
    /// �O������Update������B(��ɂ���𗘗p����StateManager�N���X)
    /// </summary>
    public void OnUpdate()
    {
        if (IsEmpty()){
            return;
        }

        //Node��Update
        NodeUpdate();

        //�J�ڃ`�F�b�N
        TransitionCheck();

        //�g���K�[�̃��Z�b�g
        TriggerReset();
    }

    /// <summary>
    /// ���݂̃m�[�h��Update
    /// </summary>
    void NodeUpdate()
    {
        var nowNode = GetNowNode();
        nowNode.OnUpdate();
    }

    /// <summary>
    /// �J�ڂ��邩�`�F�b�N
    /// </summary>
    void TransitionCheck()
    {
        var edges = m_stateMachine.GetNowNodeEdges();
        foreach (var edge in edges)
        {
            if (edge.IsTransition(m_transitionStruct))
            {
                m_stateMachine.ChangeState(edge.GetToType());
                break;
            }
        }
    }

    /// <summary>
    /// �g���K�[�̃��Z�b�g
    /// </summary>
    void TriggerReset()
    {
        var edgesDictionary = m_stateMachine.GetEdgesDictionary();
        foreach(var edges in edgesDictionary)
        {
            foreach(var edge in edges.Value)
            {
                edge.IsTransition(m_transitionStruct);
            }
        }
    }
}
