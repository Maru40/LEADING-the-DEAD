using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class FoundObject : MonoBehaviour
{
    public enum FoundType
    {
        None = -1,
        Player,
        SoundObject,
        BindObject,
        Smell,  //匂い
        ChildZombie, //子供ゾンビ
    }

    public struct FoundData
    {
        private GameObject gameObject;
        public int priority;
        public FoundType type;
        public Vector3 positionOffset;  //音などのポジションに乱数を持たせたい場合
        public Vector3 position => gameObject.transform.position + positionOffset;
        public bool isSamePriolityActiveRange;
        [Header("同じ優先度の場合有効となる長さ")]
        public float samePriolityActiveRange;

        public FoundData(GameObject gameObject, int priority, FoundType type, Vector3 positionOffset, 
            bool isSamePriolityActiveRange, float samePriolityActiveRange)
        {
            this.gameObject = gameObject;
            this.priority = priority;
            this.type = type;
            this.positionOffset = positionOffset;
            this.isSamePriolityActiveRange = isSamePriolityActiveRange;
            this.samePriolityActiveRange = samePriolityActiveRange;
        }
    }

    [SerializeField]
    private int m_priority = 0;

    [SerializeField]
    private FoundType m_type = FoundType.SoundObject;

    [SerializeField]
    private Vector3 m_positionOffset = new Vector3();

    [SerializeField]
    private bool m_isSamePriolityActiveRange = false;

    [SerializeField, Header("同じ優先度の場合有効となる長さ")]
    public float m_samePriolityActiveRange = 2.0f;

    private void Start()
    {
        //m_positionOffset = RandomPosition.CalcuVec(m_positionOffset);
    }

    public FoundData GetFoundData()
    {
        var positionOffset = RandomPosition.CalcuVec(transform.rotation * m_positionOffset);

        if (m_type == FoundType.Smell)
        {
            //Debug.Log("△デフォ：" + transform.rotation * m_positionOffset);
            //Debug.Log("◆新規：" + positionOffset);
        }

        return new FoundData(gameObject, m_priority, m_type, positionOffset,
            m_isSamePriolityActiveRange, m_samePriolityActiveRange);
    }
}
