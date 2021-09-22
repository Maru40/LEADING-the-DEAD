using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_Attack : EnemyStateNodeBase<EnemyBase>
{
    public EnState_Attack(EnemyBase owner)
        :base(owner)
    { }

    public override void OnStart()
    {
        var owner = GetOwner();

        //�U���A�j���[�V�����Đ�
        var animatorCtrl = owner.GetComponent<AnimatorCtrl_ZombieNormal>();
        animatorCtrl.AttackTriggerFire();

        AddChangeComp(owner.GetComponent<ObstacleEvasion>(), false, true);

        //test
        //��]���ꎞ�Itest�I��off
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, true);

        ChangeComps(EnableChangeType.Start);

        Debug.Log("AttackStart");
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }
}
