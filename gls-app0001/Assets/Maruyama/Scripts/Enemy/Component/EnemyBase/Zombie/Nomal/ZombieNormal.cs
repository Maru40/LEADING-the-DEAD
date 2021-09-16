using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyBase, I_Chase
{
    //�R���|�[�l���g�n
    Stator_ZombieNormal m_stator;

    void Start()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
    }

    void Update()
    {
        
    }



    //�C���^�[�t�F�[�X�̎���-------------------------------------------------

    void I_Chase.ChangeState(){
        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }
}
