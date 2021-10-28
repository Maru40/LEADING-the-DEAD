using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class TestRandomAnimationProvider : MonoBehaviour
{
    [SerializeField]
    List<AnimationClip> m_animationClips = new List<AnimationClip>();

    [SerializeField]
    string m_overrideClipName = "NormalAttack"; // 上書きするAnimationClip対象

    private AnimatorOverrideController m_overrideController;
    private Animator m_animator;

    //public RandomAnimationProvider(List<AnimationClip> clips)
    //{
    //    this.m_animationClips = clips;
    //}

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        m_overrideController = new AnimatorOverrideController();
        m_overrideController.runtimeAnimatorController = m_animator.runtimeAnimatorController;

        Debug.Log(m_overrideController["Z_Idle"]);

        var clips = m_overrideController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            Debug.Log(clips[i]);
        }

        m_animator.runtimeAnimatorController = m_overrideController;

        RandomChangeAnimationClip();
    }

    public AnimationClip GetRandomAnimationClip()
    {
        return MyRandom.RandomList(m_animationClips);
    }

    public void RandomChangeAnimationClip()
    {
        ChangeAnimationClip(GetRandomAnimationClip());
    }

    public void ChangeAnimationClip(AnimationClip clip)
    {
        Debug.Log("変える");

        // ステートをキャッシュ
        //AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[m_animator.layerCount];
        //for (int i = 0; i < m_animator.layerCount; i++)
        //{
        //    layerInfo[i] = m_animator.GetCurrentAnimatorStateInfo(i);
        //}

        // AnimationClipを差し替えて、強制的にアップデート
        // ステートがリセットされる
        m_overrideController[m_overrideClipName] = clip;
        m_animator.Update(0.0f);

        // ステートを戻す
        //for (int i = 0; i < m_animator.layerCount; i++)
        //{
        //    m_animator.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
        //}
    }

    void Test()
    {
        //ZombieNormalTable.BaseLayer.NormalAttack
    }
}
