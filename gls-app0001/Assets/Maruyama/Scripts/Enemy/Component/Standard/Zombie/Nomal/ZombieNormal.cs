using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyBase, I_Chase, I_Listen, I_BindedActiveArea, I_Smell, I_Eat
{
    //test用に表示したり、消したりする用。
    [SerializeField]
    GameObject m_tempPrehab = null;
    GameObject m_tempObject = null;

    //コンポーネント系

    Stator_ZombieNormal m_stator;
    AnimatorCtrl_ZombieNormal m_animator;
    TargetManager m_targetMgr;

    RandomPlowlingMove m_randomPlowling;
    ThrongManager m_throngMgr;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_animator = GetComponent<AnimatorCtrl_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();

        m_randomPlowling = GetComponent<RandomPlowlingMove>();
        m_throngMgr = GetComponent<ThrongManager>();
    }

    //インターフェースの実装-------------------------------------------------

    void I_Chase.ChangeState(){
        m_stator.GetTransitionMember().findTrigger.Fire();
        //var member = m_stator.GetTransitionMember();
        //member.chaseTrigger.Fire();
    }

    void I_Chase.TargetLost()
    {
        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }

    void I_Listen.Listen(FoundObject foundObject) {
        //ターゲットの切替
        m_targetMgr.SetNowTarget(GetType() ,foundObject);

        var member = m_stator.GetTransitionMember();
        member.findTrigger.Fire();
    }

    void I_BindedActiveArea.Bind(BindActivateArea bind)
    {
        Debug.Log("Bind");

        m_randomPlowling.SetCenterObject(bind.GetAreaCenterObject());
        m_randomPlowling.SetRandomPositionRadius(bind.GetBindRange());
        //m_throngMgr.enabled = false;  //ThrongMgrをfalseにするか検討中

        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();

        //ターゲットがBindオブジェクトと同じならnullにする。
        //var found = bind.GetComponent<FoundObject>();
        //if (found)
        //{
        //    m_targetMgr.SetNowTarget(GetType(), found);

        //    var target = m_targetMgr.GetNowTarget();
        //    if(found == target)
        //    {
        //        m_targetMgr.SetNowTarget(GetType(), null);
        //        m_throngMgr.enabled = true;
        //    }
        //}
    }

    void I_BindedActiveArea.BindRelease(BindActivateArea bind)
    {
        Debug.Log("BindReset");

        //m_targetMgr.SetNowTarget(GetType(), null);
        m_randomPlowling.ResetCenterObject();
    }

    void I_Smell.SmellFind(FoundObject foundObject)
    {
        Debug.Log("におうぞ");
        m_targetMgr.SetNowTarget(GetType(), foundObject);
        m_stator.GetTransitionMember().findTrigger.Fire();
    }

    public void Eat()
    {
        Debug.Log("食べる");

        m_stator.GetTransitionMember().eatTrigger.Fire();
    }
}
