using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_RandomPlowling : EnemyStateNodeBase<EnemyBase>
{
    FoundObject m_target = null;

    EyeSearchRange m_eyeRange;

    public EnState_RandomPlowling(EnemyBase owner)
        :base(owner)
    { }

    public override void OnStart()
    {
        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<RandomPlowlingMove>(), true, false);

        ChangeComps(EnableChangeType.Start);

        //その他コンポーネントの取得
        m_eyeRange = owner.GetComponent<EyeSearchRange>();

        //ターゲットの取得
        m_target = owner.GetComponent<TargetMgr>().GetNowTarget();
        if(m_target == null) {
            SearchTarget();
        }
    }

    public override void OnUpdate()
    {
        Debug.Log("RandomPlowling");

        if(m_target == null) {
            SearchTarget();
            return;
        }

        //視界に敵が入ったらステートを切り替える。
        if (m_eyeRange)
        {
            if (m_eyeRange.IsInEyeRange(m_target.gameObject))
            {
                var owner = GetOwner();

                var chase = owner.GetComponent<I_Chase>();
                if (chase != null){
                    chase.ChangeState();
                }
            }
        }
        else
        {
            Debug.Log("EnState_RomdomPlowling :: " + " Update :" + "EyeSearchRangeコンポーネントが存在しません ");
        }
    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }

    private void SearchTarget()
    {
        var owner = GetOwner();

        var target = GameObject.Find("Player");
        var foundObject = target.GetComponent<FoundObject>();
        var targetMgr = owner.GetComponent<TargetMgr>();
        targetMgr?.SetNowTarget(GetType(), foundObject);

        m_target = foundObject;
    }
}
