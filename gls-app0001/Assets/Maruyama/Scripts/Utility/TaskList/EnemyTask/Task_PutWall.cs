using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_PutWall : TaskNodeBase<EnemyBase>
{
    public struct Parametor
    {
        //public float time;
    }

    private Parametor m_param = new Parametor();

    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private ObstacleEvasion m_evasion;

    public Task_PutWall(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
        m_evasion = owner.GetComponent<ObstacleEvasion>();
    }

    public override void OnEnter()
    {
        m_velocityManager.ResetAll();

        m_evasion.enabled = false;
    }

    public override bool OnUpdate()
    {
        m_velocityManager.ResetAll();

        if (!m_targetManager.HasTarget()) {
            Debug.Log("ターゲットがなくなった");
            return true;
        }

        var type = m_targetManager.GetNowTargetType();
        if(type != FoundObject.FoundType.Smell) //Smellでなかったら
        {
            Debug.Log("匂いで亡くなった");
            return true;
        }

        return false;
    }

    public override void OnExit()
    {
        m_evasion.enabled = true;

        Debug.Log("PutWall終了");
    }
}
