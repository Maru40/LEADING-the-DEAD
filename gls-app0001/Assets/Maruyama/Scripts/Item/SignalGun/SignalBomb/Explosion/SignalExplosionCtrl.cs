using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AlarmObject))]
[RequireComponent(typeof(WaitTimer))]
public class SignalExplosionCtrl : MonoBehaviour
{
    [SerializeField]
    float m_alarmTime = 5.0f;

    [SerializeField]
    GameObject m_bindArea = null;

    AlarmObject m_alarm;
    WaitTimer m_waitTimer;

    void Awake()
    {
        m_alarm = GetComponent<AlarmObject>();
        m_waitTimer = GetComponent<WaitTimer>();
    }

    private void Start()
    {
        m_alarm.AlarmStart();
        m_waitTimer.AddWaitTimer(GetType(), m_alarmTime, EndAlarm);
        BindAreaMove();
    }

    void EndAlarm()
    {
        m_alarm.AlarmStop();

        Destroy(this.gameObject, 0.1f);
    }

    void BindAreaMove()
    {
        var direct = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direct,out hit))
        {
            m_bindArea.transform.position = hit.point;
        }
    }
}
