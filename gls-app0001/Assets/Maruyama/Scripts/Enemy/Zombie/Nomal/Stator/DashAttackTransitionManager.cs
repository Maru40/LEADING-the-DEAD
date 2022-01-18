using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

public class DashAttackTransitionManager : StateTransitionManagerBase<EnemyBase, DashAttackTransitionManager.Parametor>
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

    private GameTimer m_timer = new GameTimer();

    private TargetManager m_targetManager;

    public DashAttackTransitionManager(EnemyBase owner, Action transitionAction,Parametor parametor)
        :base(owner, transitionAction)
    {
        //m_param = parametor;
        m_timer.ResetTimer(parametor.probabilityInterbalTime);

        m_targetManager = owner.GetComponent<TargetManager>();
    }

    public override bool IsTransition(Parametor parametor)
    {
        m_timer.UpdateTimer();

        if (m_timer.UpdateTimer()) //一定間隔で攻撃を始めるか決める
        {
            m_timer.ResetTimer(parametor.probabilityInterbalTime);
            if (IsSuccesProbability(parametor)) //確率を獲得
            {
                TransitionAction?.Invoke();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 確率が成功した時
    /// </summary>
    /// <returns></returns>
    private bool IsSuccesProbability(Parametor parametor)
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

        if (m_targetManager.GetNowTargetType() != FoundObject.FoundType.Player)
        { //Playerでなかったら攻撃をしない。
            return false;
        }

        bool isProbability = MyRandom.RandomProbability(parametor.probability);

        var toTargetVec = (Vector3)m_targetManager.GetToNowTargetVector();
        //確率内で、近くにいるとき
        if (isProbability && parametor.startRange > toTargetVec.magnitude)
        {
            return true;
        }

        return false;
    }
}
