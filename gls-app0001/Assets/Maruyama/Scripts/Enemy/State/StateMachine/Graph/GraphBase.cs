using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GraphBase<NodeType, EnumType, TransitionType>
	where NodeType : class
	where EnumType : Enum
	where TransitionType : class
{
	//最初のノード(リセット行為に使う)
	private EnumType m_firstType;

	//現在のノード
	private EnumType m_nowNodeType;

	//ノードの連想配列
	private Dictionary<EnumType, NodeBase<NodeType>> m_nodes;

	//エッジの連想配列リスト
	private Dictionary<EnumType, List<EdgeBase<EnumType, TransitionType>>> m_edgesDictionary;

    public GraphBase()
    {
		m_nodes = new Dictionary<EnumType, NodeBase<NodeType>>();
		m_edgesDictionary = new Dictionary<EnumType, List<EdgeBase<EnumType, TransitionType>>>();
	}


	/// <summary>
	/// 現在使うノードのタイプの取得
	/// </summary>
	/// <returns>ノードのタイプ</returns>
	public EnumType GetNowType()
	{
		return m_nowNodeType;
	}

	/// <summary>
	/// 現在使うノードの取得
	/// </summary>
	/// <returns>ノード</returns>
	public NodeBase<NodeType> GetNowNode()
	{
		return m_nodes[m_nowNodeType];
	}

	/// <summary>
	/// 指定したノードの取得
	/// </summary>
	/// <param name="type">指定したノードのタイプ</param>
	/// <returns>ノード</returns>
	public NodeBase<NodeType> GetNode(EnumType type)
	{
		return m_nodes[type];
	}

	/// <summary>
	/// ノードの配列を取得
	/// </summary>
	/// <returns>ノードの配列</returns>
	public Dictionary<EnumType, NodeBase<NodeType>> GetNodes()
	{
		return m_nodes;
	}

	/// <summary>
	/// 特定のエッジを取得
	/// </summary>
	/// <param name="from">開始ノードタイプ</param>
	/// <param name="to">遷移先ノードタイプ</param>
	/// <returns>エッジ</returns>
	public EdgeBase<EnumType, TransitionType> GetEdge(EnumType from, EnumType to)
	{
		//存在しなかったらnullptrを返す。
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
	/// 指定したノードから伸びるエッジの取得
	/// </summary>
	/// <param name="from">指定したノードのタイプ</param>
	/// <returns>エッジ配列</returns>
	public List<EdgeBase<EnumType, TransitionType>> GetEdges(EnumType from)
	{
		return m_edgesDictionary[from];
	}

	/// <summary>
	/// エッジの連想配列全てを取得
	/// </summary>
	/// <returns>エッジの連想配列全て</returns>
	public Dictionary<EnumType, List<EdgeBase<EnumType, TransitionType>>> GetEdgesDictionary()
    {
		return m_edgesDictionary;
    }

	/// <summary>
	/// 現在のステートのエッジのリスト取得
	/// </summary>
	/// <returns>エッジのリスト取得</returns>
	public List<EdgeBase<EnumType, TransitionType>> GetNowNodeEdges()
	{
		var exist = m_edgesDictionary.ContainsKey(m_nowNodeType);
		//keyが存在したら返す。
		return exist ? m_edgesDictionary[m_nowNodeType] : new List<EdgeBase<EnumType, TransitionType>>();
	}

	/// <summary>
	/// ノードの追加
	/// </summary>
	/// <param name="type">追加するノードのタイプ</param>
	/// <param name="node">追加するノード</param>
	public void AddNode(EnumType type, NodeBase<NodeType> node)
	{
		if (IsEmpty())
		{
			m_firstType = type;
			m_nowNodeType = type;
			node?.OnStart();
		}

		m_nodes[type] = node;
	}

	/// <summary>
	/// エッジの追加
	/// </summary>
	/// <param name="edge">追加したいエッジ</param>
	public void AddEdge(EdgeBase<EnumType, TransitionType> edge)
	{
		//Keyが存在しないならインスタンス生成
		if (!m_edgesDictionary.ContainsKey(edge.GetFromType())) 
        {
			m_edgesDictionary[edge.GetFromType()] = new List<EdgeBase<EnumType, TransitionType>>();
        }

		m_edgesDictionary[edge.GetFromType()].Add(edge);
	}

	/// <summary>
	/// エッジの追加
	/// </summary>
	/// <param name="from">元のタイプ</param>
	/// <param name="to">遷移先のタイプ</param>
	/// <param name="isTransitionFunc">遷移条件</param>
	/// <param name="priority">優先度</param>
	public void AddEdge(EnumType from, EnumType to, Func<TransitionType, bool> isTransitionFunc, int priority)
	{
		var newEdge = new EdgeBase<EnumType, TransitionType>(from, to, isTransitionFunc, priority);
		AddEdge(newEdge);
	}

	/// <summary>
	/// ノードが空かどうか
	/// </summary>
	/// <returns>空ならture</returns>
	public bool IsEmpty()
	{
		return m_nodes.Count == 0 ? true : false;
	}

	/// <summary>
	/// リセット関数(最初のステートに変更)
	/// </summary>
	public void Reset()
    {
		ChangeState(m_firstType);
    }

	/// <summary>
	/// ステートの変更
	/// </summary>
	/// <param name="type">変更したいステートのタイプ</param>
	public void ChangeState(EnumType type)
	{
		m_nodes[m_nowNodeType]?.OnExit();

		m_nowNodeType = type;
		m_nodes[m_nowNodeType]?.OnStart();
	}

}
