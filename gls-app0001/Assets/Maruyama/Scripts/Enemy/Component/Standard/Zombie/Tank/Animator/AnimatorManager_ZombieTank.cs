using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

public class AnimatorManager_ZombieTank : MonoBehaviour
{
    [Serializable]
    struct TackleParametor
    {
        public float tackleStartTime; //タックルスタート
        public float deselerationStartTime; //減速スタート
    }

    [SerializeField]
    TackleParametor m_tackleParam = new TackleParametor();

    Animator m_animator;

    TankTackle m_attackComp;
    StateMachineBehaviourTable<TimeEventStateMachineBehaviour> m_behaviorTable;

    void Awake()
    {
        m_attackComp = GetComponent<TankTackle>();

        m_animator = GetComponent<Animator>();
        m_behaviorTable = new StateMachineBehaviourTable<TimeEventStateMachineBehaviour>(m_animator);

        SettingTackleAnimation();
    }

    void SettingTackleAnimation()
    { 
        var tackle = m_behaviorTable["Base Layer.TackleAttack"];
        var timeEvent = tackle.onTimeEvent;
        timeEvent.ClampWhere(0.0f).Subscribe(_ => m_attackComp.AttackStart()).AddTo(this);
        timeEvent.ClampWhere(m_tackleParam.tackleStartTime).Subscribe(_ => m_attackComp.AttackHitStart()).AddTo(this);
        timeEvent.ClampWhere(m_tackleParam.deselerationStartTime).Subscribe(_ => m_attackComp.AttackHitEnd()).AddTo(this);

        tackle.onStateExited.Subscribe(_ => m_attackComp.EndAnimationEvent());
    }
}
