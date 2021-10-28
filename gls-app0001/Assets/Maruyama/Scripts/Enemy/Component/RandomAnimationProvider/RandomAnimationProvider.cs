using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class RandomAnimationProvider
{
    List<AnimationClip> m_animationClips = new List<AnimationClip>();

    public RandomAnimationProvider(List<AnimationClip> clips)
    {
        this.m_animationClips = clips;
    }

    public AnimationClip GetRandomAnimationClip()
    {
        return MyRandom.RandomList(m_animationClips);
    }
}
