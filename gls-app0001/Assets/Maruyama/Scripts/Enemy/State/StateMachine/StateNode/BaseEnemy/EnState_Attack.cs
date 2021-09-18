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

        //���x��0�ɂ���
        var rigid = owner.GetComponent<Rigidbody>();
        if (rigid) {
            rigid.velocity = Vector3.zero;
        }

        //�U���A�j���[�V�����Đ�
        var animatorCtrl = owner.GetComponent<AnimatorCtrl_ZombieNormal>();
        animatorCtrl.AttackTriggerFire();

        //test
        //��]���ꎞ�Itest�I��off
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, true);

        ChangeComps(EnableChangeType.Start);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }
}
