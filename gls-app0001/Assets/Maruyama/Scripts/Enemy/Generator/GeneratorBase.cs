using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratorBase : MonoBehaviour
{
    private uint m_deathCount = 0;

    public void AddDeathCount(uint count = 1)
    {
        m_deathCount += count;
    }

    public int DeathCount => (int)m_deathCount;
}
