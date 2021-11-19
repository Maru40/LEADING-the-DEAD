using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class MissionStamp : MonoBehaviour
{
    /// <summary>
    /// 達成時に表示される画像
    /// </summary>
    [SerializeField]
    private Sprite m_achievementSprite;

    /// <summary>
    /// 未達成時に表示される画像
    /// </summary>
    [SerializeField]
    private Sprite m_unachievedSprite;

    [SerializeField]
    private bool m_isAchieve = false;

    [SerializeField]
    private Image m_image;

    private void Reset()
    {
        SetAchieveSprite(m_isAchieve);
    }

    private void OnValidate()
    {
        SetAchieveSprite(m_isAchieve);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetAchieveSprite(bool isAchieve)
    {
        m_image.sprite = isAchieve ? m_achievementSprite : m_unachievedSprite;
    }

    public void SetIsAchieve(bool isAchieve)
    {
        m_isAchieve = isAchieve;

        SetAchieveSprite(isAchieve);
    }
}
