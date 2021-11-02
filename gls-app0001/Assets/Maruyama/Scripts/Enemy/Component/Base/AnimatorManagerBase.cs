﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorManagerBase : MonoBehaviour
{
    protected Animator m_animator;
    protected StatusManagerBase m_statusManager; 

    virtual protected void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_statusManager = GetComponent<StatusManagerBase>();
    }

    public void CrossFadeState(string stateName, string layerName, float transitionTime = 0.0f)
    {
        int layerIndex = m_animator.GetLayerIndex(layerName);
        m_animator.CrossFade(stateName, transitionTime, layerIndex);
    }

    public void CrossFadeState(string stateName, int layerIndex, float transitionTime = 0.0f)
    {
        m_animator.CrossFade(stateName, transitionTime, layerIndex);
    }

    /// <summary>
    /// HitStopを開始する
    /// </summary>
    /// <param name="data">ダメージデータ</param>
    public void HitStop(AttributeObject.DamageData data)
    {
        StartCoroutine(HitStopCoroutine(data.hitStopTime));
    }

    //Coroutine-------------------------------------------------------------------------------

    /// <summary>
    /// ヒットストップを管理するコルーチン
    /// </summary>
    /// <param name="intervalTime">ヒットストップする時間</param>
    /// <returns></returns>
    protected IEnumerator HitStopCoroutine(float intervalTime)
    {
        float animatorSpeed = m_animator.speed;

        m_animator.speed = 0.0f;
        m_statusManager.IsHitStop = true;  //ヒットストップ状態にする。

        float elapsedTime = 0.0f;

        while (elapsedTime < intervalTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_animator.speed = animatorSpeed;
        m_statusManager.IsHitStop = false; //ヒットストップ解除
    }

    //アクセッサ・プロパティ----------------------------------------------------------------------

    public Animator GetAniamtor()
    {
        return m_animator;
    }
}
