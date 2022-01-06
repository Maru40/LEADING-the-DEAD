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
    private List<DropData> m_datas = new List<DropData>();

    private Dictionary<DropData ,GameObject> m_particles = new Dictionary<DropData, GameObject>();

    [SerializeField]
    private GameObject m_dropPositionObject = null;

    [SerializeField]
    private float m_dropPower = 100.0f;
    [SerializeField]
    private float m_dropUpPower = 50.0f;

    //test用、将来的に消す。
    [SerializeField]
    private GameObject m_tempNullParticle = null;  //particleがnullの時の仮particle

    [SerializeField]
    public bool m_isPickUp = true;  //アイテムを拾うかどうか

    private void Awake()
    {
        if (m_dropPositionObject == null)
        {
            m_dropPositionObject = this.gameObject;
        }
    }

    /// <summary>
    /// パーティクルの生成
    /// </summary>
    /// <param name="data"></param>
    private void InstatiateParticle(DropData data)
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
        Drop(null); 
    }

    /// <summary>
    /// アイテムのドロップ
    /// </summary>
    public void Drop(GameObject other)
    {
        Vector3 toVec = Vector3.zero;
        if (other) { //otherがnullでなかったら
            toVec = transform.position - other.gameObject.transform.position;
        }

        var removeDatas = new List<DropData>();
        foreach (var data in m_datas)
        {
            if (data == null)
            {
                continue;
            }
            if(data.obj == null)
            {
                continue;
            }

            var isDrop = MyRandom.RandomProbability(data.probability);
            if (isDrop)  //ドロップするなら。
            {
                //オブジェクトの生成。
                data.obj.SetActive(true);
                data.obj.transform.position = m_dropPositionObject.transform.position;
                data.obj.transform.parent = null;

                ItemAddForce(data.obj, toVec);

                removeDatas.Add(data);
                //Instantiate(data.obj, transform.position, Quaternion.identity);
                //演出の生成(particleとか？)
            }
        }

        RemoveDatas(removeDatas);
    }

    private void ItemAddForce(GameObject obj, Vector3 force)
    {
        if(force == Vector3.zero) {
            return;
        }

        var rigid = obj.GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.AddForce(Vector3.up * m_dropUpPower);
            rigid.AddForce(force.normalized * m_dropPower);
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
        if (m_isPickUp) {
            PickUp(collision.gameObject);
        }
    }

    /// <summary>
    /// 拾う
    /// </summary>
    private void PickUp(GameObject other)
    {
        var picked = other.GetComponent<PickedUpObject>();

        if (picked)
        {
            AddData(new DropData(picked.gameObject, 100));
            picked.gameObject.SetActive(false);
        }
    }
}
