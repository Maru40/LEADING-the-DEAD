using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorBase : MonoBehaviour
{
    uint m_deathCount = 0;

    public void AddDeathCount(uint count = 1)
    {
        m_deathCount += count;
    }
}
