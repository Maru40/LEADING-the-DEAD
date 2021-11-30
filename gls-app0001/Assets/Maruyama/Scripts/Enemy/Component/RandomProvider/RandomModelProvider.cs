using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class RandomModelProvider : MonoBehaviour
{
    [System.Serializable]
    public class HitObjectParametor
    {
        public GameObject selfObject;   //セットしたいオブジェクト本体
        public GameObject parentObject; //親に設定したいオブジェクト
    }

    [System.Serializable]
    public class Parametor {
        //モデルデータ
        public GameObject model;
        public Avatar avatar;
        public List<HitObjectParametor> hitObjParams;
    }

    [SerializeField]
    Animator m_animator;

    [SerializeField]
    List<Parametor> m_params = new List<Parametor>();

    Parametor m_nowParam;

    private void Awake()
    {
        Provider();
    }

    private void Start()
    {
        m_animator.avatar = m_nowParam.avatar;
    }

    /// <summary>
    /// 提供
    /// </summary>
    private void Provider()
    {
        var param = MyRandom.RandomList(m_params);
        if(param == null) {
            return;
        }

        //いらないモデル群の削除
        m_params.Remove(param);
        RemoveModels();

        m_animator.avatar = param.avatar;
        //var model = Instantiate(param.model, transform.position, Quaternion.identity, transform);
        param.model.SetActive(true);

        HitObjectProvider(param);

        m_nowParam = param;
    }

    private void HitObjectProvider(Parametor param)
    {
        foreach (var hitParam in param.hitObjParams)
        {
            hitParam.selfObject.transform.parent = hitParam.parentObject.transform;
        }
    }

    private void RemoveModels()
    {
        foreach(var param in m_params)
        {
            Destroy(param.model);
        }
    }

}
