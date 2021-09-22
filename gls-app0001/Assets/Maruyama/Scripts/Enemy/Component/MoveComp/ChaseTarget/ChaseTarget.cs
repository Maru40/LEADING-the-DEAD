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
    //目的地にたどり着いたと判断される距離
    [SerializeField]
    float m_nearRange = 0.5f;

    [SerializeField]
    float m_maxSpeed = 3.0f;

    [SerializeField]
    float m_turningPower = 5.0f;

    //見失ってから追従する時間
    [SerializeField]
    float m_lostSeekTime = 10.0f;

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

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

        var type = target.GetFoundData().type;
        if(type != FoundObject.FoundType.Player) {  //Playerでなかったら
            m_stateMachine.GetTransitionStructMember().linerTrigger.Fire(); //Linerに変更
            return; //処理を終了する。
        }

        //障害物が合ったら
        var toVec = target.transform.position - transform.position;
        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        if (Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer)){
            m_stateMachine.GetTransitionStructMember().breadTrigger.Fire(); //Breadに変更
        }
        else{
            m_stateMachine.GetTransitionStructMember().linerTrigger.Fire(); //Linerに変更
        }
    }

    void CreateNode()
    {
        var enemy = GetComponent<EnemyBase>();

        m_stateMachine.AddNode(SeekType.Liner, new LinerSeekTarget(enemy, m_maxSpeed, m_turningPower));
        m_stateMachine.AddNode(SeekType.Bread, new BreadSeekTarget(enemy, m_nearRange, m_maxSpeed, m_turningPower, m_lostSeekTime));
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


    //Collision---------------------------------------------------------------------------

    private void WallAttack()
    {
        var target = m_targetMgr.GetNowTarget();
        if(target == null) {
            return;
        }

        var data = target.GetFoundData();
        if(data.type == FoundObject.FoundType.SoundObject) //SoundObjectなら
        {
            var stator = GetComponent<Stator_ZombieNormal>(); //壁でも攻撃する。
            if (stator)
            {
                stator.GetTransitionMember().attackTrigger.Fire();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enabled == false) {
            return;
        }

        if(collision.gameObject.tag == "T_Wall")
        {
            WallAttack();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (enabled == false) {
            return;
        }

        if (collision.gameObject.tag == "T_Wall")
        {
            WallAttack();
        }
    }
}
