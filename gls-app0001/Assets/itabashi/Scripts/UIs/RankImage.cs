using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

/// <summary>
/// ランク表示UI
/// </summary>
public class RankImage : MonoBehaviour
{
    /// <summary>
    /// ランク列挙体
    /// </summary>
    public enum Rank
    {
        S,A,B,C
    }

    /// <summary>
    /// Sランク時に表示される画像
    /// </summary>
    [SerializeField]
    private Sprite m_sRankSprite;

    /// <summary>
    /// Aランク時に表示される画像
    /// </summary>
    [SerializeField]
    private Sprite m_aRankSprite;

    /// <summary>
    /// Bランク時に表示される画像
    /// </summary>
    [SerializeField]
    private Sprite m_bRankSprite;

    /// <summary>
    /// Cランク時に表示される画像
    /// </summary>
    [SerializeField]
    private Sprite m_cRankSprite;

    [SerializeField]
    private Rank m_rank = Rank.S;

    private Image m_image;

    private void OnValidate()
    {
        m_image = GetComponent<Image>();

        SetRank(m_rank);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRank(Rank rank)
    {
        m_image.sprite = rank switch
        {
            Rank.S => m_sRankSprite,
            Rank.A => m_aRankSprite,
            Rank.B => m_bRankSprite,
            Rank.C => m_cRankSprite,
            _ => null
        };
    }
}
