using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;
using MaruUtility.UtilityDictionary;
using MaruUtility.Sound;

public class AnimatorManager_ZombieTank : AnimatorManagerBase
{

    [Serializable]
    struct TackleParametor
    {
        //public float tackleStartTime; //タックルスタート
        public float chargeTime;
        public float deselerationStartTime; //減速スタート
        public float attackEndTime;  //攻撃判定の終了する時間
        //足音を鳴らす時間
        public float rightFootStepsSoundTime;
        public float leftFootStepsSoundTime;
    }

    [Serializable]
    struct AudioParametor
    {
        public AudioManager shout;
        public AudioManager tackleFootSteps;  //タックル時の足音
    }

    [SerializeField]
    AnimationHitColliderParametor m_normalAttackParam = new AnimationHitColliderParametor();

    [SerializeField]
    TackleParametor m_tackleParam = new TackleParametor();

    [SerializeField]
    Ex_Dictionary<AttackManager_ZombieTank.AttackType, TimeParametor<ParticleManager.ParticleID>> m_attackParticleDictionary =
        new Ex_Dictionary<AttackManager_ZombieTank.AttackType, TimeParametor<ParticleManager.ParticleID>>();

    [SerializeField]
    AudioParametor m_audioParam = new AudioParametor();

    private NormalAttack m_normalAttackComp;
    private TankTackle m_tackleComp;
    private WaitTimer m_waitTimer;
    private AudioManager m_audioManager;

    override protected void Awake()
    {
        base.Awake();

        m_attackParticleDictionary.InsertInspectorData();

        m_waitTimer = GetComponent<WaitTimer>();
        m_normalAttackComp = GetComponent<NormalAttack>();
        m_tackleComp = GetComponent<TankTackle>();
        m_audioManager = GetComponent<AudioManager>();
        m_animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        SettingNormalAttackAnimation();
        SettingShoutAnimation();
        SettingTackleAnimation();
        SettingTackleLastAnimation();
        SettingDrumming();
        SettingShout();
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

        //パーティクルセット
        SettingTimeEventParticle(timeEvent,
            AttackManager_ZombieTank.AttackType.Near,
            m_normalAttackParam.trigger.gameObject);

        attack.onStateEntered.Subscribe(_ => m_normalAttackComp.AttackStart()).AddTo(this);
        attack.onStateExited.Subscribe(_ => m_normalAttackComp.EndAnimationEvent()).AddTo(this);
    }

    /// <summary>
    /// シャウトアニメーション
    /// </summary>
    void SettingShoutAnimation()
    {
        var timeEvent = ZombieTankTable.BaseLayer.Shout.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        //開始時に叫び声を上げる。
        timeEvent.onStateEntered
            .Subscribe(_ => { SEPlayOneShot(m_audioParam.shout); })
            .AddTo(this);
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

        //足音
        actionBehaviour.AddTimeAction(m_tackleParam.rightFootStepsSoundTime,
            () => { SEPlayOneShot(m_audioParam.tackleFootSteps); Debug.Log("△右足音"); }
        );
        actionBehaviour.AddTimeAction(m_tackleParam.leftFootStepsSoundTime,
            () => { SEPlayOneShot(m_audioParam.tackleFootSteps); Debug.Log("△左足音"); }
        );

        //timeEvent.ClampWhere(m_tackleParam.rightFootStepsSoundTime)
        //    .Subscribe(_ => SEPlayOneShot(m_audioParam.tackleFootSteps))
        //    .AddTo(this);
        //timeEvent.ClampWhere(m_tackleParam.leftFootStepsSoundTime)
        //    .Subscribe(_ => SEPlayOneShot(m_audioParam.tackleFootSteps))
        //    .AddTo(this);
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
    private void SettingTackleLastAnimation()
    {
        var timeBehaviour = ZombieTankTable.BaseLayer.TackleLast.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

        timeBehaviour.onTimeEvent.ClampWhere(m_tackleParam.attackEndTime)
            .Subscribe(_ => m_tackleComp.IsAttack = false)
            .AddTo(this);

        timeBehaviour.onStateExited.Subscribe(_ => m_tackleComp.EndAnimationEvent());
    }

    /// <summary>
    /// ドラミング攻撃
    /// </summary>
    void SettingDrumming()
    {
        var baseIndex = m_animator.GetLayerIndex("Base Layer");

        var shoutTimeBehaviour = ZombieTankTable.BaseLayer.Drumming.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        shoutTimeBehaviour.onStateEntered.
            Subscribe(_ => m_tackleComp.AttackStart()).AddTo(this);
    }

    /// <summary>
    /// シャウト
    /// </summary>
    void SettingShout()
    {
        var baseIndex = m_animator.GetLayerIndex("Base Layer");

        var shoutTimeBehaviour = ZombieTankTable.BaseLayer.Shout.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        shoutTimeBehaviour.onStateEntered.
            Subscribe(_ => m_tackleComp.AttackStart()).AddTo(this);
    }

    /// <summary>
    /// AniamtionEventで発生するParticleを設定
    /// </summary>
    /// <param name="timeEvent">TimeEvent</param>
    /// <param name="type">攻撃タイプ</param>
    /// <param name="owner">パーティクルが発生する場所</param>
    void SettingTimeEventParticle(IObservable<UniRxIObservableExtension.BeforeAfterData<float>> timeEvent,
        AttackManager_ZombieTank.AttackType type,
        GameObject owner)
    {
        if (m_attackParticleDictionary.ContainsKey(type))
        {
            var param = m_attackParticleDictionary[type];
            timeEvent.ClampWhere(param.time)
                .Subscribe(_ => ParticleManager.Instance.Play(param.value, owner.transform.position));
        }
    }

    void SEPlayOneShot(AudioManager manager)
    {
        manager.PlayOneShot();
    }

    void SEPlayOneShot(AudioClipParametor param)
    {
        Manager.GameAudioManager.Instance.SEPlayOneShot(param.clip, param.volume);
    }

    //クロスフェード--------------------------------------------------------------------------

    public void CrossFadeIdle(int layerIndex = 0, float transitionTime = 0.0f)
    {
        CrossFadeState("Idle" ,layerIndex, transitionTime);
    }

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

    public void CrossFadeDrumming(float transitionTime = 0.25f)
    {
        int layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("Drumming", layerIndex, transitionTime);
    }

    public void CrossFadeShout(float transitionTime = 0.25f)
    {
        int layerIndex = m_animator.GetLayerIndex("Base Layer");
        CrossFadeState("Shout", layerIndex, transitionTime);
    }

    public void CrossFadeLoopShout(float transitionTime = 0.0f)
    {
        CrossFadeState("LoopShout", BaseLayerIndex, transitionTime);
    }

    //アクセッサ・プロパティ--------------------------------------------------------------------

    public float TackleSpeed
    {
        get => m_animator.GetFloat("tackleSpeed");
        set => m_animator.SetFloat("tackleSpeed", value);
    }

    public int BaseLayerIndex => m_animator.GetLayerIndex("Base Layer");
}
