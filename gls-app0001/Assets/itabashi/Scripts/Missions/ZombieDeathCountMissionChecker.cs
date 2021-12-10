using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeathCountMissionChecker : MissionCheckerBase<AllEnemyGeneratorManager>
{
    [SerializeField, Min(0)]
    private int m_zombieDeathCountBoarder;

    public override string GetDisctiptionText()
    {
        return m_descriptionFront + m_zombieDeathCountBoarder.ToString() + m_descriptionEnd;
    }

    public override bool IsMissionClear(AllEnemyGeneratorManager allEnemyGeneratorManager)
    {
        return allEnemyGeneratorManager.GetAllDeashCount() <= m_zombieDeathCountBoarder;
    }
}
