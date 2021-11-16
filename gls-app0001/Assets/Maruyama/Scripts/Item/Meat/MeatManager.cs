using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float time; //捕食時間
        public MeatState state;
        public float count;  //食べられた回数
    }

    [SerializeField]
    Parametor m_param = new Parametor();
    GameTimer m_timer =  new GameTimer();

    void Update()
    {
        m_timer.UpdateTimer();

        StateCheck();

        if (m_timer.IsTimeUp)  //時間経過
        {
            ParticleManager.Instance.Play(ParticleManager.ParticleID.MeatParticle, transform.position);
            m_param.state = MeatState.Destroy;
            Destroy(this.gameObject, 0.1f);
        }
    }

    /// <summary>
    /// ステートの監視
    /// </summary>
    void StateCheck()
    {
        const float half = 0.5f;
        if (m_timer.TimeRate > half)
        {
            ChageState(MeatState.Half);
        }
    }

    /// <summary>
    /// ステートの変更
    /// </summary>
    /// <param name="state"></param>
    void ChageState(MeatState state)
    {
        m_param.state = state;

        System.Action func = state switch
        {
            MeatState.Half => () => ChangeHalf(),
            _ => null
        };

        func?.Invoke();
    }

    /// <summary>
    /// 肉をハーフ状態にする。
    /// </summary>
    void ChangeHalf()
    {

    }

    /// <summary>
    /// 捕食開始
    /// </summary>
    public void EatenStart()
    {
        m_timer.ResetTimer(m_param.time);
    }

    /// <summary>
    /// 食べられたときの反応
    /// </summary>
    public void Eaten()
    {
        ParticleManager.Instance.Play(ParticleManager.ParticleID.MeatParticle, transform.position);
        m_param.count++;
    }
}
