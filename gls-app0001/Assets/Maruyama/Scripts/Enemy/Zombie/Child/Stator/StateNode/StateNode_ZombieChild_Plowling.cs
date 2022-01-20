using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode_ZombieChild_Plowling : EnemyStateNodeBase<EnemyBase>
{
    private FoundObject m_eyeTarget = null;

    private TargetManager m_targetManager;
    private EyeSearchRange m_eye;

    public StateNode_ZombieChild_Plowling(EnemyBase owner)
        :base(owner)
    {
        m_targetManager = owner.GetComponent<TargetManager>();
        m_eye = owner.GetComponent<EyeSearchRange>();
    }

    protected override void ReserveChangeComponents()
    {

    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        TargetCheck();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void TargetCheck()
    {
        //ターゲットが視界に入ったら切り替える。
        if (m_eyeTarget == null) {
            SearchEyeTarget();
        }

        if (m_eye.IsInEyeRange(m_eyeTarget.gameObject)) { //視界の中にいたら
            m_targetManager.SetNowTarget(GetType(), m_eyeTarget);  //ターゲットの変更
        }
    }

    private void SearchEyeTarget()
    {
        var target = GameObject.Find("Player");
        var foundObject = target.GetComponent<FoundObject>();

        m_eyeTarget = foundObject;
    }
}
