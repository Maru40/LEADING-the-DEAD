using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//親集団行動管理
public class ParentThrongManager : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float range;
        [SerializeField]
        private int sides;
        public float Sides => (float)sides;
    }

    [SerializeField]
    private Parametor m_param = new Parametor();
    private List<ThrongData> m_throngDatas = new List<ThrongData>();

    private void Awake()
    {
        
    }

    private void Update()
    {
        if(m_throngDatas.Count != 0)
        {
            ThrongUpdate();
        }
    }

    private void ThrongUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
