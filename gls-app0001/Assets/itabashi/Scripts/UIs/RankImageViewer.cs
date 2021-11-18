using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ランク表示UI表示用
/// </summary>
public class RankImageViewer : MonoBehaviour
{
    /// <summary>
    /// ランク列挙体
    /// </summary>
    public enum Rank
    {
        S,A,B,C
    }

    /// <summary>
    /// Sランク時に表示されるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject m_sRankObject;

    /// <summary>
    /// Aランク時に表示されるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject m_aRankObject;

    /// <summary>
    /// Bランク時に表示されるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject m_bRankObject;

    /// <summary>
    /// Cランク時に表示されるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject m_cRankObject;

    [SerializeField]
    private Rank m_rank = Rank.S;

    private GameObject m_nowActiveRankObject = null;

    private void OnValidate()
    {
        SetRank(m_rank);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRank(Rank rank)
    {
        if(m_nowActiveRankObject)
        {
            m_nowActiveRankObject.SetActive(false);
        }

        m_nowActiveRankObject = rank switch
        {
            Rank.S => m_sRankObject,
            Rank.A => m_aRankObject,
            Rank.B => m_bRankObject,
            Rank.C => m_cRankObject,
            _ => null
        };

        if(m_nowActiveRankObject)
        {
            m_nowActiveRankObject.SetActive(true);
        }
    }
}
