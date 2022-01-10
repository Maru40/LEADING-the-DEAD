using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class AttackManager_ZombieNormal : AttackNodeManagerBase
{
    public enum AttackType
    {
        Normal,
        WallAttack,
    }

    [Header("予備動作のパラメータ") ,SerializeField]
    private PreliminaryParametor m_preliminaryParam = new PreliminaryParametor(new RandomRange(1.0f,1.0f), 1.0f);

    [SerializeField]
    private AudioManager m_audioManager = null;

    private Stator_ZombieNormal m_stator;
    private TargetManager m_targetMgr;
    private AnimatorManager_ZombieNormal m_animatorManager;

    private GameTimer m_gameTimer = new GameTimer();

    private void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
    }

    /// <summary>
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        //FoundObject target = m_targetMgr.GetNowTarget();
        var position = m_targetMgr.GetNowTargetPosition();
        if (position != null)
        {
            //return m_eye.IsInEyeRange((Vector3)position, range);
            return Calculation.IsRange(gameObject, (Vector3)position, range);
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if(m_stator.GetNowStateType() == ZombieNormalState.Attack)  //ステートタイプが攻撃なら
        {
            m_gameTimer.UpdateTimer();
        }
    }

    public override void AttackStart()
    {
        if(m_stator.GetNowStateType() == ZombieNormalState.Attack) {
            return;
        }

        m_audioManager?.PlayOneShot();  //声を出す。

        m_stator.GetTransitionMember().attackTrigger.Fire();  //攻撃状態に遷移

        m_animatorManager.CrossFadePreliminaryNormalAttackAniamtion();  //予備動作に変更

        var time = m_preliminaryParam.timeRandomRange.RandomValue;
        m_gameTimer.ResetTimer(time, () => m_animatorManager.CrossFadeNormalAttackAnimation());
    }

    public override void EndAnimationEvent()
    {
        m_stator.GetTransitionMember().chaseTrigger.Fire();
    }

    //アクセッサ・プロパティ--------------------------------------------------------------------------
    
    public PreliminaryParametor PreliminaryParametorProperty
    {
        get => m_preliminaryParam;
        set => m_preliminaryParam = value;
    }
}
