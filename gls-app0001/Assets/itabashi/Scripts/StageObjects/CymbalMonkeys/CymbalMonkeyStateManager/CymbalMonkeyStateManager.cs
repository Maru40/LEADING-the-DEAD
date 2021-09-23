using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CymbalMonkeyStateManager : MonoBehaviour
{
    public enum CymbalMonkeyState
    {
        Normal,
        ExplosionPreparation
    }

    [SerializeField]
    private string[] m_groundTags;

    public CymbalMonkeyState nowState { private set; get; } = CymbalMonkeyState.Normal;

    private Rigidbody m_rigidbody;
    private TimingDevice m_timingDevice;
    private AlarmObject m_alarmObject;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_timingDevice = GetComponent<TimingDevice>();
        m_alarmObject = GetComponent<AlarmObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nowState == CymbalMonkeyState.ExplosionPreparation)
        {
            return;
        }

        bool isCompare = false;

        foreach (var groundTag in m_groundTags)
        {
            isCompare = other.gameObject.CompareTag(groundTag);

            if (isCompare)
            {
                break;
            }
        }

        if (!isCompare)
        {
            return;
        }

        m_rigidbody.isKinematic = true;
        m_timingDevice.TimerStart();
        m_alarmObject.AlarmStart();

        nowState = CymbalMonkeyState.ExplosionPreparation;
    }
}
