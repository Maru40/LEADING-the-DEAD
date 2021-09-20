using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundObject : MonoBehaviour
{
    public struct FoundData
    {
        public int priority;

        public FoundData(int priority)
        {
            this.priority = priority;
        }
    }

    [SerializeField]
    private int m_priority;

    public FoundData GetFoundData()
    {
        return new FoundData(m_priority);
    }
}
