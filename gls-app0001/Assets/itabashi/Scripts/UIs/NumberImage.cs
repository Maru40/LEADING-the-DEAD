using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NumberImage : MonoBehaviour
{
    [System.Serializable]
    class NumberSprites
    {
        public Sprite num0;
        public Sprite num1;
        public Sprite num2;
        public Sprite num3;
        public Sprite num4;
        public Sprite num5;
        public Sprite num6;
        public Sprite num7;
        public Sprite num8;
        public Sprite num9;

        public Sprite this[int i]
        {
            get
            {
                return i switch
                {
                    0 => num0,
                    1 => num1,
                    2 => num2,
                    3 => num3,
                    4 => num4,
                    5 => num5,
                    6 => num6,
                    7 => num7,
                    8 => num8,
                    9 => num9,
                    _ => null
                };
            }
        }
    }

    enum Alignment
    {
        Left,
        Center,
        Right
    }

    [SerializeField]
    private Image m_numberPrefab;

    [SerializeField]
    private NumberSprites m_numberSprites;

    [SerializeField,Min(0)]
    private int m_maxDigits = 5;

    [SerializeField, HideInInspector]
    private List<Image> m_numImages = new List<Image>();

    [SerializeField, Min(0)]
    private int m_value = 0;

    [SerializeField, Min(0.0f)]
    private float m_digitDistance = 0.0f;

    [SerializeField]
    private bool m_isZeroPadding = false;

    [SerializeField]
    private Alignment m_alignment = Alignment.Right;

    private RectTransform m_rectTransform;

    private void OnValidate()
    {
        DigitsChanged();

        NumberChanged();

        PositionChanged();
    }

    private void DigitsChanged()
    {
        m_numImages = m_numImages.Where(image => image != null).ToList();

        int oldDigits = m_numImages.Count;

        if(m_maxDigits == oldDigits)
        {
            return;
        }

        if(m_maxDigits > oldDigits)
        {
            for (int i = 0; i < m_maxDigits - oldDigits; ++i)
            {
                Image image = Instantiate(m_numberPrefab, transform);
                image.gameObject.name = $"Digit{m_numImages.Count}";
                m_numImages.Add(image);
            }

            return;
        }

        for (int i = 0; i < oldDigits - m_maxDigits; ++i)
        {
            Image image = m_numImages.Last();
            m_numImages.Remove(image);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(image.gameObject);
#else
            Destroy(image.gameObject);
#endif
        }
    }

    void NumberChanged()
    {
        bool isDigitOver = Digit(m_value) > m_maxDigits;

        int value = m_value;

        for (int i = 0; i < m_maxDigits; ++i)
        {
            int numIndex = value % 10;
            value /= 10;

            if(isDigitOver)
            {
                m_numImages[i].sprite = m_numberSprites.num9;
                continue;
            }


            if(numIndex == 0)
            {
                m_numImages[i].sprite = m_isZeroPadding ? m_numberSprites.num0 : null;
                continue;
            }

            m_numImages[i].sprite = m_numberSprites[numIndex];
        }
    }

    private void PositionChanged()
    {
        if(!m_rectTransform)
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        switch(m_alignment)
        {
            case Alignment.Left:

                var leftPosX = -m_rectTransform.rect.width / 2;

                foreach(var image in m_numImages.Reverse<Image>())
                {
                    var rect = image.rectTransform.rect;

                    float x = leftPosX + rect.width / 2;

                    image.rectTransform.anchoredPosition = new Vector2(x, 0);

                    leftPosX = image.rectTransform.anchoredPosition.x + rect.width / 2 + m_digitDistance;
                }

                break;

            case Alignment.Center:

                float allWidth = 0.0f;

                foreach(var image in m_numImages)
                {
                    allWidth += image.rectTransform.rect.width;
                }

                var allWidthRightPosX = allWidth / 2;

                foreach (var image in m_numImages)
                {
                    var rect = image.rectTransform.rect;

                    float x = allWidthRightPosX - rect.width / 2;

                    image.rectTransform.anchoredPosition = new Vector2(x, 0);

                    allWidthRightPosX = image.rectTransform.anchoredPosition.x - rect.width / 2 - m_digitDistance;
                }

                break;

            case Alignment.Right:

                var rightPosX = m_rectTransform.rect.width / 2;

                foreach (var image in m_numImages)
                {
                    var rect = image.rectTransform.rect;

                    float x = rightPosX - rect.width / 2;

                    image.rectTransform.anchoredPosition = new Vector2(x, 0);

                    rightPosX = image.rectTransform.anchoredPosition.x - rect.width / 2 - m_digitDistance;
                }

                break;
        }
    }

    int Digit(int num)
    {
        // Mathf.Log10(0)はNegativeInfinityを返すため、別途処理する。
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);
    }

    private void Awake()
    {
        PositionChanged();
    }

    public void SetNumber(int number)
    {
        m_value = Mathf.Max(0, number);

        NumberChanged();
    }
}
