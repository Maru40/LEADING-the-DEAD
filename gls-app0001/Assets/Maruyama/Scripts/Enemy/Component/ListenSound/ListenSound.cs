using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenSound : MonoBehaviour
{
    I_Listen m_listen;
    TargetManager m_targetManager;

    void Awake()
    {
        m_listen = GetComponent<I_Listen>();
        m_targetManager = GetComponent<TargetManager>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 音を聞く処理
    /// </summary>
    /// <param name="foundObject">聞いた音のオブジェクト</param>
    public void Listen(FoundObject foundObject)
    {
        m_listen?.Listen(foundObject);
    }

    void Lost(FoundObject other)
    {
        var target = m_targetManager.GetNowTarget();
        if (target)
        {
            if(other == target)
            {
                m_targetManager.SetNowTarget(GetType(), null);
            }
        }
    }

    //トリガーで範囲を指定する場合があるから
    private void OnTriggerEnter(Collider other)
    {
        var foundObject = other.gameObject.GetComponentInParentAndChildren<FoundObject>();
        if (foundObject)
        {
            if(foundObject.GetFoundData().type == FoundObject.FoundType.SoundObject)
            {
                Listen(foundObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var foundObject = other.gameObject.GetComponentInParentAndChildren<FoundObject>();
        if (foundObject)
        {
            if (foundObject.GetFoundData().type == FoundObject.FoundType.SoundObject)
            {
                Lost(foundObject);
            }
        }
    }
}
