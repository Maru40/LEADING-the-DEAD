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

        //速度を0にする
        var rigid = owner.GetComponent<Rigidbody>();
        if (rigid) {
            rigid.velocity = Vector3.zero;
        }

        //test
        //回転を一時的test的にoff
        AddChangeComp(owner.GetComponent<EnemyRotationCtrl>(), false, true);
        //仮で攻撃実装
        owner.GetComponent<Attack_ZombieNormal>().Attack();

        //アニメーションの再生

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
