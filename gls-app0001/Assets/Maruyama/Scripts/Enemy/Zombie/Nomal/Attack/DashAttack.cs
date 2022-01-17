using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class DashAttack : AttackNodeBase
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("行動確率")]
        public float probability;
        [Header("行動始める距離")]
        public float startRange;
        [Header("確率計算インターバル")]
        public float probabilityInterbalTime;

        public Parametor(float probability, float startRange, float probabilityIntervalTime)
        {
            this.probability = probability;
            this.startRange = startRange;
            this.probabilityInterbalTime = probabilityIntervalTime;
        }
    }

    [SerializeField]
    private Parametor m_param = new Parametor();

    private TargetManager m_targetManager;
    private EyeSearchRange m_eye;
    private AttackNodeManagerBase m_attackManager;
    private Stator_ZombieNormal m_stator;
    private BlackBoard_ZombieNormal m_backBoard;

    private GameTimer m_timer = new GameTimer();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_eye = GetComponent<EyeSearchRange>();
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_stator = GetComponent<Stator_ZombieNormal>();
        m_backBoard = GetComponent<BlackBoard_ZombieNormal>();
    }

    private void Start()
    {
        //DefineTask();
        m_timer.ResetTimer(m_param.probabilityInterbalTime);
    }

    private void Update()
    {
        return;

        if (m_stator.GetNowStateType() == ZombieNormalState.Attack) {
            return;
        }

        if (m_timer.UpdateTimer()) //一定間隔で攻撃を始めるか決める
        {
            m_timer.ResetTimer(m_param.probabilityInterbalTime);
            ProbabilityAttack();
        }
    }

    public override bool IsAttackStartRange()
    {
        float range = GetBaseParam().startRange;
        var position = m_targetManager.GetNowTargetPosition();
        if (position != null) {
            return m_eye.IsInEyeRange((Vector3)position, range);
        }

        return false;
    }

    public override void AttackStart()
    {
        //SelectTask();
        m_backBoard.Struct.attackParam.startType = StateNode_ZombieNormal_Attack.StateType.Dash;
        m_stator.GetTransitionMember().attackTrigger.Fire();
    }

    public override void EndAnimationEvent()
    {
        m_timer.ResetTimer(m_param.probabilityInterbalTime);
        m_attackManager.EndAnimationEvent();
    }

    /// <summary>
    /// 確率攻撃
    /// </summary>
    private void ProbabilityAttack()
    {
        if (IsAttackStart())
        {
            AttackStart();
        }
    }

    private bool IsAttackStart()
    {
        if(m_stator.GetNowStateType() == ZombieNormalState.Attack) {
            return false;
        }

        if (!m_targetManager.HasTarget()) {
            return false;
        }

        if(m_targetManager.GetNowTargetType() != FoundObject.FoundType.Player) { //Playerでなかったら攻撃をしない。
            return false;
        }

        bool isProbability = MyRandom.RandomProbability(m_param.probability);

        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //確率内で、近くにいるとき
        if(isProbability && m_param.startRange > toTargetVec.magnitude)
        {
            return true;
        }

        return false;
    }

    //アクセッサ----------------------------------------------------------------------------------------

    public Parametor parametor
    {
        get => m_param;
        set => m_param = value;
    }

}
