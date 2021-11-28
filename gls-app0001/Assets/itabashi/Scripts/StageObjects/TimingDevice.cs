using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimingDevice : MonoBehaviour
{
    /// <summary>
    /// イベント発火までの時間
    /// </summary>
    [SerializeField]
    private float[] m_fireTimes = { 1.0f };

    /// <summary>
    /// イベント発火までのカウント時間
    /// </summary>
    private float m_nowCountTime = 0.0f;

    /// <summary>
    /// タイマーが現在動いているか
    /// </summary>
    private bool m_isTimerActive = false;

    /// <summary>
    /// シーン開始時にタイマーを開始するか
    /// </summary>
    [SerializeField]
    private bool m_startOnAwake = false;

    [SerializeField]
    private bool m_isLoop = false;

    private int m_index = 0;

    /// <summary>
    /// 時間になると発火されるイベント
    /// </summary>
    [SerializeField]
    private UnityEvent m_timerEvent = new UnityEvent();

    /// <summary>
    /// タイマーを開始する(初めから)
    /// </summary>
    public void TimerStart()
    {
        m_isTimerActive = true;
        m_nowCountTime = 0.0f;
    }

    /// <summary>
    /// タイマーを停止する(終了)
    /// </summary>
    public void TimerStop()
    {
        m_isTimerActive = false;
        m_nowCountTime = 0.0f;
    }

    /// <summary>
    /// タイマーを一時停止する
    /// </summary>
    public void TimerPause()
    {
        m_isTimerActive = false;
    }

    /// <summary>
    /// タイマーを開始する(途中から)
    /// </summary>
    public void TimerUnPause()
    {
        m_isTimerActive = true;
    }

    private void Awake()
    {
        if(m_startOnAwake)
        {
            TimerStart();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isTimerActive || m_fireTimes.Length == 0)
        {
            return;
        }

        m_nowCountTime += Time.deltaTime;

        if (m_nowCountTime < m_fireTimes[m_index])
        {
            return;
        }

        Debug.Log($"{gameObject.name}のTimerEventが発火されました");

        m_timerEvent.Invoke();

        if(m_isLoop)
        {
            m_nowCountTime -= m_fireTimes[m_index];

            ++m_index;

            if (m_index >= m_fireTimes.Length)
            {
                m_index = 0;
            }

            return;
        }

        TimerStop();
    }
}
