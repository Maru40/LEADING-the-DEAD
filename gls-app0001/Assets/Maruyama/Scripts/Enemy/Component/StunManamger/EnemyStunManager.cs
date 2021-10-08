using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
struct StunParametor
{
    public float time;

    public StunParametor(float time)
    {
        this.time = time;
    }
}

public class EnemyStunManager : MonoBehaviour
{
    [SerializeField]
    StunParametor m_param = new StunParametor(3.0f);

    [SerializeField]
    List<ChangeCompParam> m_changeCompParam = new List<ChangeCompParam>();

    WaitTimer m_waitTimer;
    I_Stun m_stun;

    void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
        m_stun = GetComponent<I_Stun>();

        if(m_changeCompParam.Count == 0)
        {
            SetDefaultChangeComps();
        }
    }

    public void StartStun()
    {
        StartStun(m_param.time);
    }

    public void StartStun(float time)
    {
        Debug.Log("スタン");

        //切り替えるコンポーネントの今の状態の記録
        SaveNowEnableComps();
        //コンポーネントの切替。
        ChangeComps(false);

        m_waitTimer.AddWaitTimer(GetType(), time, EndStun);
    }

    private void EndStun()
    {
        //スタン解除の処理
        m_stun.EndStun();

        //コンポーネントの切替
        ReverseChangeComps();
    }

    /// <summary>
    /// 現在のコンポーネントのenable状態の記録
    /// </summary>
    void SaveNowEnableComps()
    {
        foreach(var param in m_changeCompParam)
        {
            if (param.behaviour)
            {
                param.isExit = param.behaviour.enabled;
            }
        }
    }

    void ChangeComps(bool isEnable)
    {
        foreach(var param in m_changeCompParam)
        {
            if (param.behaviour)
            {
                param.behaviour.enabled = isEnable;
            }
        }
    }

    /// <summary>
    /// コンポーネントを元の状態に戻す
    /// </summary>
    void ReverseChangeComps()
    {
        foreach(var param in m_changeCompParam)
        {
            if (param.behaviour)
            {
                param.behaviour.enabled = param.isExit;
            }
        }
    }

    /// <summary>
    /// Componentの指定がない場合のみ、デフォで指定。
    /// </summary>
    void SetDefaultChangeComps()
    {

    }

    //アクセッサ--------------------------------------------------------------------------

    public void SetStunTime(float time)
    {
        m_param.time = time;
    }
    public float GetStunTime()
    {
        return m_param.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //test用

        //var player = collision.gameObject.GetComponent<PlayerPickUpper>();
        //if (player)
        //{
        //    m_stun.StartStun();
        //}
    }
}
