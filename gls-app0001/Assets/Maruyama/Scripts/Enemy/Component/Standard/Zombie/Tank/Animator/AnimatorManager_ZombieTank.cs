using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

public class AnimatorManager_ZombieTank : AnimatorManagerBase
{

    [Serializable]
    struct TackleParametor
    {
        //public float tackleStartTime; //タックルスタート
        public float chargeTime;
        public float deselerationStartTime; //減速スタート
    }

    [SerializeField]
    AnimationHitColliderParametor m_normalAttackParam = new AnimationHitColliderParametor();

    [SerializeField]
    TackleParametor m_tackleParam = new TackleParametor();

    NormalAttack m_normalAttackComp;
    TankTackle m_tackleComp;
    WaitTimer m_waitTimer;

    override protected void Awake()
    {
        base.Awake();

        m_waitTimer = GetComponent<WaitTimer>();
        m_normalAttackComp = GetComponent<NormalAttack>();
        m_tackleComp = GetComponent<TankTackle>();

        SettingNormalAttackAnimation();
        SettingTackleAnimation();
        SettingTackleLastAnimation();
    }

    private void Update()
    {
        //Debug.Log("トラン: " + m_animator.IsInTransition(0));
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    void SettingNormalAttackAnimation()
    {
        var layerIndex = m_animator.GetLayerIndex("Base Layer");

        //Time系
        var timeParam = m_normalAttackParam;
        var attack = ZombieTankTable.BaseLayer.NormalAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        var timeEvent = attack.onTimeEvent;
        timeEvent.ClampWhere(timeParam.startTime)
            .Subscribe(_ => {
                timeParam.trigger.AttackStart();
                m_normalAttackComp.ChaseEnd();  //将来的に関数名変更
            }).AddTo(this);
        timeEvent.ClampWhere(timeParam.endTime)
            .Subscribe(_ => timeParam.trigger.AttackEnd()).AddTo(this);

        attack.onStateEntered.Subscribe(_ => m_normalAttackComp.AttackStart()).AddTo(this);
        attack.onStateExited.Subscribe(_ => m_normalAttackComp.EndAnimationEvent()).AddTo(this);
    }

    /// <summary>
    /// タックル攻撃
    /// </summary>
    void SettingTackleAnimation()
    {
        var baseIndex = m_animator.GetLayerIndex("Base Layer");
        var upperIndex = m_animator.GetLayerIndex("Upper Layer");
        m_tackleComp.state.Where(_ => m_tackleComp.state.Value == TankTackle.State.TackleLast)
            .Subscribe(_ => CrossFadeState("TackleLast", baseIndex))
            .AddTo(this);

        var tackle = ZombieTankTable.BaseLayer.TackleAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        tackle.onStateEntered.Subscribe(_ => m_tackleComp.AttackStart()).AddTo(this);

        var actionBehaviour = ZombieTankTable.BaseLayer.TackleAttack.GetBehaviour<AnimationActionBehavior>(m_animator);
        actionBehaviour.AddFirstTransitionAction(() => {
            float speed = m_animator.speed;
            m_animator.speed = 0.0f;
            m_waitTimer.AddWaitTimer(GetType(), m_tackleParam.chargeTime, () => TackleStart(speed));
        });

        //Time系
        var timeEvent = tackle.onTimeEvent;
    }

    void TackleStart(float speed)
    {
        m_animator.speed = speed;
        int upperIndex = m_animator.GetLayerIndex("Upper Layer");
        m_tackleComp.TackleStart();
        CrossFadeState("Idle", upperIndex, 0.0f);
        CrossFadeTackle();
    }

    /// <summary>
    /// タックル終了攻撃
    /// </summary>
    void SettingTackleLastAnimation()
    {
        var tackle = ZombieTankTable.BaseLayer.TackleLast.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        
        tackle.onStateExited.Subscribe(_ => m_tackleComp.EndAnimationEvent());
    }


    //クロスフェード--------------------------------------------------------------------------

    public void CrossFadeNormalAttack(float transitionTime = 0.25f)
    {
        int layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("NormalAttack", layerIndex, transitionTime);
    }

    public void CrossFadeTackle(float transitionTime = 0.25f)
    {
        int layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("TackleAttack", layerIndex, transitionTime);
    }

    public void CrossFadeTackleCharge(float transitionTime = 0.5f)
    {
        int layerIndex = m_animator.GetLayerIndex("Upper Layer");
        CrossFadeState("TackleCharge", layerIndex, transitionTime);
    }

    //アクセッサ・プロパティ--------------------------------------------------------------------

    public float TackleSpeed
    {
        get => m_animator.GetFloat("tackleSpeed");
        set => m_animator.SetFloat("tackleSpeed", value);
    }
}
