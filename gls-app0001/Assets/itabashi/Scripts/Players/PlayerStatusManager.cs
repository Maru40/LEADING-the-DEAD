using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStatusManager : MonoBehaviour
    {
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

        private void OnValidate()
        {
            m_maxStamina = Mathf.Max(m_maxStamina, 0.0f);
            m_stamina = Mathf.Clamp(m_stamina, 0.0f, m_maxStamina);
        }

        private void Awake()
        {
            m_gameControls = new GameControls();
        }

        private void OnEnable()
        {
            m_gameControls.Enable();
        }

        private void OnDisable()
        {
            m_gameControls.Disable();
        }

        private void OnDestroy()
        {
            m_gameControls.Disable();
        }

        private void Start()
        {
            m_playerParameters = GetComponent<PlayerAnimationParameters>();
        }

        private void Update()
        {
            if(m_gameControls.Player.Dash.IsPressed() && m_playerParameters.moveInput > 0)
            {
                Debug.Log("gaga");
                return;
            }

            stamina += m_staminaRecoveryPerSeconds * Time.deltaTime;
        }
    }
}