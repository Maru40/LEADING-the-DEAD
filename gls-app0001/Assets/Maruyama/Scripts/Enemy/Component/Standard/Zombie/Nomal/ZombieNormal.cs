using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyBase, I_Chase, I_Listen
{
    //コンポーネント系
    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;

    void Start()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
    }

    void Update()
    {
        
    }



    //インターフェースの実装-------------------------------------------------

    void I_Chase.ChangeState(){
        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }

    void I_Listen.Listen(FoundObject foundObject) {
        //ターゲットの切替
        m_targetMgr.SetNowTarget(GetType() ,foundObject);

        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }
}
