using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SimpleStartEndAnimateDirector : MonoBehaviour
{
    public enum AnimationType
    {
        StartAnimation,
        EndAnimation
    }


    [SerializeField]
    private SimpleAnimation m_simpleAnimation;

    [SerializeField]
    private AnimationClip m_startAnimateClip;
    [SerializeField]
    private AnimationClip m_endAnimateClip;

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    private readonly Subject<AnimationType> m_finishedSubject = new Subject<AnimationType>();

    public System.IObservable<AnimationType> finished => m_finishedSubject;

    private const string ANIMATE_START_ANIMATION_CLIP = "ANIMATE_START_ANIMATION_CLIP";
    private const string ANIMATE_END_ANIMATION_CLIP = "ANIMATE_END_ANIMATION_CLIP";

    private AnimationType m_playingAnimationType;
    private string m_playingClipName = "";

    private bool m_isPlaying = false;

    private bool m_isInit = false;

    private void Init()
    {
        if(m_isInit)
        {
            return;
        }

        m_simpleAnimation.AddClip(m_startAnimateClip, ANIMATE_START_ANIMATION_CLIP);
        m_simpleAnimation.AddClip(m_endAnimateClip, ANIMATE_END_ANIMATION_CLIP);

        m_isInit = true;
    }

    private void Start()
    {
        Init();
    }

    public void AnimationPlay(AnimationType animationType)
    {
        string animationName = animationType switch
        {
            AnimationType.StartAnimation => ANIMATE_START_ANIMATION_CLIP,
            AnimationType.EndAnimation   => ANIMATE_END_ANIMATION_CLIP,
            _                            => ""
        };

        Animate(animationName, animationType);
    }

    public void StartAnimationPlay()
    {
        Animate(ANIMATE_START_ANIMATION_CLIP, AnimationType.StartAnimation);
    }

    public void EndAnimationPlay()
    {
        Animate(ANIMATE_END_ANIMATION_CLIP, AnimationType.EndAnimation);
    }

    private void Animate(string animateClipName, AnimationType animationType)
    {
        Init();

        m_isPlaying = true;
        m_playingClipName = animateClipName;
        m_playingAnimationType = animationType;
        m_canvasGroup.alpha = animationType == AnimationType.StartAnimation ? 0.0f : 1.0f;
        m_simpleAnimation.Play(animateClipName);
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isPlaying)
        {
            return;
        }

        float normalisedTime = m_simpleAnimation.GetState(m_playingClipName).normalizedTime;

        if (normalisedTime >= 1.0f)
        {
            m_isPlaying = false;
            m_playingClipName = "";
            m_canvasGroup.alpha = m_playingAnimationType == AnimationType.StartAnimation ? 1.0f : 0.0f;
            m_finishedSubject.OnNext(m_playingAnimationType);
        }
        else
        {
            float alpha = m_playingAnimationType switch
            {
                AnimationType.StartAnimation => normalisedTime,
                AnimationType.EndAnimation   => 1.0f - normalisedTime,
                _                            => 1.0f
            };

            m_canvasGroup.alpha = alpha;
        }
        }
    }
