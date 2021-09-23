using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerThrower : MonoBehaviour
    {
        /// <summary>
        /// ゲームの入力
        /// </summary>
        GameControls m_gameControls;

        /// <summary>
        /// プレイヤーキャラクターのアニメーションパラメーター
        /// </summary>
        [SerializeField]
        PlayerAnimationParameters m_playerParameters;

        /// <summary>
        /// 投げる対象のオブジェクト
        /// </summary>
        [SerializeField]
        ThrowableObject m_throwableObjectPrefab;

        /// <summary>
        /// オブジェクト打ち出し装置
        /// </summary>
        [SerializeField]
        ObjectLauncher m_objectLauncher;

        [SerializeField]
        private float m_coolTime = 0.0f;

        [SerializeField]
        private float m_minThrowRotateX = -90.0f;

        [SerializeField]
        private float m_maxThrowRotateX = 0.0f;

        private float m_throwRotateX = 45.0f;

        private float m_countCoolTime;

        private ThrowableObject m_takingObject;

        private bool m_isThrowingStance = false;

        private void OnValidate()
        {
            m_minThrowRotateX = Mathf.Clamp(m_minThrowRotateX, -90, 0);
            m_maxThrowRotateX = Mathf.Clamp(m_maxThrowRotateX, m_minThrowRotateX, 0);
            m_minThrowRotateX = Mathf.Clamp(m_minThrowRotateX, -90, m_maxThrowRotateX);
        }


        private void Awake()
        {
            m_gameControls = new GameControls();

            m_gameControls.Player.ThrowingStance.performed += context => ThrowingStanceStart();
            m_gameControls.Player.ThrowingStance.canceled += context => ThrowingStanceEnd();

            m_gameControls.Player.Throw.performed += context => Throwing();
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

        // Start is called before the first frame update
        void Start()
        {
            m_takingObject = Instantiate(m_throwableObjectPrefab, m_objectLauncher.transform.position, Quaternion.identity, transform);
            m_takingObject.Takeing();
            m_takingObject.gameObject.SetActive(false);

            m_countCoolTime = m_coolTime;
        }

        // Update is called once per frame
        void Update()
        {
            m_countCoolTime += Time.deltaTime;

            ThrowAngleControl();
        }

        void ThrowingStanceStart()
        {
            if (m_countCoolTime < m_coolTime)
            {
                return;
            }

            m_isThrowingStance = true;
            m_playerParameters.isThrowingStance = true;

            m_takingObject.gameObject.SetActive(true);

            m_objectLauncher.isDrawPredictionLine = true;
        }

        void ThrowingStanceEnd()
        {
            m_isThrowingStance = false;
            m_playerParameters.isThrowingStance = false;

            m_takingObject.gameObject.SetActive(false);

            m_objectLauncher.isDrawPredictionLine = false;
        }

        void Throwing()
        {
            if (!m_isThrowingStance)
            {
                return;
            }

            m_countCoolTime = 0.0f;

            m_objectLauncher.Fire(m_throwableObjectPrefab, m_takingObject.transform.rotation);
            m_takingObject.gameObject.SetActive(false);

            ThrowingStanceEnd();
        }

        void ThrowAngleControl()
        {
            if(!m_isThrowingStance)
            {
                return;
            }

            float aimRotateX = m_gameControls.Player.ThrowAim.ReadValue<float>();

            m_throwRotateX += aimRotateX * 90.0f * Time.deltaTime;

            m_throwRotateX = Mathf.Clamp(m_throwRotateX, m_minThrowRotateX, m_maxThrowRotateX);

            m_objectLauncher.transform.localRotation = Quaternion.Euler(m_throwRotateX, 0, 0);
        }
    }
}
