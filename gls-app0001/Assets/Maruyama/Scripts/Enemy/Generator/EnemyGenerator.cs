using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_createObject = null;

    //���Ń]���r��z��ɓ����
    [SerializeField]
    protected List<ThrongData> m_datas = new List<ThrongData>();

    private void Start()
    {
        //��
        var objs = FindObjectsOfType<ThrongMgr>();
        foreach(var obj in objs)
        {
            m_datas.Add(new ThrongData(obj.GetComponent<Rigidbody>(),
                obj.GetComponent<TargetMgr>(),
                obj.GetComponent<ThrongMgr>()
            ));
        }
    }

    private void Update()
    {
        Debug.Log(m_datas.Count);
    }

    //�A�N�Z�b�T---------------------------------------

    public List<ThrongData> GetThrongDatas()
    {
        return m_datas;
    }

    public GameObject GetCreateObject()
    {
        return m_createObject;
    }
}
