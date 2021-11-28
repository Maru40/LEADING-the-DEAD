using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class AttackManager_ZombieNormal : AttackNodeManagerBase
{
    public enum AttackType
    {
        Normal,
    }

    [System.Serializable]
    public struct PreliminaryParametor
    {
        [Header("予備動作のランダム時間範囲")]
        public RandomRange timeRandomRange;
        [Header("予備動作の移動スピード")]
        public float moveSpeed;

        public PreliminaryParametor(RandomRange timeRandomRange, float moveSpeed)
        {
            this.timeRandomRange = timeRandomRange;
            this.moveSpeed = moveSpeed;
        }
    }

    [Header("予備動作のパラメータ") ,SerializeField]
    PreliminaryParametor m_preliminaryParam = new PreliminaryParametor(new RandomRange(1.0f,1.0f), 1.0f);

    [SerializeField]
    AudioManager m_audioManager = null;

    Stator_ZombieNormal m_stator;
    TargetManager m_targetMgr;
    AnimatorManager_ZombieNormal m_animatorManager;
    EyeSearchRange m_eye;

    GameTimer m_gameTimer = new GameTimer();

    void Awake()
    {
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_targetMgr = GetComponent<TargetManager>();
        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
        m_eye = GetComponent<EyeSearchRange>();
    }

    /// <summary>
    /// 攻撃を開始する距離かどうか
    /// </summary>
    /// <returns>開始するならtrue</returns>
    override public bool IsAttackStartRange()
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
        m_audioManager?.PlayOneShot();

        m_stator.GetTransitionMember().attackTrigger.Fire();

        m_animatorManager.CrossFadePreliminaryNormalAttackAniamtion();  //予備動作に変更

        var time = m_preliminaryParam.timeRandomRange.RandomValue;
        m_gameTimer.ResetTimer(time, () => m_animatorManager.CrossFadeNormalAttackAnimation());

        //m_animatorManager.CrossFadeNormalAttackAnimation();
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
