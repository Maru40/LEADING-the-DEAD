﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyBase, I_Chase, I_Listen, I_BindedActiveArea, I_Smell, I_Eat, I_BindPlowlingArea
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
    StatusManager_ZombieNormal m_statusManager;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_animator = GetComponent<AnimatorCtrl_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();

        m_randomPlowling = GetComponent<RandomPlowlingMove>();
        m_throngMgr = GetComponent<ThrongManager>();
        m_statusManager = GetComponent<StatusManager_ZombieNormal>();
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

        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();
    }

    void I_BindedActiveArea.BindRelease(BindActivateArea bind)
    {
        Debug.Log("BindReset");

        //m_targetMgr.SetNowTarget(GetType(), null);
        m_randomPlowling.ResetCenterObject();
    }

    void I_Smell.SmellFind(FoundObject foundObject)
    {
        if(m_targetMgr.SetNowTarget(GetType(), foundObject))
        {
            Debug.Log("におうぞ");
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
        //Debug.Log("■バインドされた");

        m_randomPlowling.SetCenterObject(manager.CenterObject);
        m_randomPlowling.SetRandomPositionRadius(manager.BindRange);
    }

    public void OutBind(BindPlowlingAreaManager manager)
    {
        //Debug.Log("■バインド解除");
        m_randomPlowling.ResetCenterObject();
    }
}
