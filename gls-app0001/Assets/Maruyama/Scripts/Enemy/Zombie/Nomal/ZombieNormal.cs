using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FoundType = FoundObject.FoundType;

public class ZombieNormal : EnemyBase, I_Chase, I_Listen, I_BindedActiveArea, I_Smell, I_Eat, I_BindPlowlingArea
{
    //test用に表示したり、消したりする用。
    [SerializeField]
    private GameObject m_tempPrehab = null;
    private GameObject m_tempObject = null;

    //コンポーネント系

    private Stator_ZombieNormal m_stator;
    private AnimatorCtrl_ZombieNormal m_animator;
    private TargetManager m_targetManager;

    private RandomPlowlingMove m_randomPlowling;
    private ThrongManager m_throngManager;
    private StatusManager_ZombieNormal m_statusManager;

    private void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_animator = GetComponent<AnimatorCtrl_ZombieNormal>();
        m_targetManager = GetComponent<TargetManager>();

        m_randomPlowling = GetComponent<RandomPlowlingMove>();
        m_throngManager = GetComponent<ThrongManager>();
        m_statusManager = GetComponent<StatusManager_ZombieNormal>();
    }

    private void Start()
    {
        SettingChangeTargetEvent();
    }

    private void SettingChangeTargetEvent()
    {
        m_targetManager.AddChangeTargetEvent(FoundType.Player, () => m_stator.GetTransitionMember().findTrigger.Fire());
        m_targetManager.AddChangeTargetEvent(FoundType.ChildZombie, () => m_stator.GetTransitionMember().findTrigger.Fire());
    }

    //インターフェースの実装-------------------------------------------------

    void I_Chase.ChangeState(){
        m_stator.GetTransitionMember().findTrigger.Fire();
    }

    void I_Chase.TargetLost()
    {
        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }

    void I_Listen.Listen(FoundObject foundObject) {
        //ターゲットの切替
        m_targetManager.SetNowTarget(GetType() ,foundObject);

        var member = m_stator.GetTransitionMember();
        member.findTrigger.Fire();
    }

    void I_BindedActiveArea.Bind(BindActivateArea bind)
    {
        m_randomPlowling.SetCenterObject(bind.GetAreaCenterObject());
        m_randomPlowling.SetRandomPositionRadius(bind.GetBindRange());

        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }

    void I_BindedActiveArea.BindRelease(BindActivateArea bind)
    {
        m_randomPlowling.ResetCenterObject();
    }

    void I_Smell.SmellFind(FoundObject foundObject)
    {
        if(m_targetManager.SetNowTarget(GetType(), foundObject))
        {
            m_stator.GetTransitionMember().findTrigger.Fire();
        }
    }

    public void Eat()
    {
        if (m_statusManager.IsEat) {
            return;
        }

        Debug.Log("食べる");
        m_statusManager.IsEat = true;
        m_stator.GetTransitionMember().eatTrigger.Fire();
    }

    public void InBind(BindPlowlingAreaManager manager)
    {
        m_randomPlowling.SetCenterObject(manager.CenterObject);
        m_randomPlowling.SetRandomPositionRadius(manager.BindRange);
    }

    public void OutBind(BindPlowlingAreaManager manager)
    {
        m_randomPlowling.ResetCenterObject();
    }
}
