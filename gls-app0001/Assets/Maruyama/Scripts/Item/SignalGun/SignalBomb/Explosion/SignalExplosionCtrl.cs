using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AlarmObject))]
[RequireComponent(typeof(WaitTimer))]
[RequireComponent(typeof(BindActivateArea))]
public class SignalExplosionCtrl : MonoBehaviour
{
    [SerializeField]
    private float m_alarmTime = 5.0f;

    private AlarmObject m_alarm;
    private WaitTimer m_waitTimer;
    private BindActivateArea m_bindArea;

    private void Awake()
    {
        m_alarm = GetComponent<AlarmObject>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_bindArea = GetComponent<BindActivateArea>();
    }

    private void Start()
    {
        m_alarm.AlarmStart();
        m_waitTimer.AddWaitTimer(GetType(), m_alarmTime, EndAlarm);
        BindAreaMove();
    }

    private void EndAlarm()
    {
        m_alarm.AlarmStop();
        //m_bindArea.GetAreaCenterObject().GetComponent<Collider>().enabled = false;

        Destroy(this.gameObject, 0.1f);
    }

    private void BindAreaMove()
    {
        var direct = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direct,out hit))
        {
            m_bindArea.GetAreaCenterObject().transform.position = hit.point;
        }
    }
}
