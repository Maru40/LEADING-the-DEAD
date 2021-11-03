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

    [SerializeField]
    private bool m_alarmSwitch = false;

    public bool alarmSwitch
    {
        set
        {
            m_alarmSwitch = value;
            
            if(!value)
            {
                m_alarmObject.AlarmStop();
                nowState = CymbalMonkeyState.Normal;
            }
        }
        
        get => m_alarmSwitch;
    }

    public CymbalMonkeyState nowState { private set; get; } = CymbalMonkeyState.Normal;

    private Rigidbody m_rigidbody;
    private AlarmObject m_alarmObject;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_alarmObject = GetComponent<AlarmObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (nowState == CymbalMonkeyState.ExplosionPreparation || !m_alarmSwitch)
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
        m_alarmObject.AlarmStart();

        nowState = CymbalMonkeyState.ExplosionPreparation;
    }
}
