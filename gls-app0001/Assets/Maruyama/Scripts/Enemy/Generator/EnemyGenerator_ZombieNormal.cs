using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator_ZombieNormal : EnemyGenerator
{
    [SerializeField]
    ChaseTargetParametor m_chaseParametor = new ChaseTargetParametor(0.75f, 3.0f, 3.0f, 10.0f, 3.0f);

    [SerializeField]
    RandomPlowlingMove.Parametor m_randomPlowlingParametor = new RandomPlowlingMove.Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);

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

        var randomPlowling = obj.GetComponent<RandomPlowlingMove>();
        if (randomPlowling)
        {
            randomPlowling.SetParametor(m_randomPlowlingParametor);
        }
    }
}
