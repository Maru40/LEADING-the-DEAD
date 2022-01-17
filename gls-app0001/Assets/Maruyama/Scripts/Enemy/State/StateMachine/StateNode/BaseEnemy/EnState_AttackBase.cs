using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnState_AttackBase : EnemyStateNodeBase<EnemyBase>
{
    public EnState_AttackBase(EnemyBase owner)
        :base(owner)
    { }

    /// <summary>
    /// 攻撃するアニメーションの再生
    /// </summary>
    protected virtual void PlayStartAnimation() { }

    /// <summary>
    /// 切り替えるコンポーネントの準備
    /// </summary>
    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<ObstacleEvasion>(), false, true);
    }

    public override void OnStart()
    {
        PlayStartAnimation();

        base.OnStart();
    }

    public override void OnUpdate()
    {
        Debug.Log("AttackState");
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
