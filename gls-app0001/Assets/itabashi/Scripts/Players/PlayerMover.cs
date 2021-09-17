using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private PlayerAnimationParameters m_playerAnimationParamator;

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
        /// 移動入力の値がこれより下だった場合無視する
        /// </summary>
        [SerializeField]
        private float m_moveSpeedDeadZone = 0.01f;

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
            if(forward.magnitude == 0.0f)
            {
                forward = camera.transform.position.y - transform.position.y > 0 ? camera.transform.up : -camera.transform.up;
            }
            
            var moveVector3 = camera.transform.right * moveVector2.x + forward * moveVector2.y;

            float moveInput = moveVector3.magnitude;

            if (moveInput <= m_moveSpeedDeadZone)
            {
                m_playerAnimationParamator.moveInput = 0.0f;
                return;
            }


            m_playerAnimationParamator.moveInput = moveInput;

            moveInput *= m_moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(moveVector3);

            transform.Translate(new Vector3(0.0f, 0.0f, moveInput));

        }
    }
}