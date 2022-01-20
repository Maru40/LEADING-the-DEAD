using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public class EnemyMainStateMachine<NodeType, EnumType, TransitionType>
    where NodeType : class
    where EnumType : Enum  
    where TransitionType : struct
{

    /// <summary>
    /// 遷移優先度系のパラメータ
    /// </summary>
    private struct TransitionCanditdateParametor
    {
        public EnumType type; //遷移先のタイプ
        public int priority;  //優先度

        public TransitionCanditdateParametor(EnumType type)
            :this(type, 0)
        {  }

        public TransitionCanditdateParametor(EnumType type, int priority)
        {
            this.type = type;
            this.priority = priority;
        }
    }

    //ステートマシン
    private GraphBase<NodeType, EnumType, TransitionType> m_stateMachine;

    //遷移条件用のメンバー
    private TransitionType m_transitionStruct = new TransitionType();

    //遷移候補群
    private List<TransitionCanditdateParametor> m_transitionCandidates = new List<TransitionCanditdateParametor>();

    //遷移状態のロック
    private bool m_isTransitionLock = false;

    public EnemyMainStateMachine()
    {
        m_stateMachine = new GraphBase<NodeType, EnumType, TransitionType>();
    }

    public EnemyMainStateMachine(TransitionType transitionStruct)
    {
        m_transitionStruct = transitionStruct;
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
    /// <param name="priority">優先度(Default: 0)</param>
    public void AddEdge(EnumType from, EnumType to, EdgeBase<EnumType, TransitionType>.IsTransitionFunc isTransitionFunc, int priority = 0)
    {
        m_stateMachine.AddEdge(from, to, isTransitionFunc, priority);
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
    public ref TransitionType GetTransitionStructMember()
    {
        return ref m_transitionStruct;
    }

    /// <summary>
    /// クロスフェード
    /// </summary>
    /// <param name="type">遷移先のタイプ</param>
    /// <param name="priority">優先度</param>
    public void ChangeState(EnumType type, int priority)
    {
        m_transitionCandidates.Add(new TransitionCanditdateParametor(type, priority));
        //m_stateMachine.ChangeState(type);
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

        //遷移候補から一番優先度が高い遷移先に遷移
        Transition();

        //遷移候補Clear
        m_transitionCandidates.Clear();
    }

    /// <summary>
    /// 現在のノードのUpdate
    /// </summary>
    private void NodeUpdate()
    {
        var nowNode = GetNowNode();
        nowNode?.OnUpdate();
    }

    /// <summary>
    /// 遷移するかチェック
    /// </summary>
    private void TransitionCheck()
    {
        var edges = m_stateMachine.GetNowNodeEdges();
        foreach (var edge in edges)
        {
            if (edge.IsTransition(ref m_transitionStruct))
            {
                m_transitionCandidates.Add(new TransitionCanditdateParametor(edge.GetToType(), edge.Priority));
                //m_stateMachine.ChangeState(edge.GetToType());
                //break;
            }
        }
    }

    /// <summary>
    /// トリガーのリセット
    /// </summary>
    private void TriggerReset()
    {
        var edgesDictionary = m_stateMachine.GetEdgesDictionary();
        foreach(var edges in edgesDictionary)
        {
            foreach(var edge in edges.Value)
            {
                edge.IsTransition(ref m_transitionStruct);
                //edge.IsTransition(m_transitionStruct);
                break;
            }
        }
    }

    /// <summary>
    /// 実際に遷移する
    /// </summary>
    private void Transition()
    {
        //ロックされている また 遷移先が一つも存在しないなら、処理を飛ばす
        if (m_isTransitionLock || m_transitionCandidates.Count == 0) {
            return;
        }

        //ソートして優先度が一番高いタイプに遷移する。
        var sorteds = m_transitionCandidates.OrderByDescending(param => param.priority);

        //int index = 0;
        //foreach (var sorted in sorteds)
        //{
        //    Debug.Log(index + ":トランジション： " + sorted.type);
        //}

        m_stateMachine.ChangeState(sorteds.ElementAt(0).type);  //一番優先度が高い先頭のステートに変更
    }

    public void SetIsTransitionLock(bool isLock)
    {
        m_isTransitionLock = isLock;
    }
}
