using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopUpUI : MonoBehaviour
{
    [SerializeField]
    public GameObject firstSelectObject;

    [SerializeField]
    private PlayableDirector m_director;

    public void Awake()
    {
        if(firstSelectObject == null)
        {
            firstSelectObject = gameObject;
        }

        if (m_director)
        {
            m_director.stopped += _ => GameFocusManager.PushFocus(firstSelectObject);
        }
    }

    public void PopUp()
    {
        gameObject.SetActive(true);

        if (!m_director)
        {
            GameFocusManager.PushFocus(firstSelectObject);
            return;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameFocusManager.PopFocus();
    }
}
