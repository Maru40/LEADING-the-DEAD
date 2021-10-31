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

        [SerializeField]
        private float m_hitStartSecond = 0.0f;
        [SerializeField]
        private float m_hitEndSecond = 1.0f;

        private readonly Subject<Unit> m_swingSubject = new Subject<Unit>();

        private GameControls m_gameControls;

        private void Awake()
        {
            var batSwingBehaviour =
                PlayerMotionsTable.Upper_Layer.Swing.Swing.GetBehaviour<TimeEventStateMachineBehaviour>(m_animator);

            m_swingSubject
                .Where(_ => !m_animatorManager.isUseActionMoving && m_playerStatusManager.isControllValid)
                .Subscribe(_ => m_animatorManager.GoState("Swing", "Upper_Layer"))
                .AddTo(this);

            batSwingBehaviour.onTimeEvent
                .ClampWhere(m_hitStartSecond)
                .Subscribe(_ => m_weaponBase.attackColliderEnabled = true)
                .AddTo(this);

            batSwingBehaviour.onTimeEvent
                .ClampWhere(m_hitEndSecond)
                .Subscribe(_ => m_weaponBase.attackColliderEnabled = false)
                .AddTo(this);

            batSwingBehaviour.onStateEntered
                .Subscribe(_ =>
            {
                m_weaponBase.gameObject.SetActive(true);
                m_weaponBase.attackColliderEnabled = false;
                m_animatorManager.isUseActionMoving = true;
            }).AddTo(this);

            batSwingBehaviour.onStateExited.Subscribe(_ =>
            {
                m_weaponBase.gameObject.SetActive(false);
                m_weaponBase.HitClear();
                m_animatorManager.isUseActionMoving = false;
            }).AddTo(this);

            m_gameControls = new GameControls();

            m_gameControls.Player.BatSwing.performed += _ => m_swingSubject.OnNext(Unit.Default);

            this.RegisterController(m_gameControls);
        }

        public void OnBatHited(TakeDamageObject takeDamageObject, DamageData damageData)
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
                yield return null;
            }

            m_animator.speed = animatorSpeed;
            m_statusManager.isHitStoped = false;
            m_weaponBase.attackColliderEnabled = true;
        }
    }
}