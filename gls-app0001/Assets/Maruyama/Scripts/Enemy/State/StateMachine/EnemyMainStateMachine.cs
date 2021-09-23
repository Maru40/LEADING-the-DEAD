using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EnemyMainStateMachine<NodeType, EnumType, TransitionType>
    where NodeType : class
    where EnumType : Enum  
    where TransitionType : class, new()
{
    //ステートマシン
    GraphBase<NodeType, EnumType, TransitionType> m_stateMachine;

    //遷移条件用のメンバー
    TransitionType m_transitionStruct = new TransitionType();

    public EnemyMainStateMachine()
    {
        m_stateMachine = new GraphBase<NodeType, EnumType, TransitionType>();
    }

    /// <summary>
    /// 現在使うノードのタイプの取得
    /// </summary>
    /// <returns>ノードのタイプ</returns>
    public EnumType GetNowType()
    {
        return m_stateMachine.GetNowType();
    }

    /// <summary>
	/// 現在使うノードの取得
	/// </summary>
	/// <returns>ノード</returns>
    public NodeBase<NodeType> GetNowNode()
    {
        return m_stateMachine.GetNowNode();
    }

    /// <summary>
	/// 指定したノードの取得
	/// </summary>
	/// <param name="type">指定したノードのタイプ</param>
	/// <returns>ノード</returns>
    public NodeBase<NodeType> GetNode(EnumType type)
    {
        return m_stateMachine.GetNode(type);
    }

    /// <summary>
    /// 指定したノードをtemplateで指定した型にキャストしてもらう。
    /// </summary>
    /// <typeparam name="T">キャストする型</typeparam>
    /// <param name="type">ノードのタイプ</param>
    /// <returns>キャストされたノード</returns>
    public T GetNode<T>(EnumType type) where T: class
    {
        var node = m_stateMachine.GetNode(type) as T;
        return node;
    }

    /// <summary>
    /// ノードの追加
    /// </summary>
    /// <param name="type">ノートのタイプ</param>
    /// <param name="node">ノードの本体</param>
    public void AddNode(EnumType type, NodeBase<NodeType> node)
    {
        m_stateMachine.AddNode(type, node);
    }

    /// <summary>
    /// エッジの追加
    /// </summary>
    /// <param name="edge">追加したいエッジ</param>
    public void AddEdge(EdgeBase<EnumType, TransitionType> edge)
    {
        m_stateMachine.AddEdge(edge);
    }

    /// <summary>
    /// エッジの追加
    /// </summary>
    /// <param name="from">元のタイプ</param>
    /// <param name="to">遷移先のタイプ</param>
    /// <param name="isTransitionFunc">遷移条件</param>
    public void AddEdge(EnumType from, EnumType to, Func<TransitionType, bool> isTransitionFunc)
    {
        m_stateMachine.AddEdge(from, to, isTransitionFunc);
    }


    /// <summary>
    /// ノードが空かどうかを判断
    /// </summary>
    /// <returns>ノードの空ならtrue</returns>
    public bool IsEmpty() {
	    return m_stateMachine.IsEmpty();
    }

    /// <summary>
    /// リセット関数(最初のステートに変更)
    /// </summary>
    public void Reset()
    {
        m_stateMachine.Reset();
    }

    /// <summary>
    /// 遷移に利用する構造体を取得する。
    /// </summary>
    /// <returns>構造体の参照を渡す</returns>
    public TransitionType GetTransitionStructMember()
    {
        return m_transitionStruct;
    }

    /// <summary>
    /// 外部からUpdateをする。(主にこれを利用するStateManagerクラス)
    /// </summary>
    public void OnUpdate()
    {
        if (IsEmpty()){
            return;
        }

        //NodeのUpdate
        NodeUpdate();

        //遷移チェック
        TransitionCheck();

        //トリガーのリセット
        TriggerReset();
    }

    /// <summary>
    /// 現在のノードのUpdate
    /// </summary>
    void NodeUpdate()
    {
        var nowNode = GetNowNode();
        nowNode.OnUpdate();
    }

    /// <summary>
    /// 遷移するかチェック
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
    /// トリガーのリセット
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
