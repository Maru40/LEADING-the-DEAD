using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScoreCalculator : MonoBehaviour
{

    [SerializeField]
    private ClearResult m_clearResult;

    [SerializeField]
    private StageTimer m_stageTimer;

    [SerializeField]
    private PopUpUI m_popUpUI;

    [SerializeField]
    private Player.PlayerStatusManager m_playerStatusManager;

    [SerializeField]
    private EnemyGenerator m_enemyGenerator;

    [SerializeField]
    private MissionData m_mission1;
    [SerializeField]
    private MissionData m_mission2;
    [SerializeField]
    private MissionData m_mission3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearResultDraw()
    {
        float score = 0.0f;

        //score += Mathf.Max(m_scoringBaseTime - m_stageTimer.timeSeconds, 0.0f) * m_timeSecondToScoreScale;

        //score += m_playerStatusManager.hp / m_playerStatusManager.maxHp * m_hpScoreBase;

        m_clearResult.SetScore((int)score);


        int missionClearNum = 0;

        bool isAchieve = m_mission1.IsMissionClear(m_playerStatusManager, m_enemyGenerator);
        m_clearResult.SetMissionUI1(isAchieve, m_mission1.GetExplanationText());
        if (isAchieve) { ++missionClearNum; }

        isAchieve = m_mission2.IsMissionClear(m_playerStatusManager, m_enemyGenerator);
        m_clearResult.SetMissionUI2(isAchieve, m_mission2.GetExplanationText());
        if (isAchieve) { ++missionClearNum; }

        isAchieve = m_mission3.IsMissionClear(m_playerStatusManager, m_enemyGenerator);
        m_clearResult.SetMissionUI3(isAchieve, m_mission3.GetExplanationText());
        if (isAchieve) { ++missionClearNum; }

         m_clearResult.SetRank(GetScoreRank(missionClearNum));
      
        m_clearResult.UpdateFirstSelect();

        m_popUpUI.PopUp();

    }

    private RankImageViewer.Rank GetScoreRank(int missionClearNum)
    {
        if(missionClearNum >= 3)
        {
            return RankImageViewer.Rank.S;
        }

        if(missionClearNum >= 2)
        {
            return RankImageViewer.Rank.A;
        }

        if(missionClearNum >= 1)
        {
            return RankImageViewer.Rank.B;
        }

        return RankImageViewer.Rank.C;
    }
}
