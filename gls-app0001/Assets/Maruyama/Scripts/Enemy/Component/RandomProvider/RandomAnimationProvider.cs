using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using MaruUtility;

[Serializable]
public class AnimationClipParametor
{
    public AnimationClip animationClip = null;
    public float speed = 1.0f;
}

/// <summary>
/// randomにアニメーションクリップを再生するためのパラメータ
/// </summary>
[Serializable]
public class RandomAnimationProviderParametor
{
    public string nowClipName;                  //現在使用中のAnimationClip
    public List<AnimationClipParametor> animationClipParametors;  //randomに再生したいAnimationClipList

    public RandomAnimationProviderParametor(string nowClipName, List<AnimationClipParametor> animationClipParametors)
    {
        this.nowClipName = nowClipName;
        this.animationClipParametors = animationClipParametors;
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
    AnimatorCtrl_ZombieNormal m_animatorController;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animatorController = GetComponent<AnimatorCtrl_ZombieNormal>();
    }

    void Start()
    {
        m_overrideController = new AnimatorOverrideController(m_animator.runtimeAnimatorController);

        m_animator.runtimeAnimatorController = m_overrideController;

        RandomChangeAnimationClips();


    }

    /// <summary>
    /// 渡されたデータの中で、randomなアニメーションクリップを返す。
    /// </summary>
    /// <param name="param">データ</param>
    /// <returns>randomなアニメーションクリップ</returns>
    AnimationClipParametor GetRandomAnimationClip(RandomAnimationProviderParametor param)
    {
        var clipParam = MyRandom.RandomList(param.animationClipParametors);
        return clipParam;
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
    void ChangeAnimationClip(RandomAnimationProviderParametor param ,AnimationClipParametor clipParam)
    {
        if(clipParam.animationClip == null) {
            Debug.Log("clipがnullです");
            return;
        }

        // ステートを変更される
        m_overrideController[param.nowClipName] = clipParam.animationClip;
        param.nowClipName = clipParam.animationClip.name;
        m_animatorController.BaseMoveSpeed = clipParam.speed;
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
