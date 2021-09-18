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

        //test
        //��]���ꎞ�Itest�I��off
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, true);
        //���ōU������
        owner.GetComponent<Attack_ZombieNormal>().Attack();

        //�A�j���[�V�����̍Đ�

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
