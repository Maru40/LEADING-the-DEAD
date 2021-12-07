using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

using MaruUtility;
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

    public enum NormalAttackHitColliderType
    {
        Left,
        Right
    }

    [SerializeField]
    Ex_Dictionary<NormalAttackHitColliderType, AnimationHitColliderParametor> m_normalAttackParam =
        new Ex_Dictionary<NormalAttackHitColliderType, AnimationHitColliderParametor>();

    [SerializeField]
    AnimationHitColliderParametor m_dashAttackParam = new AnimationHitColliderParametor();

    [SerializeField]
    AnimationHitColliderParametor m_wallAttackParam = new AnimationHitColliderParametor();

    [Header("壁攻撃ダメ―ジ与える時間"), SerializeField]
    List<AnimationHitColliderParametor> m_putWallParams = new List<AnimationHitColliderParametor>();

    Dictionary<StateEnum, string> m_stateNameDictionary = new Dictionary<StateEnum, string>();

    NormalAttack m_normalAttackComp;

    EnemyStunManager m_stunManager;
    AngerManager m_angerManager;
    KnockBackManager m_knockBackManager;
    EnemyRotationCtrl m_rotationController;
    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    AttackManager_ZombieNormal m_attackManager;
    Stator_ZombieNormal m_stator;
    ThrongManager m_throngManager;

    [SerializeField]
    AudioManager m_preliminaryNormalAttackVoice;

    override protected void Awake()
    {
        base.Awake();

        m_normalAttackParam.InsertInspectorData();

        m_normalAttackComp = GetComponent<NormalAttack>();
        m_stunManager = GetComponent<EnemyStunManager>();
        m_angerManager = GetComponent<AngerManager>();
        m_knockBackManager = GetComponent<KnockBackManager>();
        m_rotationController = GetComponent<EnemyRotationCtrl>();
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_attackManager = GetComponent<AttackManager_ZombieNormal>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_throngManager = GetComponent<ThrongManager>();
    }

    protected override void Start()
    {
        base.Start();

        SettingNormalAttack();
        SettingPreliminaryNormalAttack();

        SettingDashAttack();
        SettingDashAttackMove();

        SettingWallAttack();
        SettingPutWallAttack();

        SettingEat();

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
            .Subscribe(_ => leftTimeParam.trigger.AttackEnd())
            .AddTo(this);

        //右腕のコライダーのOn、Off
        timeEvent.ClampWhere(rightTimeParam.startTime)
            .Subscribe(_ => rightTimeParam.trigger.AttackStart())
            .AddTo(this);
        timeEvent.ClampWhere(rightTimeParam.endTime)
            .Subscribe(_ => rightTimeParam.trigger.AttackEnd())
            .AddTo(this);

        behavior.onStateEntered.Subscribe(_ => m_normalAttackComp.AttackStart()).AddTo(this);

        behavior.onStateExited.Subscribe(_ => m_normalAttackComp.EndAnimationEvent()).AddTo(this);
    }

    /// <summary>
    /// 通常攻撃の予備動作
    /// </summary>
    void SettingPreliminaryNormalAttack()
    {
        var actionBehaviour = ZombieNormalTable.UpperLayer.PreliminaryNormalAttack.GetBehaviour<AnimationActionBehavior>(m_animator);

        actionBehaviour.AddEnterAction(() => { 
            m_rotationController.enabled = true; //ローテーションのenableをtrue

        });  

        actionBehaviour.AddUpdateAction(() => {
            //予備動作中のアップデート
            var vectorCheck = m_targetManager.GetToNowTargetVector();
            if(vectorCheck == null) {
                return;
            }
            var toTragetVec = (Vector3)vectorCheck;

            m_rotationController.SetDirect(toTragetVec);  //回転計算
            //速度計算
            var moveSpeed = m_attackManager.PreliminaryParametorProperty.moveSpeed;
            var force = CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toTragetVec, moveSpeed);
            m_velocityManager.AddForce(force); 
        });

        actionBehaviour.AddExitAction(() => m_preliminaryNormalAttackVoice?.FadeOutStart());
    }

    void SettingDashAttack()
    {
        //攻撃判定の入れだし
        var timeBehaviour = ZombieNormalTable.UpperLayer.DashAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        timeBehaviour.onTimeEvent.ClampWhere(m_dashAttackParam.startTime)
            .Subscribe(_ => m_dashAttackParam.trigger.AttackStart())
            .AddTo(this);

        timeBehaviour.onTimeEvent.ClampWhere(m_dashAttackParam.endTime)
            .Subscribe(_ => m_dashAttackParam.trigger.AttackEnd())
            .AddTo(this);
    }

    void SettingDashAttackMove()
    {
        
    }

    void SettingWallAttack()
    {
        var timeBehaviour = ZombieNormalTable.UpperLayer.WallAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        var timeEvent = timeBehaviour.onTimeEvent;

        timeEvent.ClampWhere(m_wallAttackParam.startTime)
            .Subscribe(_ => m_wallAttackParam.trigger.AttackStart())
            .AddTo(this);

        timeEvent.ClampWhere(m_wallAttackParam.endTime)
            .Subscribe(_ => m_wallAttackParam.trigger.AttackEnd())
            .AddTo(this);

        timeBehaviour.onStateExited
            .Subscribe(_ => m_wallAttackParam.trigger.AttackEnd())
            .AddTo(this);
    }

    void SettingPutWallAttack()
    {
        var timeBehaviour = ZombieNormalTable.UpperLayer.PutWallAttack.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        var timeEvent = timeBehaviour.onTimeEvent;

        foreach(var param in m_putWallParams)
        {
            timeEvent.ClampWhere(param.startTime)
                .Subscribe(_ => param.trigger.AttackStart()) 
                .AddTo(this);

            timeEvent.ClampWhere(param.endTime)
                .Subscribe(_ => param.trigger.AttackEnd())   
                .AddTo(this);
        }

        timeBehaviour.onStateEntered  //集団行動Off
            .Subscribe(_ => { 
                m_throngManager.enabled = false;
                m_velocityManager.ResetAll();
            }) 
            .AddTo(this);

        timeBehaviour.onStateExited
            .Subscribe(_ => m_throngManager.enabled = true)  //集団行動On
            .AddTo(this);
    }

    void SettingEat()
    {
        var timeBehaviour = ZombieNormalTable.AllLayer.Eat.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        var timeEvent = timeBehaviour.onTimeEvent;

        //const float time = 3.0f; //将来的にインスぺクタから変更
        List<float> times = new List<float>();
        times.Add(3.0f);
        times.Add(11.0f);

        foreach(var time in times)
        {
            timeEvent.ClampWhere(time)
                .Subscribe(_ => {
                    Eat();
            })
            .AddTo(this);
        }

        timeBehaviour.onStateEntered  //スタート時のEat状態にする。
            .Subscribe(_ => {
                m_statusManager.IsEat = true;
            })
            .AddTo(this);

        timeBehaviour.onStateExited
            .Subscribe(_ => { 
                m_animator.SetBool("isEat", false);
                m_statusManager.IsEat = false;
            })
            .AddTo(this);
    }

    void Eat()
    {
        if (!m_targetManager.HasTarget())  {
            return;
        }

        var eaten = m_targetManager.GetNowTarget().GetComponent<EatenBase>();
        if (eaten)
        {
            eaten.Eaten();
            Debug.Log("△食べたよ");
        }
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

        var timeBehaviour = ZombieNormalTable.BaseLayer.Land.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);
        timeBehaviour.onStateExited
            .Subscribe(_ => m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire())
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

    public void CrossFadeNormalAttackAnimation()
    {
        CrossFadeState("NormalAttack", UpperLayerIndex);
    }

    /// <summary>
    /// 通常攻撃の予備動作
    /// </summary>
    public void CrossFadePreliminaryNormalAttackAniamtion(float transitionTime = 0.75f)
    {
        CrossFadeState("PreliminaryNormalAttack", UpperLayerIndex, transitionTime);
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

    public void CrossFadeIdleAnimation(int layerIndex, float transitionTime = 0.05f)
    {
        CrossFadeState("Idle", layerIndex, transitionTime);
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

    public void CrossFadeEatAnimation(float transitionTime = 0.25f)
    {
        CrossFadeState("Eat", AllLayerIndex, transitionTime);
        m_animator.SetBool("isEat", true);
    }

    public void CrossFadeDashAttack(float transitionTime = 0.1f)
    {
        CrossFadeState("DashAttack", UpperLayerIndex, transitionTime);
    }

    public void CrossFadeDashAttackMove(float transitionTime = 0.5f)
    {
        CrossFadeState("DashAttackWalk", UpperLayerIndex, transitionTime);
    }

    public void CrossFadeWallAttack(float transitionTime = 0.25f)
    {
        CrossFadeState("WallAttack", UpperLayerIndex, transitionTime);
    }

    public void CrossFadePutWallAttack(float transitionTime = 0.25f)
    {
        CrossFadeState("PutWallAttack", UpperLayerIndex, transitionTime);
    }

    //アクセッサ・プロパティ---------------------------------------------------------------------------------

    /// <summary>
    /// 通常攻撃のトリガーのそれぞれの攻撃力を設定できるようにした。
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    public void SetNormalAttackDamageData(NormalAttackHitColliderType type, AttributeObject.DamageData data)
    {
        m_normalAttackParam[type].trigger.damageData = data;
    }

    public int BaseLayerIndex => m_animator.GetLayerIndex("Base Layer");
    public int UpperLayerIndex => m_animator.GetLayerIndex("Upper Layer");
    public int AllLayerIndex => m_animator.GetLayerIndex("All Layer");

    public void Dying()
    {
        const float weight = 0.0f;
        m_animator.SetLayerWeight(UpperLayerIndex, weight);
        CrossFadeIdleAnimation(UpperLayerIndex);
    }

    public void Respawn()
    {
        const float weight = 1.0f;
        m_animator.SetLayerWeight(UpperLayerIndex, weight);
        CrossFadeIdleAnimation(UpperLayerIndex);
        CrossFadeIdleAnimation();
    }
}
