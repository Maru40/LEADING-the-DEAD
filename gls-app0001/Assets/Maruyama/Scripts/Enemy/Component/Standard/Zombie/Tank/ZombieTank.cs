using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTank : EnemyBase, I_Chase, I_Listen
{
    //コンポーネント系

    private Stator_ZombieTank m_stator;
    private AnimatorManager_ZombieTank m_animatorManager;
    private TargetManager m_targetMgr;

    private void Start()
    {
        m_stator = GetComponent<Stator_ZombieTank>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieTank>();
        m_targetMgr = GetComponent<TargetManager>();

        m_animatorManager.CrossFadeLoopShout();
    }

    private void Update()
    {
        
    }

    public void GameStartEvent()
    {
        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
        m_animatorManager.CrossFadeIdle();
    }

    //インターフェースの実装-------------------------------------------------

    void I_Chase.ChangeState()
    {
        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }

    void I_Chase.TargetLost()
    {
        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }

    void I_Listen.Listen(FoundObject foundObject)
    {
        //ターゲットの切替
        //m_targetMgr.SetNowTarget(GetType(), foundObject);

        //var member = m_stator.GetTransitionMember();
        //member.chaseTrigger.Fire();
    }
}
