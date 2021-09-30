using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoRecovery : MonoBehaviour
{
    [SerializeField]
    private float m_recoveryCoolTime;

    [SerializeField]
    private float m_recoveryValue;

    [SerializeField]
    private UnityEvent<float> m_recoveryEvent;

    private float m_nowCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_nowCoolTime += Time.deltaTime;

        if(m_nowCoolTime < m_recoveryCoolTime)
        {
            return;
        }

        m_nowCoolTime -= m_recoveryCoolTime;

        m_recoveryEvent?.Invoke(m_recoveryValue);
    }
}
