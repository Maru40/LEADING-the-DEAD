using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// エネミーリスポーン時にステータスの工場
/// </summary>
public abstract class EnemyRespawnStatusUpBase : MonoBehaviour
{
    private float m_respawnCount = 0;

    abstract protected void StatusUp();

    virtual public void Respawn()
    {
        m_respawnCount++;
    }
}
