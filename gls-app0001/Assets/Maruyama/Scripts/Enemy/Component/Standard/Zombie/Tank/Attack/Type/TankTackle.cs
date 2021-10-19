using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class TankTackle : AttackBase
{
    enum State 
    {
        None,
        Tackle,
    }

    State m_state = State.None;

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
    }

    private void Update()
    {
        TackleMove();
    }

    void TackleMove()
    {
        if(m_state != State.Tackle) {  //Tackleでなかったら処理をしない
            return;
        }


    }

    public override void AttackStart()
    {

    }

    public override void Attack()
    {

    }

    public override void AttackHitEnd()
    {

    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        FoundObject target = m_targetManager.GetNowTarget();
        if (target) {
            return Calculation.IsRange(gameObject, target.gameObject, range);
        }
        else {
            return false;
        }
    }

    public override void EndAnimationEvent()
    {

    }
}
