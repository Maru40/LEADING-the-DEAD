using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using System;

using StateMachine = EnemyMainStateMachine<EnemyBase, SeekType, SeekTransitonMember>;

public enum SeekType {
    Liner,
    Bread
}

public class SeekTransitonMember 
{
    public MyTrigger linerTrigger;
    public MyTrigger breadTrigger;
}


/// <summary>
/// ターゲットの追従
/// </summary>
public class ChaseTarget : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed = 3.0f;

    //目的地にたどり着いたと判断される距離
    [SerializeField]
    float m_nearRange = 0.5f;

    //見失ってから追従する時間
    [SerializeField]
    float m_lostSeekTime = 10.0f;

    /// <summary>
    /// 障害物判定とするレイヤー
    /// </summary> 
    [SerializeField]
    LayerMask m_obstacleLayer = new LayerMask();

    StateMachine m_stateMachine;

    //コンポーネント系------------------

    TargetMgr m_targetMgr;

    void Start()
    {
        m_targetMgr = GetComponent<TargetMgr>();

        m_stateMachine = new StateMachine();

        CreateNode();
        CreateEdge();
    }

    void Update()
    {
        m_stateMachine.OnUpdate();

        StateCheck();  //ステートの管理
    }

    void StateCheck()
    {
        var target = m_targetMgr.GetNowTarget();
        if(target == null)  //ターゲットがnullなら
        {
            TargetLost();
            return;
        }

        var toVec = target.transform.position - transform.position;

        //障害物が合ったら
        if (Physics.Raycast(transform.position, toVec, toVec.magnitude, m_obstacleLayer)){
            m_stateMachine.GetTransitionStructMember().breadTrigger.Fire(); //Breadに変更
        }
        else{
            m_stateMachine.GetTransitionStructMember().linerTrigger.Fire(); //Linerに変更
        }
    }

    void CreateNode()
    {
        var enemy = GetComponent<EnemyBase>();

        m_stateMachine.AddNode(SeekType.Liner, new LinerSeekTarget(enemy, m_maxSpeed));
        m_stateMachine.AddNode(SeekType.Bread, new BreadSeekTarget(enemy, m_nearRange, m_maxSpeed, m_lostSeekTime));
    }

    void CreateEdge()
    {
        m_stateMachine.AddEdge(SeekType.Liner, SeekType.Bread, ToBreadTrigger);

        m_stateMachine.AddEdge(SeekType.Bread, SeekType.Liner, ToLinerTrigger);
    }

    //遷移条件------------------------------------------------------------

    bool ToBreadTrigger(SeekTransitonMember member)
    {
        return member.breadTrigger.Get();
    }
    bool ToLinerTrigger(SeekTransitonMember member)
    {
        return member.linerTrigger.Get();
    }

    
    //アクセッサ------------------------------------------------------------

    public void TargetLost()
    {
        var stator = GetComponent<Stator_ZombieNormal>();
        stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }

    public void SetMaxSpeed(float speed){
        m_maxSpeed = speed;

        m_stateMachine.GetNode<BreadSeekTarget>(SeekType.Bread)?.SetMaxSpeed(speed);
        m_stateMachine.GetNode<LinerSeekTarget>(SeekType.Liner)?.SetMaxSpeed(speed);
    }
    public float GetMaxSpeed() { 
        return m_maxSpeed;
    }

    public void SetNearRange(float range){
        m_nearRange = range;

        m_stateMachine.GetNode<BreadSeekTarget>(SeekType.Bread)?.SetNearRange(range);
    }
    public float GetNearRange(float rage) { 
        return m_nearRange;
    }

    public void SetLostSeekTime(float seekTime){
        m_lostSeekTime = seekTime;

        m_stateMachine.GetNode<BreadSeekTarget>(SeekType.Bread)?.SetLostSeekTime(seekTime);
    }
    public float GetLostSeekTime() { 
        return m_lostSeekTime; 
    }
}
