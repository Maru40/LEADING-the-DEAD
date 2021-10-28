using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MaruUtility;

/// <summary>
/// randomにアニメーションクリップを再生するためのパラメータ
/// </summary>
[Serializable]
public class RandomAnimationProviderParametor
{
    public string nowClipName;                  //現在使用中のAnimationClip
    public List<AnimationClip> animationClips;  //randomに再生したいAnimationClipList

    public RandomAnimationProviderParametor(string nowClipName, List<AnimationClip> animationClips)
    {
        this.nowClipName = nowClipName;
        this.animationClips = animationClips;
    }
}

/// <summary>
/// randomなAnimationClipに設定してくれるクラス
/// </summary>
public class RandomAnimationProvider : MonoBehaviour
{
    [SerializeField]
    List<RandomAnimationProviderParametor> m_params;

    private AnimatorOverrideController m_overrideController;
    private Animator m_animator;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        m_overrideController = new AnimatorOverrideController();
        m_overrideController.runtimeAnimatorController = m_animator.runtimeAnimatorController;

        m_animator.runtimeAnimatorController = m_overrideController;

        RandomChangeAnimationClips();
    }

    /// <summary>
    /// 渡されたデータの中で、randomなアニメーションクリップを返す。
    /// </summary>
    /// <param name="param">データ</param>
    /// <returns>randomなアニメーションクリップ</returns>
    AnimationClip GetRandomAnimationClip(RandomAnimationProviderParametor param)
    {
        return MyRandom.RandomList(param.animationClips);
    }

    /// <summary>
    /// 渡されたデータのみ、randomなアニメーションに変更する。
    /// </summary>
    /// <param name="param">randomに変更したいデータ</param>
    void RandomChangeAnimationClip(RandomAnimationProviderParametor param)
    {
        ChangeAnimationClip(param, GetRandomAnimationClip(param));
    }

    /// <summary>
    /// 全てのデータをrandomなアニメーションに変更する。
    /// </summary>
    void RandomChangeAnimationClips()
    {
        foreach(var param in m_params)
        {
            RandomChangeAnimationClip(param);
        }
    }

    /// <summary>
    /// アニメーションを変更する
    /// </summary>
    /// <param name="param">変更を管理するデータ</param>
    /// <param name="clip">変更したいアニメーション</param>
    void ChangeAnimationClip(RandomAnimationProviderParametor param ,AnimationClip clip)
    {
        if(clip == null) {
            Debug.Log("clipがnullです");
            return;
        }

        // ステートを変更される
        m_overrideController[param.nowClipName] = clip;
        param.nowClipName = clip.name;
        //m_animator.Update(0.0f);
    }


    //void Test()
    //{
    //    // ステートをキャッシュ
    //    //AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[m_animator.layerCount];
    //    //for (int i = 0; i < m_animator.layerCount; i++)
    //    //{
    //    //    layerInfo[i] = m_animator.GetCurrentAnimatorStateInfo(i);
    //    //}

    //    // ステートを戻す
    //    //for (int i = 0; i < m_animator.layerCount; i++)
    //    //{
    //    //    m_animator.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
    //    //}
    //}
}
