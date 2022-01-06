using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

class WaitTimerParam
{
    public float time = 1.0f;
    public float timeElapsed = 0;
    public float countSpeed = 1.0f;
    public Action endAction = null;
    public bool isEnd = false;

    public WaitTimerParam(float time, float countSpeed, Action endAction)
    {
        this.time = time;
        this.countSpeed = countSpeed;
        this.endAction = endAction;
    }

    /// <summary>
    /// タイム終了時にする処理
    /// </summary>
    /// <param name="isEndAction">終了関数を呼び出すかどうか</param>
    public void EndTimer(bool isEndAction = true)
    {
        timeElapsed = time;
        isEnd = true;

        if (isEndAction){
            endAction?.Invoke();
        }
        endAction = null;
    }
}

public class WaitTimer : MonoBehaviour
{
    private Dictionary<Type,WaitTimerParam> m_params = new Dictionary<Type, WaitTimerParam>();
    private Dictionary<Type, WaitTimerParam> m_addParams = new Dictionary<Type, WaitTimerParam>();

    private void Awake()
    {
        m_params = new Dictionary<Type,WaitTimerParam>();
    }

    private void Update()
    {
        //パラメータのUpdate
        foreach (var keyValuePair in m_params)
        {
            var param = keyValuePair.Value;

            if (param.isEnd){  //終了していたら消去候補に入れる。
                continue;
            }

            param.timeElapsed += param.countSpeed * Time.deltaTime;

            if(param.timeElapsed > param.time)
            {
                param.EndTimer();
            }
        }

        AddParams();
    }

    private void AddParams()
    {
        foreach(var param in m_addParams)
        {
            m_params[param.Key] = param.Value;
        }
    }

    /// <summary>
    /// タイマーの設置
    /// </summary>
    /// <param name="type">追加したクラスのタイプ</param>
    /// <param name="time">待つ時間</param>
    /// <param name="endAction">終了時に呼ぶアクション</param>
    /// <param name="countSpeed">カウントスピード</param>
    public void AddWaitTimer(Type type ,float time, Action endAction = null, float countSpeed = 1.0f)
    {
        var newParam = new WaitTimerParam(time, countSpeed, endAction);
        //m_params[type] = newParam;
        m_addParams[type] = newParam;
    }

    /// <summary>
    /// 待機状態かどうか
    /// </summary>
    /// <param name="type">オブジェクトのタイプ</param>
    /// <returns>待機状態ならtrue</returns>
    public bool IsWait(Type type)
    {
        //キーが存在するなら
        if (m_params.ContainsKey(type)) {
            return !m_params[type].isEnd;  //終了状態でないならtrue(待機状態)
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 強制的に待機状態を解除
    /// </summary>
    /// <param name="type">オブジェクトタイプ</param>
    /// <param name="isEndAction">終了時に呼び出す関数を呼ぶかどうか</param>
    public void AbsoluteEndTimer(Type type, bool isEndAction)
    {        
        //キーが存在するなら
        if (m_params.ContainsKey(type))
        {
            m_params[type].EndTimer(isEndAction);  //待機状態強制終了
        }
    }
}
