using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private PlayerAnimationParameters m_playerAnimationParamator;

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
            var moveVector3 = new Vector3(moveVector2.x, 0.0f, moveVector2.y);

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