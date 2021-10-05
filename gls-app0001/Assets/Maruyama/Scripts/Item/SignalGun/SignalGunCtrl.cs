using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalGunCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject m_bomb = null;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Use();
    }

    public void Use()
    {
        //信号弾の生成
        Instantiate(m_bomb, transform.position, Quaternion.identity);
    }
}
