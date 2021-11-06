using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

using MaruUtility.UtilityDictionary;

public class AnimatorManager_ZombieNormal : AnimatorManagerBase
{
    enum LayerEnum
    {
        Base,
        Upper,
        Lower
    }

    enum StateEnum
    {

    }

    enum NormalAttackHitColliderType
    {
        Left,
        Right
    }

    [SerializeField]
    Ex_Dictionary<NormalAttackHitColliderType, AnimationHitColliderParametor> m_normalAttackParam =
        new Ex_Dictionary<NormalAttackHitColliderType, AnimationHitColliderParametor>();

    Dictionary<StateEnum, string> m_stateNameDictionary = new Dictionary<StateEnum, string>();

    NormalAttack m_normalAttackComp;

    EnemyStunManager m_stunManager;
    AngerManager m_angerManager;
    KnockBackManager m_knockBackManager;

    override protected void Awake()
    {
        base.Awake();

        m_normalAttackComp = GetComponent<NormalAttack>();
        m_stunManager = GetComponent<EnemyStunManager>();
        m_angerManager = GetComponent<AngerManager>();
        m_knockBackManager = GetComponent<KnockBackManager>();
    }

    private void Start()
    {
        m_normalAttackParam.InsertInspectorData();

        SettingNormalAttack();

        SettingStun();
        SettingAnger();

        SettingKnockBack();

        SettingDeath();
    }

    void SettingNormalAttack()
    {
        var behavior = ZombieNormalTable.UpperLayer.NormalAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        var timeParam = m_normalAttackParam;
        var leftTimeParam = timeParam[NormalAttackHitColliderType.Left];
        var rightTimeParam = timeParam[NormalAttackHitColliderType.Right];
        var timeEvent = behavior.onTimeEvent;
        //左腕もコライダーのOn、Off
        timeEvent.ClampWhere(leftTimeParam.startTime)
            .Subscribe(_ => { leftTimeParam.trigger.AttackStart();
                m_normalAttackComp.ChaseEnd();
            }).AddTo(this);
        timeEvent.ClampWhere(leftTimeParam.endTime)
            .Subscribe(_ => leftTimeParam.trigger.AttackEnd()).AddTo(this);

        //右腕のコライダーのOn、Off
        timeEvent.ClampWhere(rightTimeParam.startTime)
            .Subscribe(_ => rightTimeParam.trigger.AttackStart()).AddTo(this);
        timeEvent.ClampWhere(rightTimeParam.endTime)
            .Subscribe(_ => rightTimeParam.trigger.AttackEnd()).AddTo(this);

        behavior.onStateEntered.Subscribe(_ => m_normalAttackComp.AttackStart()).AddTo(this);
        behavior.onStateExited.Subscribe(_ => m_normalAttackComp.EndAnimationEvent()).AddTo(this);
    }

    void SettingStun()
    {
        const float stunTransitionTimes = 0.25f;
        m_stunManager.IsStunReactive.Where(isStun => isStun)
            .Subscribe(_ => { CrossFadeStunAnimation(stunTransitionTimes); })
            .AddTo(this);

        //スタン解除時
        m_stunManager.IsStunReactive.Skip(1)
            .Where(isStun => !isStun)
            .Subscribe(_ => { CrossFadeIdleAnimation("Upper Layer"); })
            .AddTo(this);
    }

    void SettingAnger()
    {
        m_angerManager.isAngerObservable.Where(isAnger => isAnger)
            .Subscribe(_ => ChangeAngerAnimation())
            .AddTo(this);
    }

    void SettingKnockBack()
    {
        m_knockBackManager.IsKnockBackReactive.Where(isKnockBack => isKnockBack)
            .Subscribe(_ =>
            {
                CrossFadeKnockBackState();
            })
            .AddTo(this);

        m_knockBackManager.IsKnockBackReactive.Skip(1)
            .Where(isKnockBack => !isKnockBack && !m_statusManager.IsStun)
            .Subscribe(_ => CrossFadeIdleAnimation())
            .AddTo(this);

        var behavior = ZombieNormalTable.UpperLayer.KnockBack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        behavior.onStateExited.Where(_ => m_stunManager.IsStun)
            .Subscribe(_ => { 
                CrossFadeStunAnimation();
            })
            .AddTo(this);
    }

    void SettingDeath()
    {

    }

    //ChangeAnimaotion-------------------------------------------------------------------------

    public void ChangeNormalAttackAnimation()
    {
        var layerIndex = m_animator.GetLayerIndex("Upper Layer");
        CrossFadeState("NormalAttack", layerIndex);
    }

    void CrossFadeStunAnimation(float transitionTime = 0)
    {
        var layerIndex = m_animator.GetLayerIndex("Upper Layer");
        CrossFadeState("Stunned", layerIndex, transitionTime);
    }

    void ChangeAngerAnimation()
    {
        var layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("Anger", layerIndex);
    }

    public void CrossFadeIdleAnimation(string layerString = "Base Layer")
    {
        var layerIndex = m_animator.GetLayerIndex(layerString);
        CrossFadeState("Idle", layerIndex);
    }

    public void CrossFadeDeathAnimatiron()
    {
        var layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("Death", layerIndex);
    }

    void CrossFadeKnockBackState()
    {
        var layerIndex = m_animator.GetLayerIndex("Upper Layer");
        CrossFadeState("KnockBack", layerIndex);
    }
}
