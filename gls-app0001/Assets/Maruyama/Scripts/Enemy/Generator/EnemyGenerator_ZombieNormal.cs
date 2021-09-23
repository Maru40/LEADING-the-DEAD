using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator_ZombieNormal : EnemyGenerator
{
    [SerializeField]
    ChaseTargetParametor m_chaseParametor = new ChaseTargetParametor(0.75f, 3.0f, 3.0f, 10.0f, 3.0f);

    protected override void Start()
    {
        base.Start();
    }

    protected override void CreateObjectAdjust(GameObject obj)
    {
        var chase = obj.GetComponent<ChaseTarget>();
        if (chase)
        {
            chase.SetPamametor(m_chaseParametor);
        }
    }
}
