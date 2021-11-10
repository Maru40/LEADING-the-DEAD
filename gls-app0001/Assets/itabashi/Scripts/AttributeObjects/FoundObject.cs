using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

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
        public Vector3 positionOffset;  //音などのポジションに乱数を持たせたい場合
        public Vector3 position => gameObject.transform.position + positionOffset;

        public FoundData(GameObject gameObject, int priority, FoundType type, Vector3 positionOffset)
        {
            this.gameObject = gameObject;
            this.priority = priority;
            this.type = type;
            this.positionOffset = positionOffset;
        }
    }

    [SerializeField]
    private int m_priority = 0;

    [SerializeField]
    private FoundType m_type = FoundType.SoundObject;

    [SerializeField]
    private Vector3 m_positionOffset = new Vector3();

    public FoundData GetFoundData()
    {
        var offset = RandomPosition.CalcuVec(m_positionOffset);
        return new FoundData(gameObject, m_priority, m_type, offset);
    }
}
