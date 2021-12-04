using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MENU_PATH + "/RemainingHpRateMissionData")]
public class RemainingHpRateMissionData : MissionData
{
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_hpRateBoader;

    public override bool IsMissionClear(PlayerStatusManager playerStatusManager, EnemyGenerator enemyGenerator)
    {
        return playerStatusManager.hp / playerStatusManager.maxHp >= m_hpRateBoader;
    }

    public override string GetExplanationText()
    {
        float hpParsent = m_hpRateBoader * 100.0f;

        return regexText.Replace(VALUE_REPLACE_TEXT, hpParsent.ToString());
    }

}
