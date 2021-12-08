using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieNormal_Eat : EnemyStateNodeBase<EnemyBase>
{
    AnimatorManager_ZombieNormal m_animatorManager;
    TargetManager m_targetManager;
    Stator_ZombieNormal m_stator;
    EnemyVelocityMgr m_velocityManager;
    SmellManaer m_smellManager;

    public StateNode_ZombieNormal_Eat(EnemyBase owner)
        :base(owner)
    {
        m_animatorManager = owner.GetComponent<AnimatorManager_ZombieNormal>();
        m_targetManager = owner.GetComponent<TargetManager>();
        m_stator = owner.GetComponent<Stator_ZombieNormal>();
        m_velocityManager = owner.GetComponent<EnemyVelocityMgr>();
        m_smellManager = owner.GetComponent<SmellManaer>();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ThrongManager>(), false, true);
    }

    public override void OnStart()
    {
        base.OnStart();

        m_animatorManager.CrossFadeEatAnimation();
        m_velocityManager.StartDeseleration();
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
    }
}
