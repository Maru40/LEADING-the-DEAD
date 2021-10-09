using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerBatAttacker : PlayerActionBase
    {
        [SerializeField]
        private PlayerAnimationParameters m_playerParameters;

        [SerializeField]
        private WeaponBase m_weaponBase;

        [SerializeField]
        private bool m_hasReady = false;

        private bool m_canSwing = true;

        private Subject<Unit> m_swingSubject = new Subject<Unit>();
        private Subject<Unit> m_swingEndSubject = new Subject<Unit>();

        private GameControls m_gameControls;

        private void Awake()
        {
            m_canSwing = !m_hasReady;

            m_swingSubject
                .Where(_ => (!m_hasReady || m_canSwing) && !m_playerStatusManager.isStun)
                .Subscribe(_ => SwingStart());

            m_swingEndSubject
                .Delay(System.TimeSpan.FromSeconds(0.75f))
                .Subscribe(_ => m_weaponBase.gameObject.SetActive(false));

            m_gameControls = new GameControls();

            m_gameControls.Player.BatSwing.performed += _ => m_swingSubject.OnNext(Unit.Default);

            this.RegisterController(m_gameControls);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        private void SwingStart()
        {
            m_playerParameters.Swing();
            m_weaponBase.gameObject.SetActive(true);
            m_swingEndSubject.OnNext(Unit.Default);
        }
    }
}