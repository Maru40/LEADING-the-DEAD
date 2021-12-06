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

    AnimatorCtrl_ZombieNormal m_animatorContoroller;
    RandomAnimationProvider m_randomAimationProvider;
    AnimatorManagerBase m_aniamtorManager;

    private void Awake()
    {
        m_animatorContoroller = GetComponentInParent<AnimatorCtrl_ZombieNormal>();
        m_randomAimationProvider = GetComponentInParent<RandomAnimationProvider>();
        m_aniamtorManager = GetComponentInParent<AnimatorManagerBase>();

        Provider();
    }

    private void Start()
    {
        //m_animator.
        //m_animator.avatar = m_nowParam.avatar;
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

        //m_animator.avatar = param.avatar;
        //var model = Instantiate(param.model, transform.position, Quaternion.identity, transform);
        param.model.SetActive(true);

        HitObjectProvider(param);

        m_nowParam = param;

        //いらないモデル群の削除
        m_params.Remove(param);
        RemoveModels();

        m_animator = param.model.GetComponent<Animator>();
        m_randomAimationProvider.animator = m_animator;
        m_animatorContoroller.animator = m_animator;
        m_aniamtorManager.animator = m_animator;
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
