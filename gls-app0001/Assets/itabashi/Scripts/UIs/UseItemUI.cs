using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 仕様アイテムの種類
/// </summary>
enum UseItemType
{
    /// <summary>
    /// アイテムは一つのみ
    /// </summary>
    SingleItem,
    /// <summary>
    /// アイテムは複数個
    /// </summary>
    CountItem
}

/// <summary>
/// 仕様アイテム用UI
/// </summary>
public class UseItemUI : MonoBehaviour
{
    /// <summary>
    /// アイテムが有効かどうか
    /// </summary>
    [SerializeField]
    private bool m_isValidity = true;

    /// <summary>
    /// 無効時のアイテム画像のAlpha
    /// </summary>
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float m_disabledAlpha = 0.25f;

    [SerializeField]
    private Image[] m_useItemImages = { };

    [SerializeField]
    private UseItemType m_useItemType = UseItemType.SingleItem;

    [SerializeField]
    private int m_count;

    [SerializeField]
    private NumberImage m_countImage;


    /// <summary>
    /// アイテムが有効かどうか
    /// </summary>
    public bool isValidity
    {
        set
        {
            m_isValidity = value;

            UpdateItemImageAlpha();
        }
        
        get { return m_isValidity; }
    }

    public float disabledAlpha
    {
        set
        {
            m_disabledAlpha = value;

            UpdateItemImageAlpha();
        }

        get { return m_disabledAlpha; }
    }

    public int count
    {
        set
        {
            m_count = Mathf.Max(value, 0);

            UpdateCountText();
        }

        get { return m_count; }
    }

    private void OnValidate()
    {
        count = m_count;
        UpdateItemImageAlpha();
        UpdateCountText();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateItemImageAlpha()
    {
        if (m_useItemImages.Length == 0)
        {
            return;
        }

        foreach (var image in m_useItemImages)
        {
            if(!image)
            {
                continue;
            }

            var color = image.color;
            color.a = m_isValidity ? 1.0f : m_disabledAlpha;
            image.color = color;
        }
    }

    private void UpdateCountText()
    {
        if(!m_countImage)
        {
            return;
        }

        if (m_useItemType != UseItemType.CountItem)
        {
            m_countImage.gameObject.SetActive(false);

            return;
        }

        m_countImage.gameObject.SetActive(true);

        m_countImage.SetNumber(m_count);

        m_isValidity = m_count > 0;

        UpdateItemImageAlpha();
    }
}
