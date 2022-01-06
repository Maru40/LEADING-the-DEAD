using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SignalGunCtrl : MonoBehaviour
{
    [SerializeField]
    private Player.PlayerAnimatorManager m_animatorManager;

    [SerializeField]
    private Player.PlayerStatusManager m_statusManager;

    [SerializeField]
    private Flaregun m_flareGun;

    [SerializeField,Min(0.0f)]
    private float m_coolTime = 1.0f;

    private float m_nowCountTime = 1.0f;

    [SerializeField]
    private float m_animationShotDelay = 0.0f;

    private ReactiveProperty<float> m_coolTimePercentage = new ReactiveProperty<float>();

    public System.IObservable<float> coolTimePercentageOnChanged => m_coolTimePercentage;

    private Subject<Unit> m_shotStartSubject = new Subject<Unit>();
    private Subject<Unit> m_shotSubject = new Subject<Unit>();

    private GameControls m_gameControls;

    private void Awake()
    {
        m_nowCountTime = m_coolTime;

        this.UpdateAsObservable()
            .Where(_ => m_nowCountTime < m_coolTime)
            .Subscribe(_ => { m_nowCountTime += Time.deltaTime;m_coolTimePercentage.Value = m_nowCountTime / m_coolTime; })
            .AddTo(this);

        var shotBehaviour =
            PlayerMotionsTable.Upper_Layer.Shot.GetBehaviour<TimeEventStateMachineBehaviour>(m_animatorManager.animator);

        shotBehaviour.onStateEntered
            .Subscribe(_ => { m_animatorManager.isUseActionMoving = true; m_flareGun.gameObject.SetActive(true); });

        shotBehaviour.onStateExited
            .Subscribe(_ => { m_animatorManager.isUseActionMoving = false; m_flareGun.gameObject.SetActive(false); });

        shotBehaviour.onTimeEvent.ClampWhere(m_animationShotDelay)
            .Where(_ => m_statusManager.isControllValid)
            .Subscribe(_ => OnShot())
            .AddTo(this);

        m_shotStartSubject.Subscribe(_ => m_animatorManager.isUseActionMoving = true);

        m_shotSubject
            .Where(_ => m_nowCountTime >= m_coolTime && !m_animatorManager.isUseActionMoving)
            .Subscribe(_ => OnUse())
            .AddTo(this);

        m_gameControls = new GameControls();

        m_gameControls.Player.GunShot.performed += _ => m_shotSubject.OnNext(Unit.Default);

        this.RegisterController(m_gameControls);
    }

    private void Start()
    {
    }

    protected void OnUse()
    {
        m_nowCountTime = 0.0f;

        m_animatorManager.GoState("Shot", "Upper_Layer", 0.1f);
    }

    private void OnShot()
    {
        m_flareGun.Shoot();

        m_animatorManager.GoState("Idle", "Upper_Layer", 0.1f);
    }
}
