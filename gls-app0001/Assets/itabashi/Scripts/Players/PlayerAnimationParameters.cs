using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    [RequireComponent(typeof(Animator))]

    public class PlayerAnimationParameters : MonoBehaviour
    {
        private Animator m_playerAnimator;

        public float moveInput
        {
            set { m_playerAnimator.SetFloat("moveInput", value); }
            get { return m_playerAnimator.GetFloat("moveInput"); }
        }
        public float stamina
        {
            set { m_playerAnimator.SetFloat("stamina", value); }
            get { return m_playerAnimator.GetFloat("stamina"); }
        }

        public bool isDash
        {
            set { m_playerAnimator.SetBool("isDash", value); }
            get { return m_playerAnimator.GetBool("isDash"); }
        }

        /// <summary>
        /// Throwトリガー
        /// </summary>
        public void Throw() { m_playerAnimator.SetTrigger("Throw"); }

        public void Swing() { m_playerAnimator.SetTrigger("Swing"); }

        // Start is called before the first frame update
        void Start()
        {
            m_playerAnimator = GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            m_playerAnimator.ResetTrigger("Throw");
            m_playerAnimator.ResetTrigger("Swing");
        }
    }
}
