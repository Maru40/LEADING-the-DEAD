using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

public class AnimatorManager_ZombieNormal : MonoBehaviour
{
    [Serializable]
    struct NormalAttackParametor
    {
        public float hitStartTime;
        public float hitEndTime;
    }

    [SerializeField]
    NormalAttackParametor m_normalAttackParam = new NormalAttackParametor();

    Animator m_animator;
    NormalAttack m_normalAttackComp;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_normalAttackComp = GetComponent<NormalAttack>();

        SettingNormalAttack();
    }

    void SettingNormalAttack()
    {
        var behavior = ZombieNormalTable.BaseLayer.NormalAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        var timeParam = m_normalAttackParam;
        var timeEvent = behavior.onTimeEvent;
        timeEvent.ClampWhere(timeParam.hitStartTime).Subscribe(_ => m_normalAttackComp.AttackHitStart()).AddTo(this);
        timeEvent.ClampWhere(timeParam.hitEndTime).Subscribe(_ => m_normalAttackComp.AttackHitEnd()).AddTo(this);

        behavior.onStateEntered.Subscribe(_ => m_normalAttackComp.AttackStart()).AddTo(this);
        behavior.onStateExited.Subscribe(_ => m_normalAttackComp.EndAnimationEvent()).AddTo(this);
    }

    public void CrossFadeState(string stateName, string layerName, float transitionTime = 0.0f)
    {
        int layerIndex = m_animator.GetLayerIndex(layerName);
        m_animator.CrossFade(stateName, transitionTime, layerIndex);
    }

    public void CrossFadeState(string stateName, int layerIndex, float transitionTime = 0.0f)
    {
        m_animator.CrossFade(stateName, transitionTime, layerIndex);
    }


    //ChangeAnimaotion-------------------------------------------------------------------------

    public void ChangeNormalAttackAnimation()
    {
        var layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("NormalAttack", layerIndex);
    }
}
