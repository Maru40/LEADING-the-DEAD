using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UniRx;

public class PopUpUI : MonoBehaviour
{
    public enum DirectorType
    {
        None,
        Timeline,
        SimpleStartEndAnimator,
    }

    [SerializeField]
    public GameObject firstSelectObject;

    [SerializeField]
    private DirectorType m_directorType = DirectorType.None;

    [SerializeField]
    private PlayableDirector m_director;

    [SerializeField]
    private SimpleStartEndAnimateDirector m_animateDirector;

    public void Awake()
    {
        if(firstSelectObject == null)
        {
            firstSelectObject = gameObject;
        }

        if(m_directorType == DirectorType.None)
        {
            return;
        }

        if (m_directorType == DirectorType.Timeline && m_director)
        {
            m_director.stopped += _ => GameFocusManager.PushFocus(firstSelectObject);
            return;
        }

        if(m_directorType == DirectorType.SimpleStartEndAnimator && m_animateDirector)
        {
            m_animateDirector.finished
                .Where(animationType => animationType == SimpleStartEndAnimateDirector.AnimationType.StartAnimation)
                .Subscribe(_ => GameFocusManager.PushFocus(firstSelectObject))
                .AddTo(this);

            m_animateDirector.finished
                .Where(animationType => animationType == SimpleStartEndAnimateDirector.AnimationType.EndAnimation)
                .Subscribe(_ =>
                {
                    gameObject.SetActive(false);
                    GameFocusManager.PopFocus();
                })
                .AddTo(this);

            return;
        }
    }

    public void PopUp()
    {
        gameObject.SetActive(true);

        if (m_directorType == DirectorType.None)
        {
            GameFocusManager.PushFocus(firstSelectObject);
            return;
        }

        if(m_directorType == DirectorType.SimpleStartEndAnimator)
        {
            m_animateDirector.StartAnimationPlay();
        }
    }

    public void Close()
    {

        if (m_directorType == DirectorType.None)
        {
            gameObject.SetActive(false);
            GameFocusManager.PopFocus();
            return;
        }

        if (m_directorType == DirectorType.SimpleStartEndAnimator)
        {
            EventSystem.current.SetSelectedGameObject(null);
            m_animateDirector.EndAnimationPlay();
        }
    }
}
