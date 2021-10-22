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
    private float m_S_rankBorder = 1000;
    [SerializeField]
    private float m_A_rankBorder = 750;
    [SerializeField]
    private float m_B_rankBorder = 500;

    [SerializeField]
    private float m_timeSecondToScoreScale = 1.0f;

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

        score += m_stageTimer.timeSeconds * m_timeSecondToScoreScale;

        m_clearResult.SetTime(m_stageTimer.timeSeconds);

        m_clearResult.SetScore((int)score);

        m_clearResult.SetRank(GetScoreRank(score));


        m_clearResult.gameObject.SetActive(true);
    }

    private RankImage.Rank GetScoreRank(float score)
    {
        if(score >= m_S_rankBorder)
        {
            return RankImage.Rank.S;
        }

        if(score >= m_A_rankBorder)
        {
            return RankImage.Rank.A;
        }

        if(score >= m_B_rankBorder)
        {
            return RankImage.Rank.B;
        }

        return RankImage.Rank.C;
    }
}
