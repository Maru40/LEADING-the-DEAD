using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class BlendListCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineBlendListCamera m_blendListCamera;

    [SerializeField]
    private UnityEvent m_transitionEndEvent = new UnityEvent();

    private float m_maxTime = 0.0f;

    private void Awake()
    {
        m_maxTime = 0.0f;

        foreach (var instruction in m_blendListCamera.m_Instructions)
        {
            m_maxTime += instruction.m_Hold;
            m_maxTime += instruction.m_Blend.BlendTime;
        }
    }

    public void ChangeTest(ICinemachineCamera afterCamera, ICinemachineCamera beforeCamera)
    {
        ICinemachineCamera cinemachineCamera = m_blendListCamera;

        if (afterCamera == cinemachineCamera)
        {
            StartCoroutine(TimeCount(m_maxTime));
        }
    }

    private IEnumerator TimeCount(float count)
    {
        float time = 0;

        while(time < count)
        {
            time += Time.deltaTime;

            yield return null;
        }

        m_transitionEndEvent.Invoke();
    }
}
