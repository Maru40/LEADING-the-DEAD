using UnityEngine;
using UniRx;
using System;

public class StageTimer : MonoBehaviour
{
    [SerializeField]
    private FloatReactiveProperty m_timeSeconds = new FloatReactiveProperty(0.0f);

    public float timeSeconds => m_timeSeconds.Value;
    public IObservable<float> OnChangedTimeSeconds => m_timeSeconds;

    [SerializeField]
    private BoolReactiveProperty m_isCount = new BoolReactiveProperty(false);

    public bool isCount => m_isCount.Value;
    public IObservable<bool> OnChangedIsCount => m_isCount;


    private void Update()
    {
        if(isCount)
        {
            m_timeSeconds.Value += Time.deltaTime;
        }
    }

    public void TimerStart()
    {
        m_isCount.Value = true;
    }

    public void TimerStop()
    {
        m_isCount.Value = false;
    }
}
