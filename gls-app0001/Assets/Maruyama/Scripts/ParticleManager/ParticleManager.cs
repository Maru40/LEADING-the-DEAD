using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using MaruUtility.UtilityDictionary;

public class ParticleManager : SingletonMonoBehaviour<ParticleManager>
{
    [System.Serializable]
    public struct ParticleData
    {
        public ParticleID id;
        [SerializeField]
        private GameObject createPositionObject;
        public Vector3 CreatePosition => createPositionObject.transform.position;
    }

    [System.Serializable]
    public enum ParticleID
    {
        None = -1,
        HitAttack_Normal,
        CloudDust,  //土煙
        MeatParticle,  //肉が弾ける。
        Fire,  //炎
    }

    [SerializeField]
    private Ex_Dictionary<ParticleID, GameObject> m_particleDictionary = new Ex_Dictionary<ParticleID, GameObject>();

    private void Start()
    {
        m_particleDictionary.InsertInspectorData();
    }

    /// <summary>
    /// パーティクルの生成
    /// </summary>
    /// <param name="id">パーティクルID</param>
    /// <param name="position">生成ポジション</param>
    /// <returns>生成したパーティクル</returns>
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

    /// <summary>
    /// パーティクルの生成
    /// </summary>
    /// <param name="id">パーティクルのID</param>
    /// <param name="position">生成ポジション</param>
    /// <param name="scale">生成スケール</param>
    /// <returns></returns>
    public GameObject Play(ParticleID id, Vector3 position, Vector3 scale)
    {
        if (!m_particleDictionary.ContainsKey(id))
        {  //particleが存在しないなら
            Debug.Log("idが存在しません");
            return null;
        }

        var prefab = m_particleDictionary[id];
        if (prefab == null)
        {  //prefabが存在しないなら
            Debug.Log("prefabが存在しません。");
            return null;
        }

        var particle = Instantiate(prefab, position, Quaternion.identity);  //生成
        particle.transform.localScale = scale;
        particle.transform.SetParent(transform);     //ParticleManagerの子供にする。

        return particle;
    }
}
