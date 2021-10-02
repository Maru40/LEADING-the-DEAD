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
    public GameObject particle;

    public DropData(GameObject obj, float probability)
        :this(obj, probability, null)
    { }

    public DropData(GameObject obj, float probability, GameObject particle)
    {
        this.obj = obj;
        this.probability = probability;
        this.particle = particle;
    }
}

public class DropObjecptManager : MonoBehaviour
{
    [SerializeField]
    List<DropData> m_datas = new List<DropData>();

    Dictionary<DropData ,GameObject> m_particles = new Dictionary<DropData, GameObject>();

    void InstatiateParticle(DropData data)
    {
        if (data.particle != null)
        {
            var particle = Instantiate(data.particle, transform.position, Quaternion.identity, transform);
            m_particles[data] = particle;
        }
    }

    /// <summary>
    /// アイテムのドロップ
    /// </summary>
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

        InstatiateParticle(data);
    }

    public void RemoveData(DropData data)
    {
        if (m_particles.ContainsKey(data))  //存在したら
        {
            Destroy(m_particles[data]);
            m_particles.Remove(data);
        }

        m_datas.Remove(data);
    }

    public void RemoveDatas(List<DropData> datas)
    {
        for(int i = 0; i < datas.Count; i++)
        {
            RemoveData(datas[i]);
        }
    }

    public int GetNumData()
    {
        return m_datas.Count;
    }

    public List<DropData> GetDatas()
    {
        return m_datas;
    }
}
