using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GraphBase<NodeType, EnumType, TransitionType>
	where NodeType : class
	where EnumType : Enum
	where TransitionType : class
{
	//�ŏ��̃m�[�h(���Z�b�g�s�ׂɎg��)
	EnumType m_firstType;

	//���݂̃m�[�h
	EnumType m_nowNodeType;

	//�m�[�h�̘A�z�z��
	Dictionary<EnumType, NodeBase<NodeType>> m_nodes;

	//�G�b�W�̘A�z�z�񃊃X�g
	Dictionary<EnumType, List<EdgeBase<EnumType, TransitionType>>> m_edgesDictionary;

    public GraphBase()
    {
		m_nodes = new Dictionary<EnumType, NodeBase<NodeType>>();
		m_edgesDictionary = new Dictionary<EnumType, List<EdgeBase<EnumType, TransitionType>>>();
	}


	/// <summary>
	/// ���ݎg���m�[�h�̃^�C�v�̎擾
	/// </summary>
	/// <returns>�m�[�h�̃^�C�v</returns>
	public EnumType GetNowType()
	{
		return m_nowNodeType;
	}

	/// <summary>
	/// ���ݎg���m�[�h�̎擾
	/// </summary>
	/// <returns>�m�[�h</returns>
	public NodeBase<NodeType> GetNowNode()
	{
		return m_nodes[m_nowNodeType];
	}

	/// <summary>
	/// �w�肵���m�[�h�̎擾
	/// </summary>
	/// <param name="type">�w�肵���m�[�h�̃^�C�v</param>
	/// <returns>�m�[�h</returns>
	public NodeBase<NodeType> GetNode(EnumType type)
	{
		return m_nodes[type];
	}

	/// <summary>
	/// �m�[�h�̔z����擾
	/// </summary>
	/// <returns>�m�[�h�̔z��</returns>
	public Dictionary<EnumType, NodeBase<NodeType>> GetNodes()
	{
		return m_nodes;
	}

	/// <summary>
	/// ����̃G�b�W���擾
	/// </summary>
	/// <param name="from">�J�n�m�[�h�^�C�v</param>
	/// <param name="to">�J�ڐ�m�[�h�^�C�v</param>
	/// <returns>�G�b�W</returns>
	public EdgeBase<EnumType, TransitionType> GetEdge(EnumType from, EnumType to)
	{
		//���݂��Ȃ�������nullptr��Ԃ��B
		if (m_edgesDictionary.ContainsKey(from))
		{
			return null;
		}

		var edges = m_edgesDictionary[from];
		foreach (var edge in edges)
		{
			if (edge.GetToType().Equals(to))
			{
				return edge;
			}
		}

		return null;
	}

	/// <summary>
	/// �w�肵���m�[�h����L�т�G�b�W�̎擾
	/// </summary>
	/// <param name="from">�w�肵���m�[�h�̃^�C�v</param>
	/// <returns>�G�b�W�z��</returns>
	public List<EdgeBase<EnumType, TransitionType>> GetEdges(EnumType from)
	{
		return m_edgesDictionary[from];
	}

	/// <summary>
	/// �G�b�W�̘A�z�z��S�Ă��擾
	/// </summary>
	/// <returns>�G�b�W�̘A�z�z��S��</returns>
	public Dictionary<EnumType, List<EdgeBase<EnumType, TransitionType>>> GetEdgesDictionary()
    {
		return m_edgesDictionary;
    }

	/// <summary>
	/// ���݂̃X�e�[�g�̃G�b�W�̃��X�g�擾
	/// </summary>
	/// <returns>�G�b�W�̃��X�g�擾</returns>
	public List<EdgeBase<EnumType, TransitionType>> GetNowNodeEdges()
	{
		return m_edgesDictionary[m_nowNodeType];
	}

	/// <summary>
	/// �m�[�h�̒ǉ�
	/// </summary>
	/// <param name="type">�ǉ�����m�[�h�̃^�C�v</param>
	/// <param name="node">�ǉ�����m�[�h</param>
	public void AddNode(EnumType type, NodeBase<NodeType> node)
	{
		if (IsEmpty())
		{
			m_firstType = type;
			m_nowNodeType = type;
			node.OnStart();
		}

		m_nodes[type] = node;
	}

	/// <summary>
	/// �G�b�W�̒ǉ�
	/// </summary>
	/// <param name="edge">�ǉ��������G�b�W</param>
	public void AddEdge(EdgeBase<EnumType, TransitionType> edge)
	{
		//Key�����݂��Ȃ��Ȃ�C���X�^���X����
		if (!m_edgesDictionary.ContainsKey(edge.GetFromType())) 
        {
			m_edgesDictionary[edge.GetFromType()] = new List<EdgeBase<EnumType, TransitionType>>();
        }

		m_edgesDictionary[edge.GetFromType()].Add(edge);
	}

	/// <summary>
	/// �G�b�W�̒ǉ�
	/// </summary>
	/// <param name="from">���̃^�C�v</param>
	/// <param name="to">�J�ڐ�̃^�C�v</param>
	/// <param name="isTransitionFunc">�J�ڏ���</param>
	public void AddEdge(EnumType from, EnumType to, Func<TransitionType, bool> isTransitionFunc)
	{
		var newEdge = new EdgeBase<EnumType, TransitionType>(from, to, isTransitionFunc);
		AddEdge(newEdge);
	}

	/// <summary>
	/// �m�[�h���󂩂ǂ���
	/// </summary>
	/// <returns>��Ȃ�ture</returns>
	public bool IsEmpty()
	{
		return m_nodes.Count == 0 ? true : false;
	}

	/// <summary>
	/// ���Z�b�g�֐�(�ŏ��̃X�e�[�g�ɕύX)
	/// </summary>
	public void Reset()
    {
		ChangeState(m_firstType);
    }

	/// <summary>
	/// �X�e�[�g�̕ύX
	/// </summary>
	/// <param name="type">�ύX�������X�e�[�g�̃^�C�v</param>
	public void ChangeState(EnumType type)
	{
		m_nodes[m_nowNodeType].OnExit();

		m_nowNodeType = type;
		m_nodes[m_nowNodeType].OnStart();
	}

}
