using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        public bool isThrowingStance
        {
            set { m_playerAnimator.SetBool("isThrowingStance", value); }
            get { return m_playerAnimator.GetBool("isThrowingStance"); }
        }

        /// <summary>
        /// Throw�g���K�[
        /// </summary>
        public void Throw() { m_playerAnimator.SetTrigger("Throw"); }

        // Start is called before the first frame update
        void Start()
        {
            m_playerAnimator = GetComponent<Animator>();
        }
    }
}