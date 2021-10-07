﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行動を制限するオブジェクト
/// </summary>
public class BindActivateArea : MonoBehaviour
{
    [SerializeField]
    float m_bindRange = 10.0f;  //行動制限範囲

    /// <summary>
    /// 行動制限する距離の取得
    /// </summary>
    /// <returns>行動制限する距離</returns>
    public float GetBindRange()
    {
        return m_bindRange;
    }

    public void Bind(Collider other)
    {
        var bind = other.GetComponent<I_BindedActiveArea>();
        bind?.Bind(this);
    }

    //バインド解除
    public void BindRelease(Collider other)
    {
        var bind = other.GetComponent<I_BindedActiveArea>();
        bind?.BindRelease(this);
    }
}
