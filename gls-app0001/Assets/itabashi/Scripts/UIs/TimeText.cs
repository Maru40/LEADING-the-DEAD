using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeText : MonoBehaviour
{
    enum TimeDisplay
    {
        HoursMinutesSeconds,
        MinutesSecounds,
        Seconds
    }

    private Text m_text;

    [SerializeField]
    private float m_time;

    [SerializeField]
    private TimeDisplay m_timeDisplay;

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

    public void SetTime(float time)
    {
        m_time = time;

        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        var span = System.TimeSpan.FromSeconds(m_time);
        m_text.text = m_timeDisplay switch
        {
            TimeDisplay.HoursMinutesSeconds => span.ToString(@"hh\:mm\:ss"),
            TimeDisplay.MinutesSecounds => span.ToString(@"mm\:ss"),
            TimeDisplay.Seconds => span.Seconds.ToString(),
            _ => ""
        };
    }
}
