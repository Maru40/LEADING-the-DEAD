using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyBase, I_Chase
{
    //コンポーネント系
    Stator_ZombieNormal m_stator;

    void Start()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
    }

    void Update()
    {
        
    }



    //インターフェースの実装-------------------------------------------------

    void I_Chase.ChangeState(){
        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }
}
