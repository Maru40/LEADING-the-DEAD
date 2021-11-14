using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;
using Utility;

namespace Player
{
    public class PlayerStatusManager : MonoBehaviour
    {
        [SerializeField]
        private GameStateManager m_gameStateManager;

        [SerializeField]
        private StunStar m_stunStar;

        [SerializeField]
        private Gauge m_hpGauge;

        [SerializeField]
        private FloatReactiveProperty m_hp = new FloatReactiveProperty(10);

        [SerializeField]
        private float m_maxHp;

        public float maxHp => m_maxHp;

        [SerializeField]
        private BoolReactiveProperty m_isStun = new BoolReactiveProperty(false);

        public bool isStun { private set => m_isStun.Value = value; get => m_isStun.Value; }

        private bool m_isHitStoped = false;

        public bool isHitStoped { set => m_isHitStoped = value; get => m_isHitStoped; }

        public bool isControllValid => !isStun && !isHitStoped && !isDead;

        [SerializeField]
        private float m_stunSecond = 1.0f;

        public float hp
        {
            set
            {
                m_hp.Value = Mathf.Max(value, 0.0f);

                m_hpGauge.fillAmount = m_hp.Value / m_maxHp;
            }

            get => m_hp.Value;
        }

        [SerializeField]
        private Gauge m_staminaGauge;

        [SerializeField]
        private float m_stamina;

        [SerializeField]
        private float m_maxStamina;

        public float stamina
        {
            set
            {
                m_stamina = Mathf.Clamp(value, 0.0f, m_maxStamina);
                m_staminaGauge.fillAmount = m_stamina / m_maxStamina;
            }
            get { return m_stamina; }
        }

        /// <summary>
        /// 一秒あたりのスタミナ回復量
        /// </summary>
        [SerializeField]
        private float m_staminaRecoveryPerSeconds = 0.0f;

        [SerializeField]
        private PlayerAnimatorManager m_animatorManager;


        private GameControls m_gameControls;

        private bool m_isDead = false;

        public bool isDead => m_isDead;

        [SerializeField]
        private bool m_isInvincible = false;

        public bool isInvincible { set => m_isInvincible = value; get => m_isInvincible; }

        [SerializeField]
        private UnityEvent m_deadStartEvent;

        [SerializeField]
        private UnityEvent m_deadEndEvent;

        readonly private Subject<Unit> m_endStunSubject = new Subject<Unit>();

        public IObservable<float> OnHpChanged => m_hp;

        public IObservable<bool> OnIsStanChanged => m_isStun;

        private void OnValidate()
        {
            m_maxStamina = Mathf.Max(m_maxStamina, 0.0f);
            m_stamina = Mathf.Clamp(m_stamina, 0.0f, m_maxStamina);
        }

        private void Awake()
        {
            m_gameControls = new GameControls();

            this.RegisterController(m_gameControls);

            OnHpChanged
                .Where(hp => hp == 0.0f)
                .Subscribe(_ => m_deadStartEvent?.Invoke()).AddTo(this);

            m_endStunSubject.Delay(TimeSpan.FromSeconds(m_stunSecond))
                .Subscribe(_ =>
                {
                    isStun = false;
                    m_stunStar.Stop();
                })
                .AddTo(this);

            m_deadStartEvent.AddListener(DeadStart);

            m_gameStateManager.OnChangedGameState
                .Where(gameState => gameState == GameState.Play)
                .Subscribe(_ => m_isInvincible = false)
                .AddTo(this);

            m_gameStateManager.OnChangedGameState
                .Where(gameState => gameState != GameState.Play)
                .Subscribe(_ => isInvincible = true)
                .AddTo(this);

            var deadBehaviour = PlayerMotionsTable.BaseLayer.Dead.GetBehaviour<TimeEventStateMachineBehaviour>(m_animatorManager.animator);

            deadBehaviour.onTimeEvent.ClampWhere(3.0f).Subscribe(_ => m_deadEndEvent?.Invoke()).AddTo(this);
        }

        private void DeadStart()
        {
            m_isDead = true;
            m_isInvincible = true;

            if(isStun)
            {
                isStun = false;
                m_stunStar.Stop();
            }

            m_animatorManager.GoState("Dead", "Base Layer", 0.0f);
            m_animatorManager.GoState("Idle", "Upper_Layer", 0.0f);
        }

        public void Clear()
        {
            
            m_isInvincible = true;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if(m_gameControls.Player.Dash.IsPressed() && m_animatorManager.moveInput > 0)
            {
                return;
            }

            stamina += m_staminaRecoveryPerSeconds * Time.deltaTime;

        }

        public void TakeDamage(AttributeObject.DamageData damageData)
        {
            if(m_isInvincible)
            {
                return;
            }

            hp -= damageData.damageValue;
            
            if(damageData.isStunAttack && !isDead)
            {
                StartStun();
            }
        }

        public void StartStun()
        {
            isStun = true;

            m_stunStar.Play();

            m_endStunSubject.OnNext(Unit.Default);
        }
    }
}