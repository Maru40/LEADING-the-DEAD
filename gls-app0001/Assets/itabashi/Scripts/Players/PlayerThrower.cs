using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerThrower : MonoBehaviour
    {
        /// <summary>
        /// ゲームの入力
        /// </summary>
        private GameControls m_gameControls;

        [SerializeField]
        private PlayerAnimatorManager m_animatorManager;

        [SerializeField]
        PlayerPickUpper m_playerpickUpper;

        [SerializeField]
        PlayerStatusManager m_playerStatusManager;
        /// <summary>
        /// オブジェクト打ち出し装置
        /// </summary>
        [SerializeField]
        ObjectLauncher m_objectLauncher;

        [SerializeField]
        private float m_coolTime = 0.0f;

        [SerializeField]
        private float m_minThrowRotateX = -90.0f;

        [SerializeField]
        private float m_maxThrowRotateX = 0.0f;

        private float m_throwRotateX = 45.0f;

        [SerializeField]
        private AudioSource m_audioSource;

        [SerializeField]
        private AudioClip m_throwingSound;

        private float m_countCoolTime;

        private ThrowableObject m_takingObject;
        private PickedUpObject m_pickedUpObject;

        private bool m_isThrowingStance = false;

        private bool m_isThrowing = false;

        private Subject<bool> m_throwingStanceSubject = new Subject<bool>();
        private Subject<Unit> m_OnThrowingSubject = new Subject<Unit>();

        private void OnValidate()
        {
            m_minThrowRotateX = Mathf.Clamp(m_minThrowRotateX, -90, 0);
            m_maxThrowRotateX = Mathf.Clamp(m_maxThrowRotateX, m_minThrowRotateX, 0);
            m_minThrowRotateX = Mathf.Clamp(m_minThrowRotateX, -90, m_maxThrowRotateX);
        }


        private void Awake()
        {

            m_throwingStanceSubject.Where(isThrowingStance => isThrowingStance)
                .Where(_ => !m_animatorManager.isUseActionMoving && m_playerStatusManager.isControllValid)
                .Subscribe(_ => ThrowingStanceStart());

            m_throwingStanceSubject.Where(isThrowingStance => !isThrowingStance)
                .Where(_ => m_isThrowingStance && !m_isThrowing)
                .Subscribe(_ => { ThrowingStanceEnd(); m_animatorManager.GoState("Idle", "Upper_Layer"); });

            m_OnThrowingSubject
                .Where(_ => m_isThrowingStance && !m_isThrowing)
                .Subscribe(_ => Throwing())
                .AddTo(this);

            var throwingStanceBehaviour =
                PlayerMotionsTable.Upper_Layer.Throw.ThrowingStance.GetBehaviour<TimeEventStateMachineBehaviour>(m_animatorManager.animator);

            throwingStanceBehaviour.onStateEntered.Subscribe(_ => m_animatorManager.isUseActionMoving = true);
            throwingStanceBehaviour.onStateExited
                .Where(_ => !m_isThrowing)
                .Subscribe(_ => m_animatorManager.isUseActionMoving = false);

            var throwingBehaviour =
                PlayerMotionsTable.Upper_Layer.Throw.Throwing.GetBehaviour<TimeEventStateMachineBehaviour>(m_animatorManager.animator);

            throwingBehaviour.onStateEntered.Subscribe(_ => m_isThrowing = true);
            throwingBehaviour.onStateEntered.Subscribe(_ => m_isThrowingStance = false);
            throwingBehaviour.onStateExited.Subscribe(_ => m_isThrowing = false);
            throwingBehaviour.onStateExited.Subscribe(_ => m_animatorManager.isUseActionMoving = false);

            m_gameControls = new GameControls();

            m_gameControls.Player.ThrowingStance.performed += context => m_throwingStanceSubject.OnNext(true);
            m_gameControls.Player.ThrowingStance.canceled += context => m_throwingStanceSubject.OnNext(false);

            m_gameControls.Player.Throwing.performed += context => m_OnThrowingSubject.OnNext(Unit.Default);

            this.RegisterController(m_gameControls);
        }

        // Start is called before the first frame update
        void Start()
        {
            m_countCoolTime = m_coolTime;
        }

        // Update is called once per frame
        void Update()
        {
            m_countCoolTime += Time.deltaTime;

            if(!m_playerStatusManager.isControllValid && m_isThrowingStance)
            {
                ThrowingStanceEnd();
                return;
            }

            ThrowAngleControl();
        }

        void ThrowingStanceStart()
        {

            var cymbalMonkeyList = m_playerpickUpper.GetPickedUpObjectList("CymbalMonkey");

            if (cymbalMonkeyList.Count == 0)
            {
                return;
            }

            ThrowableObject throwableObject = cymbalMonkeyList[0].GetComponent<ThrowableObject>();

            if (m_countCoolTime < m_coolTime || !throwableObject)
            {
                return;
            }

            m_takingObject = throwableObject;
            m_pickedUpObject = cymbalMonkeyList[0];

            m_isThrowingStance = true;

            throwableObject.gameObject.SetActive(true);
            throwableObject.gameObject.transform.SetParent(transform);
            throwableObject.gameObject.transform.position = m_objectLauncher.transform.position;
            throwableObject.gameObject.transform.localRotation = Quaternion.identity;
            throwableObject.Takeing();

            m_objectLauncher.isDrawPredictionLine = true;


            m_animatorManager.GoState("ThrowingStance", "Upper_Layer");

            var cymbalMonkeyStateManager = throwableObject.GetComponent<RadioStateManager>();

            if (cymbalMonkeyStateManager)
            {
                cymbalMonkeyStateManager.alarmSwitch = true;
            }
        }

        void ThrowingStanceEnd()
        {
            m_takingObject = null;

            if (m_pickedUpObject)
            {
                m_pickedUpObject.gameObject.SetActive(false);
                m_pickedUpObject.transform.SetParent(m_pickedUpObject.transform);
                m_pickedUpObject = null;
            }

            m_isThrowingStance = false;

            m_objectLauncher.isDrawPredictionLine = false;
        }

        private void Throwing()
        {
            m_countCoolTime = 0.0f;

            m_takingObject.transform.SetParent(null);

            m_playerpickUpper.TakeOut("CymbalMonkey");

            m_objectLauncher.Fire(m_takingObject, m_takingObject.transform.rotation);

            m_takingObject = null;
            m_pickedUpObject = null;

            m_isThrowing = true;

            m_animatorManager.GoState("Throwing", "Upper_Layer", 0.1f);

            m_objectLauncher.isDrawPredictionLine = false;

            m_audioSource.PlayOneShot(m_throwingSound);
        }

        void ThrowAngleControl()
        {
            if(!m_isThrowingStance)
            {
                return;
            }

            float aimRotateX = m_gameControls.Player.ThrowAim.ReadValue<float>();

            m_throwRotateX += aimRotateX * 90.0f * Time.deltaTime;

            m_throwRotateX = Mathf.Clamp(m_throwRotateX, m_minThrowRotateX, m_maxThrowRotateX);

            m_objectLauncher.transform.localRotation = Quaternion.Euler(m_throwRotateX, 0, 0);
        }
    }
}
