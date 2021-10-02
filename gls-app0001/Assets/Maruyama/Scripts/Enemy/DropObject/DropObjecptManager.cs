using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaruUtility;

using System;

[Serializable]
public class DropData
{
    public GameObject obj;
    public float probability;  //ドロップする確率

    public DropData(GameObject obj, float probability)
    {
        this.obj = obj;
        this.probability = probability;
    }
}

public class DropObjecptManager : MonoBehaviour
{
    [SerializeField]
    List<DropData> m_datas = new List<DropData>();

    public void Drop()
    {
        foreach(var data in m_datas)
        {
            if(data == null) {
                continue;
            }

            var isDrop = MyRandom.RandomProbability(data.probability);
            if (isDrop)  //ドロップするなら。
            {
                //オブジェクトの生成。
                Instantiate(data.obj, transform.position, Quaternion.identity);
                //演出の生成(particleとか？)

            }
        }   
    }

    
    //アクセッサ------------------------------------------------------------------

    public void AddData(DropData data)
    {
        m_datas.Add(data);
    }

    public void RemoveData(DropData data)
    {
        m_datas.Remove(data);
    }
}
