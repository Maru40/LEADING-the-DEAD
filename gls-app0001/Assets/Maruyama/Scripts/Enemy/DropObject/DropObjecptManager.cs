using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MaruUtility;

using System;

[Serializable]
public class DropData
{
    public GameObject obj;
    public string prefabPath;
    public float probability;  //ドロップする確率
    public GameObject particle;
    public bool isRemove = false;

    public DropData(GameObject obj, float probability)
        :this(obj, probability, null)
    { }

    public DropData(GameObject obj, float probability, GameObject particle)
    {
        this.obj = obj;
        this.prefabPath = "Assets/Resources/Prefabs/CymbalMonkey.prefab";
        this.probability = probability;
        this.particle = particle;
    }
}

public class DropObjecptManager : MonoBehaviour
{
    [SerializeField]
    List<DropData> m_datas = new List<DropData>();

    Dictionary<DropData ,GameObject> m_particles = new Dictionary<DropData, GameObject>();

    //test用、将来的に消す。
    [SerializeField]
    GameObject m_tempNullParticle = null;  //particleがnullの時の仮particle

    void InstatiateParticle(DropData data)
    {
        if (data.particle != null)
        {
            var particle = Instantiate(data.particle, transform.position, Quaternion.identity, transform);
            m_particles[data] = particle;
        }
        else
        {//nullの時のparticle,test要であり、将来的に消す。
            if (m_tempNullParticle)
            {
                var particle = Instantiate(m_tempNullParticle, transform.position, Quaternion.identity, transform);
                m_particles[data] = particle;
            }
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
                data.obj.SetActive(true);
                data.obj.transform.position = transform.position;
                //Instantiate(data.obj, transform.position, Quaternion.identity);
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


    //Collision---------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        var picked = collision.gameObject.GetComponent<PickedUpObject>();

        if (picked) {
            AddData(new DropData(picked.gameObject, 100));
            picked.gameObject.SetActive(false);
        }
    }
}
