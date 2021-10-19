using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using AttributeObject;

namespace Player
{
    public class PlayerBatAttacker : PlayerActionBase
    {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private PlayerAnimatorManager m_animatorManager;

        [SerializeField]
        private PlayerStatusManager m_statusManager;

        [SerializeField]
        private WeaponBase m_weaponBase;

        private Subject<Unit> m_swingSubject = new Subject<Unit>();

        private GameControls m_gameControls;

        private void Awake()
        {
            var batSwingBehaviour = m_animatorManager.behaviourTable["Upper_Layer.Swing.Swing"];

            m_swingSubject
                .Where(_ => !m_animatorManager.isUseActionMoving && !m_playerStatusManager.isStun)
                .Subscribe(_ => m_animatorManager.GoState("Swing", "Upper_Layer"));

            batSwingBehaviour.onStateEntered.Subscribe(_ => m_weaponBase.gameObject.SetActive(true));
            batSwingBehaviour.onStateEntered.Subscribe(_ => m_animatorManager.isUseActionMoving = true);

            batSwingBehaviour.onStateExited.Subscribe(_ => m_weaponBase.gameObject.SetActive(false));
            batSwingBehaviour.onStateExited.Subscribe(_ => m_weaponBase.HitClear());
            batSwingBehaviour.onStateExited.Subscribe(_ => m_animatorManager.isUseActionMoving = false);

            m_gameControls = new GameControls();

            m_gameControls.Player.BatSwing.performed += _ => m_swingSubject.OnNext(Unit.Default);

            this.RegisterController(m_gameControls);
        }

        public void OnBatHited(TakeDamageObject takeDamageObject,DamageData damageData)
        {
            StartCoroutine(HitStop(damageData.hitStopTime));
        }

        private IEnumerator HitStop(float hitStopTime)
        {
            float animatorSpeed = m_animator.speed;

            m_animator.speed = 0.0f;
            m_statusManager.isHitStoped = true;
            m_weaponBase.attackColliderEnabled = false;

            float countHitStopTime = 0.0f;

            while (countHitStopTime < hitStopTime)
            {
                countHitStopTime += Time.deltaTime;
                Debug.Log("こんばんが");
                yield return null;
            }

            m_animator.speed = animatorSpeed;
            m_statusManager.isHitStoped = false;
            m_weaponBase.attackColliderEnabled = true;
        }
    }
}