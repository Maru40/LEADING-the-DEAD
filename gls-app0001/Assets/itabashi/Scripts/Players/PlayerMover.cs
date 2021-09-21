using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private PlayerAnimationParameters m_playerAnimationParamator;

        [SerializeField]
        private PlayerStatusManager m_playerStatusManager;

        /// <summary>
        /// ゲームの入力
        /// </summary>
        GameControls m_gameControls;

        /// <summary>
        /// プレイヤーキャラクターの移動速度
        /// </summary>
        [SerializeField]
        private float m_moveSpeed = 1.0f;

        /// <summary>
        /// ダッシュ倍率
        /// </summary>
        [SerializeField]
        private float m_dashSpeedScale = 2.0f;

        /// <summary>
        /// 一秒間にダッシュで減るスタミナ量
        /// </summary>
        [SerializeField]
        private float m_dashStaminaToOneSecond = 1.0f;

        /// <summary>
        /// 移動入力の値がこれより下だった場合無視する
        /// </summary>
        [SerializeField]
        private float m_moveSpeedDeadZone = 0.01f;

        private Rigidbody m_rigitbody;

        private void Awake()
        {
            m_gameControls = new GameControls();
            m_rigitbody = GetComponent<Rigidbody>();
            m_playerStatusManager = GetComponent<PlayerStatusManager>();
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

        }

        // Update is called once per frame
        void Update()
        {
            var moveVector2 = m_gameControls.Player.Move.ReadValue<Vector2>();

            var camera = Camera.main;

            var forward = camera.transform.forward;
            forward.y = 0;
            forward = forward.normalized;

            // カメラが真上か真下にある場合
            if (forward.magnitude == 0.0f)
            {
                forward = camera.transform.position.y - transform.position.y > 0 ? camera.transform.up : -camera.transform.up;
            }

            bool isDash = m_gameControls.Player.Dash.IsPressed() && m_playerStatusManager.stamina > 0;

            m_playerAnimationParamator.isDash = isDash;

            var moveVector3 = camera.transform.right * moveVector2.x + forward * moveVector2.y;

            float moveInput = moveVector3.magnitude;

            m_playerAnimationParamator.moveInput = moveInput;

            if (moveInput > m_moveSpeedDeadZone)
            {
                transform.rotation = Quaternion.LookRotation(moveVector3);
            }
            else
            {
                m_playerAnimationParamator.moveInput = 0.0f;
            }

            var moveSpeed = isDash ? m_moveSpeed * m_dashSpeedScale : m_moveSpeed;

            if (isDash && m_playerAnimationParamator.moveInput > 0.0f)
            {
                moveVector3 = moveVector3.normalized;

                m_playerStatusManager.stamina -= m_dashStaminaToOneSecond * Time.deltaTime;
            }

            m_rigitbody.velocity = moveVector3 * moveSpeed + new Vector3(0.0f, m_rigitbody.velocity.y, 0.0f);
        }
    }
}