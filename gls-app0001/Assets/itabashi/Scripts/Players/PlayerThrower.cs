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

        [SerializeField]
        PlayerPickUpper m_playerpickUpper;

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
        private PickedUpObject m_pickedUpObject;

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
            if(!m_playerpickUpper.stackObject)
            {
                return;
            }

            ThrowableObject throwableObject = m_playerpickUpper.stackObject.GetComponent<ThrowableObject>();

            if (m_countCoolTime < m_coolTime || !throwableObject)
            {
                return;
            }

            m_takingObject = throwableObject;
            m_pickedUpObject = m_playerpickUpper.stackObject;

            m_isThrowingStance = true;
            m_playerParameters.isThrowingStance = true;

            throwableObject.gameObject.SetActive(true);
            throwableObject.gameObject.transform.SetParent(transform);
            throwableObject.gameObject.transform.position = m_objectLauncher.transform.position;
            throwableObject.gameObject.transform.localRotation = Quaternion.identity;
            throwableObject.Takeing();

            m_objectLauncher.isDrawPredictionLine = true;
        }

        void ThrowingStanceEnd()
        {
            m_isThrowingStance = false;
            m_playerParameters.isThrowingStance = false;

            m_takingObject = null;

            if (m_pickedUpObject)
            {
                m_pickedUpObject.gameObject.SetActive(false);
                m_pickedUpObject.transform.SetParent(m_pickedUpObject.transform);
                m_pickedUpObject = null;
            }

            m_objectLauncher.isDrawPredictionLine = false;
        }

        void Throwing()
        {
            if (!m_isThrowingStance)
            {
                return;
            }

            m_countCoolTime = 0.0f;

            m_takingObject.transform.SetParent(null);

            m_playerpickUpper.TakeOut();

            m_objectLauncher.Fire(m_takingObject, m_takingObject.transform.rotation);

            m_takingObject = null;
            m_pickedUpObject = null;

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
