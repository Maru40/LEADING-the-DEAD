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
    /// ���𕷂�����
    /// </summary>
    /// <param name="foundObject">���������̃I�u�W�F�N�g</param>
    public void Listen(FoundObject foundObject)
    {
        m_listen?.Listen(foundObject);
    }

    //�g���K�[�Ŕ͈͂��w�肷��ꍇ�����邩��
    private void OnTriggerEnter(Collider other)
    {
        var foundObject = other.gameObject.GetComponentInParentAndChildren<FoundObject>();
        if (foundObject)
        {
            Listen(foundObject);
        }
    }
}
