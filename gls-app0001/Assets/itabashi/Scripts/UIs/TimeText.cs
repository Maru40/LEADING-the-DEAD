using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeText : MonoBehaviour
{
    private Text m_text;

    [SerializeField]
    private int m_time;

    private void OnValidate()
    {
        if (!m_text)
        {
            m_text = GetComponent<Text>();
        }

        UpdateTimeText();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTime(int time)
    {
        m_time = time;

        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        m_text.text = m_time.ToString();
    }
}
