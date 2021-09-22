using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimingDevice : MonoBehaviour
{
    /// <summary>
    /// �C�x���g���΂܂ł̎���
    /// </summary>
    [SerializeField]
    private float m_fireTime = 1.0f;

    /// <summary>
    /// �C�x���g���΂܂ł̃J�E���g����
    /// </summary>
    private float m_nowCountTime = 0.0f;

    /// <summary>
    /// �^�C�}�[�����ݓ����Ă��邩
    /// </summary>
    private bool m_isTimerActive = false;

    /// <summary>
    /// �V�[���J�n���Ƀ^�C�}�[���J�n���邩
    /// </summary>
    [SerializeField]
    private bool m_startOnAwake = false;

    [SerializeField]
    private bool m_isLoop = false;

    /// <summary>
    /// ���ԂɂȂ�Ɣ��΂����C�x���g
    /// </summary>
    [SerializeField]
    private UnityEvent m_timerEvent = new UnityEvent();

    /// <summary>
    /// �^�C�}�[���J�n����(���߂���)
    /// </summary>
    public void TimerStart()
    {
        m_isTimerActive = true;
        m_nowCountTime = 0.0f;
    }

    /// <summary>
    /// �^�C�}�[���~����(�I��)
    /// </summary>
    public void TimerStop()
    {
        m_isTimerActive = false;
        m_nowCountTime = 0.0f;
    }

    /// <summary>
    /// �^�C�}�[���ꎞ��~����
    /// </summary>
    public void TimerPause()
    {
        m_isTimerActive = false;
    }

    /// <summary>
    /// �^�C�}�[���J�n����(�r������)
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
        if(!m_isTimerActive)
        {
            return;
        }

        m_nowCountTime += Time.deltaTime;

        if(m_nowCountTime<m_fireTime)
        {
            return;
        }

        Debug.Log($"{gameObject.name}��TimerEvent�����΂���܂���");

        m_timerEvent.Invoke();

        if(m_isLoop)
        {
            m_nowCountTime -= m_fireTime;

            return;
        }

        TimerStop();
    }
}
