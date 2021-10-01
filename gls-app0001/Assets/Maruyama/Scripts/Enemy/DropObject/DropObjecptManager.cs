using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
struct DropData
{
    public GameObject obj;
    public float probability;  //ドロップする確率
}

public class DropObjecptManager : MonoBehaviour
{
    [SerializeField]
    List<DropData> m_datas = new List<DropData>();

    public void Drop()
    {
        foreach(var data in m_datas)
        {
            
        }   
    }
}
