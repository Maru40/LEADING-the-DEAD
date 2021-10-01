using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private TimeText m_timeText;

    [SerializeField]
    private Text m_scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void SetTime(int time)
    {
        m_timeText.SetTime(time);
    }

    public void SetScore(int score)
    {
        m_rankImage.SetRank(RankImage.Rank.S);

        m_scoreText.text = score.ToString();
    }
}
