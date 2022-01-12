using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility.UtilityDictionary;

public class MeatManager : EatenBase
{
    private enum MeatState
    {
        Normal,
        Half,
        Destroy,
    }

    [System.Serializable]
    private struct Parametor
    {
        public MeatState state;
        public float maxEatCount;      //最大の食べられた回数。
        public float elapsedEatCount;  //食べられた回数
    }

    [SerializeField]
    private Ex_Dictionary<MeatState, GameObject> m_modelDictionary = new Ex_Dictionary<MeatState, GameObject>();

    [SerializeField]
    private Parametor m_param = new Parametor();

    private AudioManager m_audioManager;

    private void Awake()
    {
        m_audioManager = GetComponent<AudioManager>();
        m_modelDictionary.InsertInspectorData();
    }

    private void Update()
    {
        //StateCheck(); //ステートのチェック
    }

    /// <summary>
    /// ステートの監視
    /// </summary>
    private void StateCheck()
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
    private void ChangeState(MeatState state)
    {
        ChangeModel(state);

        m_param.state = state;
    }

    private void ChangeModel(MeatState state)
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
    public override void Eaten(float power)
    {
        ParticleManager.Instance?.Play(ParticleManager.ParticleID.MeatParticle, transform.position);
        m_param.elapsedEatCount += power;
        m_audioManager?.PlayOneShot();
        StateCheck();

        if (m_param.elapsedEatCount >= m_param.maxEatCount) //最大捕食回数が超えたら。
        {
            EndProcess();
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    private void EndProcess()
    {
        ParticleManager.Instance.Play(ParticleManager.ParticleID.MeatParticle, transform.position);
        m_param.state = MeatState.Destroy;

        const float time = 0.1f;
        var parent = transform.parent;
        if (parent)
        {
            Destroy(parent.gameObject, time);
            return;
        }

        Destroy(gameObject, time);
    }
}
