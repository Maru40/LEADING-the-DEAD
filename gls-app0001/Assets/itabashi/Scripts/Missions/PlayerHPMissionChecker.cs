using Player;
using UnityEngine;

public class PlayerHPMissionChecker : MissionCheckerBase<Player.PlayerStatusManager>
{
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_hpRateBoader;

    public override string GetDisctiptionText()
    {
        float hpParsent = m_hpRateBoader * 100.0f;

        return m_descriptionFront + hpParsent.ToString() + m_descriptionEnd;
    }

    public override bool IsMissionClear(PlayerStatusManager playerStatusManager)
    {
        return playerStatusManager.hp / playerStatusManager.maxHp >= m_hpRateBoader;
    }
}
