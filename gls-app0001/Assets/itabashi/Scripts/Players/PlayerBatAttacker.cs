using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerBatAttacker : PlayerActionBase
    {
        [SerializeField]
        private PlayerAnimatorManager m_animatorManager;

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
            batSwingBehaviour.onStateExited.Subscribe(_ => m_animatorManager.isUseActionMoving = false);

            m_gameControls = new GameControls();

            m_gameControls.Player.BatSwing.performed += _ => m_swingSubject.OnNext(Unit.Default);

            this.RegisterController(m_gameControls);
        }

    }
}