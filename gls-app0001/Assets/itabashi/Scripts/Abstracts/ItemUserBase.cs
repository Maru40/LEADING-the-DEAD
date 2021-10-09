using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ItemUserBase : MonoBehaviour
{
    /// <summary>
    /// 使われているかどうか
    /// </summary>
    private BoolReactiveProperty m_isUse = new BoolReactiveProperty(false);

    /// <summary>
    /// 使われているかどうか
    /// </summary>
    public bool isUse
    {
        set => m_isUse.Value = value;
        get => m_isUse.Value;
    }

    public System.IObservable<bool> isUseOnChanged => m_isUse;

    /// <summary>
    /// アイテムを使う関数
    /// </summary>
    public void Use()
    {
        if(isUse)
        {
            OnUse();
        }
    }

    /// <summary>
    /// アイテムが使われた際のコールバック韓嵩
    /// </summary>
    protected abstract void OnUse();
}
