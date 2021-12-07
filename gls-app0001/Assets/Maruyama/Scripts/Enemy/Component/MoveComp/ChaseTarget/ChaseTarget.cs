using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using System;
using MaruUtility;

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
public struct ChaseTargetParametor : I_Random<ChaseTargetParametor>
{
    [Header("目的地にたどり着いたと判断される距離")]
    public float nearRange;      //目的地にたどり着いたと判断される距離
    [Header("最大スピード")]
    public float maxSpeed;
    [Header("旋回する力")]
    public float turningPower;
    [Header("見失ってから追従する時間")]
    public float lostSeekTime;   //見失ってから追従する時間
    [Header("集団行動をする範囲")]
    public float inThrongRange;  //集団行動をする範囲

    public ChaseTargetParametor(float nearRange, float maxSpeed, float turningPower, float lostSeekTime, float inThrongRange)
    {
        this.nearRange = nearRange;
        this.maxSpeed = maxSpeed;
        this.turningPower = turningPower;
        this.lostSeekTime = lostSeekTime;
        this.inThrongRange = inThrongRange;
    }

    public void Random(RandomRange<ChaseTargetParametor> range)
    {
        if (range.isActive == false) { return; }

        maxSpeed = UnityEngine.Random.Range(range.min.maxSpeed, range.max.maxSpeed);
        turningPower = UnityEngine.Random.Range(range.min.turningPower, range.max.turningPower);
        lostSeekTime = UnityEngine.Random.Range(range.min.lostSeekTime, range.max.lostSeekTime);
        inThrongRange = UnityEngine.Random.Range(range.min.inThrongRange, range.max.inThrongRange);
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
    I_Chase m_chase;

    void Awake()
    {
        m_targetMgr = GetComponent<TargetManager>();
        m_chase = GetComponent<I_Chase>();

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
        var lostPosition = m_targetMgr.GetLostPosition();
        if(target == null)  //ターゲットがnullなら
        {
            if(lostPosition == null)
            {
                TargetLost();
            }
            else
            {
                m_stateMachine.GetTransitionStructMember().linerTrigger.Fire(); //Linerに変更
            }
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
        var colliders = Physics.OverlapSphere(transform.position, 1, obstacleLayer);
        if (Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer) || colliders.Length != 0){
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

    /// <summary>
    /// ターゲットを見失ったとき
    /// </summary>
    public void TargetLost()
    {
        Debug.Log("対象のロスト");
        m_chase.TargetLost();   
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

    public void SetParametor(ChaseTargetParametor param)
    {
        SetNearRange(param.nearRange);
        SetMaxSpeed(param.maxSpeed);
        SetTurningPower(param.turningPower);
        SetLostSeekTime(param.lostSeekTime);
        SetInThrongRange(param.inThrongRange);
    }

    public void AddParametor(ChaseTargetParametor param)
    {
        m_param.nearRange += param.nearRange;
        m_param.maxSpeed += param.maxSpeed;
        m_param.turningPower += param.turningPower;
        m_param.lostSeekTime += param.lostSeekTime;
        m_param.inThrongRange += param.inThrongRange;

        SetParametor(m_param);
    }

    //Collision---------------------------------------------------------------------------

    private void WallAttack()
    {
        var target = m_targetMgr.GetNowTarget();
        if (target == null) {
            return;
        }

        var data = target.GetFoundData();
        if (data.type == FoundObject.FoundType.SoundObject || data.type == FoundObject.FoundType.Smell) //SoundObjectなら
        {
            //m_wallAttack.AttackStart();
            //var attackManager = GetComponent<AttackNodeManagerBase>();
            //attackManager.AttackStart();
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
