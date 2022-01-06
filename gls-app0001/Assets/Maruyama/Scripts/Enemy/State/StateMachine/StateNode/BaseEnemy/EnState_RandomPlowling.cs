using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_RandomPlowling : EnemyStateNodeBase<EnemyBase>
{
    private FoundObject m_target = null;

    private EyeSearchRange m_eyeRange;
    private TargetManager m_targetMgr = null;

    public EnState_RandomPlowling(EnemyBase owner)
        :base(owner)
    {
        m_eyeRange = owner.GetComponent<EyeSearchRange>();
        m_targetMgr = owner.GetComponent<TargetManager>();
    }

    protected override void ReserveChangeComponents()
    {
        var owner = GetOwner();

        AddChangeComp(owner.GetComponent<RandomPlowlingMove>(), true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        var owner = GetOwner();
        var randomPlowling = owner.GetComponent<RandomPlowlingMove>();

        //その他コンポーネントの取得
        m_eyeRange = owner.GetComponent<EyeSearchRange>();

        //ターゲットの取得
        SearchTarget();

        //集団範囲の設定
        var throngMgr = owner.GetComponent<ThrongManager>();
        if(randomPlowling && throngMgr)
        {
            float range = randomPlowling.GetInThrongRange();
            throngMgr.SetInThrongRange(range);
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
                    m_targetMgr.SetNowTarget(GetType() ,m_target);
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
        base.OnExit();
    }

    private void SearchTarget()
    {
        var owner = GetOwner();

        var target = GameObject.Find("Player");
        var foundObject = target.GetComponent<FoundObject>();
        m_targetMgr?.SetNowTarget(GetType(), null);
        //targetMgr?.SetNowTarget(GetType(), foundObject);

        m_target = foundObject;
    }
}
