using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenSound : MonoBehaviour
{
    I_Listen m_listen;

    void Start()
    {
        m_listen = GetComponent<I_Listen>();
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

    //トリガーで範囲を指定する場合があるから
    private void OnTriggerEnter(Collider other)
    {
        var foundObject = other.gameObject.GetComponentInParentAndChildren<FoundObject>();
        if (foundObject)
        {
            Listen(foundObject);
        }
    }
}
