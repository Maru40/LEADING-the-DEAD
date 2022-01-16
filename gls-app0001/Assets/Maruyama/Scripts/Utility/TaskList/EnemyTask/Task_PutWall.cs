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

    public Task_PutWall(EnemyBase owner, Parametor parametor)
        :base(owner)
    {
        m_param = parametor;

        m_targetManager = owner.GetComponent<TargetManager>();
        m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
    }

    public override void OnEnter()
    {
        m_velocityManager.ResetAll();
    }

    public override bool OnUpdate()
    {
        if (!m_targetManager.HasTarget()) {
            return true;
        }

        var type = m_targetManager.GetNowTargetType();
        if(type != FoundObject.FoundType.Smell) //Smellでなかったら
        {
            return true;
        }

        return false;
    }

    public override void OnExit()
    {
        Debug.Log("PutWall終了");
    }
}
