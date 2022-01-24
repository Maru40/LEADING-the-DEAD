using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Cry : TaskNodeBase<EnemyBase>
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("立ち止まって泣く時間")]
        public float cryWaitTime;
        [Header("鳴き声の半径範囲")]
        public float range;
        [Header("鳴き声の効果範囲")]
        public SphereCollider collider;
        [Header("鳴き声パラメータ")]
        public List<AudioManager_Ex.Parametor> audioParams;
    }

    private Parametor m_param = new Parametor();

    private TriggerAction m_triggerAction = null;
    private FoundObject m_foundObject = null;
    private AudioManager_Ex m_audioManager = null;
    private TargetManager m_targetManager = null;

    private GameTimer m_timer = new GameTimer();

    public Task_Cry(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_foundObject = owner.GetComponent<FoundObject>();
        m_audioManager = owner.GetComponent<AudioManager_Ex>();

        m_triggerAction = m_param.collider.GetComponent<TriggerAction>();
        m_triggerAction.AddEnterAction(CrySoundHit);

        m_targetManager = owner.GetComponent<TargetManager>();
        m_targetManager.AddChangeTargetEvent(FoundObject.FoundType.None, () => m_param.collider.enabled = false);
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<FoundObject>(), true, true);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_param.collider.radius = m_param.range;
        m_param.collider.enabled = true;

        m_audioManager?.PlayRandomClipOneShot(m_param.audioParams);

        m_timer.ResetTimer(m_param.cryWaitTime);
    }

    public override bool OnUpdate()
    {
        m_timer.UpdateTimer();

        return m_timer.IsTimeUp;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    /// <summary>
    /// 鳴き声が届いた時
    /// </summary>
    /// <param name="other"></param>
    private void CrySoundHit(Collider other)
    {
        var targetManager = other.GetComponent<TargetManager>();

        if (targetManager)
        {
            targetManager.SetNowTarget(GetType(), m_foundObject);
        }
    }
}
