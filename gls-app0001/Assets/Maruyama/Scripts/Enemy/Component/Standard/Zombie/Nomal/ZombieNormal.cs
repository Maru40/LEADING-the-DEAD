using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyBase, I_Chase, I_Listen, I_BindedActiveArea
{
    //test用に表示したり、消したりする用。
    [SerializeField]
    GameObject m_tempPrehab = null;
    GameObject m_tempObject = null;

    //コンポーネント系

    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;

    RandomPlowlingMove m_randomPlowling;
    ThrongManager m_throngMgr;

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();

        m_randomPlowling = GetComponent<RandomPlowlingMove>();
        m_throngMgr = GetComponent<ThrongManager>();
    }

    void Update()
    {
        
    }



    //インターフェースの実装-------------------------------------------------

    void I_Chase.ChangeState(){
        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }

    void I_Listen.Listen(FoundObject foundObject) {
        //ターゲットの切替
        m_targetMgr.SetNowTarget(GetType() ,foundObject);

        var member = m_stator.GetTransitionMember();
        member.chaseTrigger.Fire();
    }

    void I_BindedActiveArea.Bind(BindActivateArea bind)
    {
        m_randomPlowling.SetCenterObject(bind.gameObject);
        m_randomPlowling.SetRandomPositionRadius(bind.GetBindRange());
        //m_throngMgr.enabled = false;  //ThrongMgrをfalseにするか検討中

        m_stator.GetTransitionMember().rondomPlowlingTrigger.Fire();

        //ターゲットがBindオブジェクトと同じならnullにする。
        var found = bind.GetComponent<FoundObject>();
        if (found)
        {
            m_targetMgr.SetNowTarget(GetType(), found);

            var target = m_targetMgr.GetNowTarget();
            if(found == target)
            {
                m_targetMgr.SetNowTarget(GetType(), null);
                m_throngMgr.enabled = true;
            }
        }
    }

    void I_BindedActiveArea.BindRelease(BindActivateArea bind)
    {
        m_randomPlowling.ResetCenterObject();
    }
}
