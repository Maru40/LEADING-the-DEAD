using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using MaruUtility.UtilityDictionary;

public class ParticleManager : SingletonMonoBehaviour<ParticleManager>
{
    public enum ParticleID
    {
        None = -1,
        HitAttack_Normal,
        CloudDust  //土煙
    }

    [SerializeField]
    Ex_Dictionary<ParticleID, GameObject> m_particleDictionary = new Ex_Dictionary<ParticleID, GameObject>();

    void Start()
    {
        m_particleDictionary.InsertInspectorData();

        Debug.Log(m_particleDictionary.Count);
    }

    public GameObject Play(ParticleID id, Vector3 position)
    {
        if(!m_particleDictionary.ContainsKey(id)) {  //particleが存在しないなら
            Debug.Log("idが存在しません");
            return null;
        }

        var prefab = m_particleDictionary[id];
        if(prefab == null){  //prefabが存在しないなら
            Debug.Log("prefabが存在しません。");
            return null;
        }

        var particle = Instantiate(prefab, position, Quaternion.identity);  //生成
        particle.transform.SetParent(transform);     //ParticleManagerの子供にする。

        return particle;
    }
}
