﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

public class AnimatorManager_ZombieTank : MonoBehaviour
{
    [Serializable]
    struct NormalAttackParametor
    {
        public float hitStartTime;
        public float hitEndTime;
    }

    [Serializable]
    struct TackleParametor
    {
        public float tackleStartTime; //タックルスタート
        public float deselerationStartTime; //減速スタート
    }

    [SerializeField]
    NormalAttackParametor m_normapAttackParam = new NormalAttackParametor();

    [SerializeField]
    TackleParametor m_tackleParam = new TackleParametor();

    Animator m_animator;

    NormalAttack m_normalAttackComp;
    TankTackle m_tackleComp;
    //StateMachineBehaviourTable<TimeEventStateMachineBehaviour> m_behaviorTable;

    void Awake()
    {
        m_normalAttackComp = GetComponent<NormalAttack>();
        m_tackleComp = GetComponent<TankTackle>();

        m_animator = GetComponent<Animator>();
        //m_behaviorTable = new StateMachineBehaviourTable<TimeEventStateMachineBehaviour>(m_animator);

        SettingNormalAttackAnimation();
        SettingTackleAnimation();
        SettingTackleLastAnimation();
    }

    void SettingNormalAttackAnimation()
    {
        var layerIndex = m_animator.GetLayerIndex("Base Layer");

        //Time系
        var timeParam = m_normapAttackParam;
        //var attack = m_behaviorTable["Base Layer.NormalAttack"];
        var attack = ZombieTankTable.BaseLayer.NormalAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        var timeEvent = attack.onTimeEvent;
        timeEvent.ClampWhere(timeParam.hitStartTime).Subscribe(_ => m_normalAttackComp.AttackHitStart()).AddTo(this);
        timeEvent.ClampWhere(timeParam.hitEndTime).Subscribe(_ => m_normalAttackComp.AttackHitEnd()).AddTo(this);

        attack.onStateEntered.Subscribe(_ => m_normalAttackComp.AttackStart()).AddTo(this);
        attack.onStateExited.Subscribe(_ => m_normalAttackComp.EndAnimationEvent()).AddTo(this);
    }

    void SettingTackleAnimation()
    { 
        var layerIndex = m_animator.GetLayerIndex("Base Layer");
        m_tackleComp.state.Where(_ => m_tackleComp.state.Value == TankTackle.State.TackleLast)
            .Subscribe(_ => GoState("TackleLast", layerIndex))
            .AddTo(this);

        //Time系
        //var tackle = m_behaviorTable["Base Layer.TackleAttack"];
        var tackle = ZombieTankTable.BaseLayer.TackleAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        tackle.onStateEntered.Subscribe(_ => m_tackleComp.AttackStart()).AddTo(this);

        var timeEvent = tackle.onTimeEvent;
        timeEvent.ClampWhere(m_tackleParam.tackleStartTime).Subscribe(_ => m_tackleComp.AttackHitStart()).AddTo(this);
        //timeEvent.ClampWhere(m_tackleParam.deselerationStartTime).Subscribe(_ => m_attackComp.AttackHitEnd()).AddTo(this);

        //tackle.onStateExited.Subscribe(_ => m_attackComp.EndAnimationEvent());
    }

    void SettingTackleLastAnimation()
    {
        //var tackle = m_behaviorTable["Base Layer.TackleLast"];
        var tackle = ZombieTankTable.BaseLayer.TackleLast.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        tackle.onStateExited.Subscribe(_ => m_tackleComp.EndAnimationEvent());
    }

    public void GoState(string stateName, int layerIndex, float transitionTime = 0.0f)
    {
        Debug.Log("GoState");
        m_animator.CrossFade(stateName, transitionTime, layerIndex);
    }
}
