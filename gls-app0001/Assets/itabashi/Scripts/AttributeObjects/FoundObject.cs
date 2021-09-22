using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundObject : MonoBehaviour
{
    public enum FoundType
    {
        Player,
        SoundObject,
    }

    public struct FoundData
    {
        public int priority;
        public FoundType type;

        public FoundData(int priority, FoundType type)
        {
            this.priority = priority;
            this.type = type;
        }
    }

    [SerializeField]
    private int m_priority = 0;

    [SerializeField]
    private FoundType m_type = FoundType.SoundObject;

    public FoundData GetFoundData()
    {
        return new FoundData(m_priority, m_type);
    }
}
