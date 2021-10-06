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
        private Gauge m_hpGauge;

        [SerializeField]
        private float m_hp;

        [SerializeField]
        private float m_maxHp;

        [SerializeField]
        private bool m_isStun = false;
        public bool isStun
        {
            private set
            {
                m_isStun = value;
                m_playerParameters.isStun = m_isStun;
            }

            get => m_isStun;
        }

        [SerializeField]
        private float m_stunSecond = 1.0f;

        public float hp
        {
            set
            {
                m_hp = Mathf.Max(value, 0.0f);

                m_hpGauge.fillAmount = m_hp / m_maxHp;

                if(m_hp == 0.0f)
                {
                    m_deadEvent.Invoke();
                }
            }

            get => m_hp;
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
                m_playerParameters.stamina = m_stamina;
                m_staminaGauge.fillAmount = m_stamina / m_maxStamina;
                m_updateStaminaEvent.Invoke(m_stamina);
            }
            get { return m_stamina; }
        }

        /// <summary>
        /// 一秒あたりのスタミナ回復量
        /// </summary>
        [SerializeField]
        private float m_staminaRecoveryPerSeconds = 0.0f;


        private PlayerAnimationParameters m_playerParameters;

        private GameControls m_gameControls;

        [SerializeField]
        private UnityEngine.Events.UnityEvent<float> m_updateStaminaEvent;

        [SerializeField]
        private UnityEngine.Events.UnityEvent m_deadEvent;

        private void OnValidate()
        {
            m_maxStamina = Mathf.Max(m_maxStamina, 0.0f);
            m_stamina = Mathf.Clamp(m_stamina, 0.0f, m_maxStamina);
        }

        private void Awake()
        {
            m_gameControls = new GameControls();

            this.RegisterController(m_gameControls);
        }

        private void Start()
        {
            m_playerParameters = GetComponent<PlayerAnimationParameters>();
        }

        private void Update()
        {
            if(m_gameControls.Player.Dash.IsPressed() && m_playerParameters.moveInput > 0)
            {
                return;
            }

            stamina += m_staminaRecoveryPerSeconds * Time.deltaTime;

        }

        public void TakeDamage(AttributeObject.DamageData damageData)
        {
            hp -= damageData.damageValue;
            
            if(damageData.isStunAttack)
            {
                StartStun();
            }
        }

        public void StartStun()
        {
            isStun = true;

            Observable.Timer(TimeSpan.FromSeconds(m_stunSecond)).Subscribe(_ => isStun = false).AddTo(this);
        }
    }
}
