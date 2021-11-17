using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility.UtilityDictionary;

public class MeatManager : MonoBehaviour
{
    enum MeatState
    {
        Normal,
        Half,
        Destroy,
    }

    [System.Serializable]
    struct Parametor
    {
        public MeatState state;
        public float maxEatCount;      //最大の食べられた回数。
        public float elapsedEatCount;  //食べられた回数
    }

    [SerializeField]
    Ex_Dictionary<MeatState, GameObject> m_modelDictionary = new Ex_Dictionary<MeatState, GameObject>();

    [SerializeField]
    Parametor m_param = new Parametor();

    private void Awake()
    {
        m_modelDictionary.InsertInspectorData();
    }

    void Update()
    {
        StateCheck(); //ステートのチェック
    }

    /// <summary>
    /// ステートの監視
    /// </summary>
    void StateCheck()
    {
        var rate = m_param.elapsedEatCount / m_param.maxEatCount;

        const float half = 0.5f;
        if(rate > half)
        {
            ChangeState(MeatState.Half);
        }
    }

    /// <summary>
    /// ステートの変更
    /// </summary>
    /// <param name="state"></param>
    void ChangeState(MeatState state)
    {
        ChangeModel(state);

        m_param.state = state;
    }

    void ChangeModel(MeatState state)
    {
        //どちらのモデルも存在したら。
        if(m_modelDictionary.ContainsKey(m_param.state) &&
            m_modelDictionary.ContainsKey(state))
        {
            m_modelDictionary[state].SetActive(false);
            m_modelDictionary[state].SetActive(true);
        }
    }

    /// <summary>
    /// 食べられたときの反応
    /// </summary>
    public void Eaten()
    {
        ParticleManager.Instance.Play(ParticleManager.ParticleID.MeatParticle, transform.position);
        m_param.elapsedEatCount++;
        StateCheck();

        if (m_param.elapsedEatCount >= m_param.maxEatCount) //最大捕食回数が超えたら。
        {
            EndProcess();
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    void EndProcess()
    {
        ParticleManager.Instance.Play(ParticleManager.ParticleID.MeatParticle, transform.position);
        m_param.state = MeatState.Destroy;
        Destroy(this.gameObject, 0.1f);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Eaten();
    //}
}
