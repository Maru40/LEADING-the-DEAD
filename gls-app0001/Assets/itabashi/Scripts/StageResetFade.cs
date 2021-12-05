using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageResetFade : MonoBehaviour
{
    [SerializeField]
    private FadeObject m_fadeOutObject;
    [SerializeField]
    private FadeObject m_fadeInObject;

    [SerializeField]
    private UnityEvent m_blackOutEvent;

    [SerializeField]
    private UnityEvent m_blackOutEndEvent;


    public void FadeStart()
    {
        m_fadeOutObject.FadeStart();
        StartCoroutine(WaitBlackOut());
    }

    private IEnumerator WaitBlackOut()
    {
        while(!m_fadeOutObject.IsFinish())
        {
            yield return null;
        }

        Debug.Log("ステージが暗転しました");
        m_blackOutEvent?.Invoke();
        m_fadeInObject.FadeStart();
        StartCoroutine(WaitBlackOutEnd());
    }

    private IEnumerator WaitBlackOutEnd()
    {
        while(!m_fadeInObject.IsFinish())
        {
            yield return null;
        }

        Debug.Log("ステージ暗転が終了しました");
        m_blackOutEndEvent?.Invoke();
    }
}
