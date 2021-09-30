using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MyTrigger
{
    bool m_isTrigger;

    /// <summary>
    /// Triggerをtrueにする。
    /// </summary>
    public void Fire()
    {
        m_isTrigger = true;
    }

    /// <summary>
    /// 現在のトリガーを取得し、トリガーをoffにする。
    /// </summary>
    /// <returns>トリガーの値(bool型)</returns>
    public bool Get()
    {
        bool isTrigger = m_isTrigger;
        m_isTrigger = false;
        return isTrigger;
    }
}
