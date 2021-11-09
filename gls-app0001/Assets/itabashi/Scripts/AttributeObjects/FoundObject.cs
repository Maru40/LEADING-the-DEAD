﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundObject : MonoBehaviour
{
    public enum FoundType
    {
        Player,
        SoundObject,
        BindObject,
    }

    public struct FoundData
    {
        private GameObject gameObject;
        public int priority;
        public FoundType type;

        public FoundData(GameObject gameObject, int priority, FoundType type)
        {
            this.gameObject = gameObject;
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
        return new FoundData(gameObject, m_priority, m_type);
    }
}
