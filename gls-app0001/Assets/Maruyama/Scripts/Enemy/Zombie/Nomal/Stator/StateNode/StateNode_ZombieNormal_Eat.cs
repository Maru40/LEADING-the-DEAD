using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Eat : EnemyStateNodeBase<EnemyBase>
{
    private AnimatorManager_ZombieNormal m_animatorManager;
    private TargetManager m_targetManager;
    private Stator_ZombieNormal m_stator;
    private EnemyVelocityManager m_velocityManager;
    private SmellManager m_smellManager;
    private Rigidbody m_rigid;

    public StateNode_ZombieNormal_Eat(EnemyBase owner)
        :base(owner)
    {
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_smellManager = owner.GetComponent<SmellManager>();
        m_rigid = owner.GetComponent<Rigidbody>();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ThrongManager>(), false, true);
        AddChangeComp(owner.GetComponent<ObstacleEvasion>(), false, true);
    }

    public override void OnStart()
    {
        base.OnStart();

        m_animatorManager.CrossFadeEatAnimation();
        m_velocityManager.StartDeseleration();
        m_rigid.isKinematic = true;
    }

    public override void OnUpdate()
    {
        Debug.Log("EatUpdate");

        if (m_targetManager.HasTarget())  
        {
            //ターゲットと遠かったらChase
            if (!m_smellManager.IsTargetNear(m_smellManager.NearRange))
            {
                m_stator.GetTransitionMember().chaseTrigger.Fire();
                return;
            }

            var eaten = m_targetManager.GetNowTarget().GetComponent<EatenBase>();
            if (eaten == null) {
                m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
            }
        }
        else  //ターゲットが無かったら
        {
            m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.AllLayerIndex);
        m_velocityManager.SetIsDeseleration(false);
        m_rigid.isKinematic = false;
    }
}
