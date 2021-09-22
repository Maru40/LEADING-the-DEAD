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
/// �^�[�Q�b�g�̒Ǐ]
/// </summary>
public class ChaseTarget : MonoBehaviour
{
    //�ړI�n�ɂ��ǂ蒅�����Ɣ��f����鋗��
    [SerializeField]
    float m_nearRange = 0.5f;

    [SerializeField]
    float m_maxSpeed = 3.0f;

    [SerializeField]
    float m_turningPower = 5.0f;

    //�������Ă���Ǐ]���鎞��
    [SerializeField]
    float m_lostSeekTime = 10.0f;

    /// <summary>
    /// Ray�̏�Q������Layer�̔z��
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    StateMachine m_stateMachine;

    //�R���|�[�l���g�n------------------

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

        StateCheck();  //�X�e�[�g�̊Ǘ�
    }

    void StateCheck()
    {
        var target = m_targetMgr.GetNowTarget();
        if(target == null)  //�^�[�Q�b�g��null�Ȃ�
        {
            TargetLost();
            return;
        }

        var type = target.GetFoundData().type;
        if(type != FoundObject.FoundType.Player) {  //Player�łȂ�������
            m_stateMachine.GetTransitionStructMember().linerTrigger.Fire(); //Liner�ɕύX
            return; //�������I������B
        }

        //��Q������������
        var toVec = target.transform.position - transform.position;
        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        if (Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer)){
            m_stateMachine.GetTransitionStructMember().breadTrigger.Fire(); //Bread�ɕύX
        }
        else{
            m_stateMachine.GetTransitionStructMember().linerTrigger.Fire(); //Liner�ɕύX
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

    //�J�ڏ���------------------------------------------------------------

    bool ToBreadTrigger(SeekTransitonMember member)
    {
        return member.breadTrigger.Get();
    }
    bool ToLinerTrigger(SeekTransitonMember member)
    {
        return member.linerTrigger.Get();
    }

    
    //�A�N�Z�b�T------------------------------------------------------------

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
        if(data.type == FoundObject.FoundType.SoundObject) //SoundObject�Ȃ�
        {
            var stator = GetComponent<Stator_ZombieNormal>(); //�ǂł��U������B
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
