using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MENU_PATH + "/ZombieKillCountMissionData")]
public class ZombieKillCountMissionData : MissionData
{
    [SerializeField, Min(0)]
    private int m_killCountBoader = 100;

    public override bool IsMissionClear(PlayerStatusManager playerStatusManager, AllEnemyGeneratorManager enemyGenerator)
    {
        return false;
    }

    public override string GetExplanationText()
    {
        return regexText.Replace(VALUE_REPLACE_TEXT, m_killCountBoader.ToString());
    }
}
