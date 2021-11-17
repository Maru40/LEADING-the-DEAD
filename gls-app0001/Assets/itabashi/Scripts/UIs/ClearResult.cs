using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// クリアリザルトUI
/// </summary>
public class ClearResult : MonoBehaviour
{
    /// <summary>
    /// ランク表示画像
    /// </summary>
    [SerializeField]
    private RankImage m_rankImage;

    [SerializeField]
    private MissionStar m_missionStar1;
    [SerializeField]
    private MissionStar m_missionStar2;
    [SerializeField]
    private MissionStar m_missionStar3;

    [SerializeField]
    private NumberImage m_scoreImage;

    [SerializeField]
    private Button m_nextStageButton;

    [SerializeField]
    private Button m_exitStageButton;

    public void SetMissionStar1(bool isAchieve)
    {
        m_missionStar1.SetIsAchieve(isAchieve);
    }
    public void SetMissionStar2(bool isAchieve)
    {
        m_missionStar2.SetIsAchieve(isAchieve);
    }
    public void SetMissionStar3(bool isAchieve)
    {
        m_missionStar3.SetIsAchieve(isAchieve);
    }

    public void SetScore(int score)
    {
        m_scoreImage.value = score;
    }

    public void SetRank(RankImage.Rank rank)
    {
        m_rankImage.SetRank(rank);
    }
    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(m_nextStageButton.gameObject);
    }
}
