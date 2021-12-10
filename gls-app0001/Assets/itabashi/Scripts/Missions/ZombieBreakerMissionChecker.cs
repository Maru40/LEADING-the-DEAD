using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBreakerMissionChecker : MissionCheckerBase<AllEnemyGeneratorManager>
{
    [SerializeField, Min(0)]
    private int m_zombieBreakerCountBoader;

    public override string GetDisctiptionText()
    {
        return m_descriptionFront + m_zombieBreakerCountBoader.ToString() + m_descriptionEnd;
    }

    public override bool IsMissionClear(AllEnemyGeneratorManager allEnemyGeneratorManager)
    {
        //Debug.Log($"参加ゾンビ数　{allEnemyGeneratorManager.GetNumAllClearServices()}");
        //Debug.Break();
        return allEnemyGeneratorManager.GetNumAllClearServices() >= m_zombieBreakerCountBoader;
    }
}
