using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Manager;

/// <summary>
/// クリアリザルトUI
/// </summary>
public class ClearResult : MonoBehaviour
{
    /// <summary>
    /// ランク表示画像
    /// </summary>
    [SerializeField]
    private RankImageViewer m_rankImageViewer;

    [SerializeField]
    private MissionUI m_missionUI1;
    [SerializeField]
    private MissionUI m_missionUI2;
    [SerializeField]
    private MissionUI m_missionUI3;

    [SerializeField]
    private NumberImage m_scoreImage;

    [SerializeField]
    private Button m_nextStageButton;

    [SerializeField]
    private Button m_exitStageButton;

    [SerializeField]
    private PopUpUI m_popUI;

    private void Start()
    {
    }

    public void SetMissionUI1(bool isAchieve, string text)
    {
        m_missionUI1.SetMissionStatus(isAchieve, text);
    }
    public void SetMissionUI2(bool isAchieve, string text)
    {
        m_missionUI2.SetMissionStatus(isAchieve, text);
    }
    public void SetMissionUI3(bool isAchieve, string text)
    {
        m_missionUI3.SetMissionStatus(isAchieve, text);
    }

    public void SetScore(int score)
    {
        m_scoreImage.value = score;
    }

    public void SetRank(RankImageViewer.Rank rank)
    {
        m_rankImageViewer.SetRank(rank);
    }

    public void UpdateFirstSelect()
    {
        if (!GameStageManager.Instance.CanIncrement())
        {
            m_popUI.firstSelectObject = m_exitStageButton.gameObject;

            m_nextStageButton.gameObject.SetActive(false);
        }
    }
}
