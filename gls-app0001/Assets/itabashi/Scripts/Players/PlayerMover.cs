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
        /// �Q�[���̓���
        /// </summary>
        GameControls m_gameControls;

        /// <summary>
        /// �v���C���[�L�����N�^�[�̈ړ����x
        /// </summary>
        [SerializeField]
        private float m_moveSpeed = 1.0f;

        /// <summary>
        /// �ړ����͂̒l�������艺�������ꍇ��������
        /// </summary>
        [SerializeField]
        private float m_moveSpeedDeadZone = 0.01f;

        private Rigidbody m_rigitbody;

        private void Awake()
        {
            m_gameControls = new GameControls();
            m_rigitbody = GetComponent<Rigidbody>();
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

            // �J�������^�ォ�^���ɂ���ꍇ
            if (forward.magnitude == 0.0f)
            {
                forward = camera.transform.position.y - transform.position.y > 0 ? camera.transform.up : -camera.transform.up;
            }

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

            m_rigitbody.velocity = moveVector3 * m_moveSpeed;
        }
    }
}