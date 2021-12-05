using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleAnimation))]

public class FadeAnimationObject : FadeObject
{
    [SerializeField]
    private SimpleAnimation m_animation;

    private bool m_isStarted = false;

    private void Reset()
    {
        m_animation = GetComponent<SimpleAnimation>();
    }

    public override void FadeStart()
    {
        m_animation.Play();
        m_isStarted = true;
    }

    void Update()
    {
        Debug.Log(m_animation.isPlaying);
    }

    public override bool IsFinish()
    {
        return m_isStarted && !m_animation.isPlaying;
    }
}
