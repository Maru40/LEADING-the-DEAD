using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MENU_PATH + "/BarricaeeBreakerCountMissionData")]
public class BarricadeBreakerCountMissionData : MissionData
{
    [SerializeField]
    private int m_breakerCountBoader = 0;

    public override bool IsMissionClear(PlayerStatusManager playerStatusManager, AllEnemyGeneratorManager enemyGenerator)
    {
        return false;
    }

    public override string GetExplanationText()
    {
        return regexText.Replace(VALUE_REPLACE_TEXT, m_breakerCountBoader.ToString());
    }
}