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

[Serializable]
public struct ChaseTargetParametor
{
    public float nearRange;      //目的地にたどり着いたと判断される距離
    public float maxSpeed;
    public float turningPower;
    public float lostSeekTime;   //見失ってから追従する時間
    public float inThrongRange;  //集団行動をする範囲

    public ChaseTargetParametor(float nearRange, float maxSpeed, float turningPower, float lostSeekTime, float inThrongRange)
    {
        this.nearRange = nearRange;
        this.maxSpeed = maxSpeed;
        this.turningPower = turningPower;
        this.lostSeekTime = lostSeekTime;
        this.inThrongRange = inThrongRange;
    }
}

/// <summary>
/// ターゲットの追従
/// </summary>
public class ChaseTarget : MonoBehaviour
{
    [SerializeField]
    ChaseTargetParametor m_param = new ChaseTargetParametor(0.75f, 3.0f, 3.0f, 10.0f, 3.0f);

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    StateMachine m_stateMachine;

    //コンポーネント系------------------

    TargetManager m_targetMgr;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();

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

        float nearRange = m_param.nearRange;
        float maxSpeed = m_param.maxSpeed;
        float turningPower = m_param.turningPower;
        float lostSeekTime = m_param.lostSeekTime;

        m_stateMachine.AddNode(SeekType.Liner, new LinerSeekTarget(enemy, maxSpeed, turningPower));
        m_stateMachine.AddNode(SeekType.Bread, new BreadSeekTarget(enemy, nearRange, maxSpeed, turningPower, lostSeekTime));
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
        m_param.maxSpeed = speed;

        m_stateMachine?.GetNode<BreadSeekTarget>(SeekType.Bread)?.SetMaxSpeed(speed);
        m_stateMachine?.GetNode<LinerSeekTarget>(SeekType.Liner)?.SetMaxSpeed(speed);
    }
    public float GetMaxSpeed() { 
        return m_param.maxSpeed;
    }

    public void SetNearRange(float range){
        m_param.nearRange = range;

        m_stateMachine?.GetNode<BreadSeekTarget>(SeekType.Bread)?.SetNearRange(range);
    }
    public float GetNearRange(float rage) { 
        return m_param.nearRange;
    }

    public void SetTurningPower(float power) {
        m_param.turningPower = power;
    }
    public float GetTurningPower() {
        return m_param.turningPower;
    }

    public void SetLostSeekTime(float seekTime){
        m_param.lostSeekTime = seekTime;

        m_stateMachine?.GetNode<BreadSeekTarget>(SeekType.Bread)?.SetLostSeekTime(seekTime);
    }
    public float GetLostSeekTime() { 
        return m_param.lostSeekTime; 
    }

    public void SetInThrongRange(float range) {
        m_param.inThrongRange = range;
    }
    public float GetInThrongRange() {
        return m_param.inThrongRange;
    }

    public void SetPamametor(ChaseTargetParametor param)
    {
        SetNearRange(param.nearRange);
        SetMaxSpeed(param.maxSpeed);
        SetTurningPower(param.turningPower);
        SetLostSeekTime(param.lostSeekTime);
        SetInThrongRange(param.inThrongRange);
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
