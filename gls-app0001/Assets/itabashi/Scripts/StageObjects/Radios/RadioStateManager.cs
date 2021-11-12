using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioStateManager : MonoBehaviour
{
    public enum RadioState
    {
        Normal,
        ExplosionPreparation
    }

    [SerializeField]
    private string[] m_groundTags;

    [SerializeField]
    private bool m_alarmSwitch = false;

    [SerializeField]
    private float m_stopTimerSecond = 5.0f;

    public bool alarmSwitch
    {
        set
        {
            m_alarmSwitch = value;
            
            if(!value)
            {
                m_alarmObject.AlarmStop();
                nowState = RadioState.Normal;
                Debug.Log("アラームが停止しました");
            }
        }
        
        get => m_alarmSwitch;
    }

    public RadioState nowState { private set; get; } = RadioState.Normal;

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
        if (nowState == RadioState.ExplosionPreparation || !m_alarmSwitch)
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

        nowState = RadioState.ExplosionPreparation;

        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        float countTimer = 0.0f;

        while(countTimer < m_stopTimerSecond)
        {
            countTimer += Time.deltaTime;

            yield return null;
        }

        alarmSwitch = false;
    }
}
