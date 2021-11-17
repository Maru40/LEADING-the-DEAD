using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 血だまり管理
/// </summary>
[RequireComponent(typeof(WaitTimer))]
public class BloodPuddleManager : MonoBehaviour
{
    [SerializeField]
    float m_time = 5.0f;

    WaitTimer m_waitTimer;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    private void Start()
    {
        m_waitTimer.AddWaitTimer(GetType(), m_time, EndProcess);
    }

    /// <summary>
    /// 終了関数。
    /// </summary>
    void EndProcess()
    {
        //将来的にフェード
        Destroy(gameObject);
    }
}
