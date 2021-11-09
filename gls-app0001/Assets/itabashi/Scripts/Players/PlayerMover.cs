using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Abnormal;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private PlayerAnimatorManager m_animatorManager;

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
            this.RegisterController(m_gameControls);

            m_rigitbody = GetComponent<Rigidbody>();
            m_playerStatusManager = GetComponent<PlayerStatusManager>();
        }

        private void OnDisable()
        {
            m_animatorManager.moveInput = 0.0f;

            m_rigitbody.velocity = new Vector3(0.0f, m_rigitbody.velocity.y, 0.0f);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(m_playerStatusManager.isHitStoped)
            {
                m_rigitbody.velocity = new Vector3(0.0f, m_rigitbody.velocity.y, 0.0f);
                return;
            }

            if(m_playerStatusManager.isStun)
            {
                m_animatorManager.moveInput = 0.0f;

                m_rigitbody.velocity = new Vector3(0.0f, m_rigitbody.velocity.y, 0.0f);

                return;
            }
            
            if(GameTimeManager.isPause)
            {
                return;
            }

            var moveVector2 = m_gameControls.Player.Move.ReadValue<Vector2>();

            var camera = Manager.GameCameraManager.current;

            var forward = camera.transform.forward;
            forward.y = 0;
            forward = forward.normalized;

            // カメラが真上か真下にある場合
            if (forward.magnitude == 0.0f)
            {
                forward = camera.transform.position.y - transform.position.y > 0 ? camera.transform.up : -camera.transform.up;
            }

            bool canDash = m_gameControls.Player.Dash.IsPressed() && m_playerStatusManager.stamina > 0;

            m_animatorManager.canDash = canDash;

            var moveVector3 = camera.transform.right * moveVector2.x + forward * moveVector2.y;

            float moveInput = moveVector3.magnitude;

            m_animatorManager.moveInput = moveInput;

            if (moveInput > m_moveSpeedDeadZone)
            {
                transform.rotation = Quaternion.LookRotation(moveVector3);
            }
            else
            {
                m_animatorManager.moveInput = 0.0f;
            }

            var moveSpeed = canDash ? m_moveSpeed * m_dashSpeedScale : m_moveSpeed;

            if (canDash && m_animatorManager.moveInput > 0.0f && m_playerStatusManager.stamina > 0)
            {
                moveVector3 = moveVector3.normalized;

                m_playerStatusManager.stamina -= m_dashStaminaToOneSecond * Time.deltaTime;
            }

            m_rigitbody.velocity = moveVector3 * moveSpeed + new Vector3(0.0f, m_rigitbody.velocity.y, 0.0f);
        }
    }
}
