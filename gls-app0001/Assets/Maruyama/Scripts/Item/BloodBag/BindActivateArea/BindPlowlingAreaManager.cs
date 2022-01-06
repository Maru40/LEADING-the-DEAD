using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 徘徊エリアに制限を掛ける。
/// </summary>
public class BindPlowlingAreaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_centerObject = null;

    private FoundObject m_foudObject;
    private SphereCollider m_collider;

    private void Awake()
    {
        m_foudObject = GetComponent<FoundObject>();
        m_collider = GetComponent<SphereCollider>();
    }

    //アクセッサ・プロパティ----------------------------------------------------

    public FoundObject GetFoundOjbect()
    {
        return m_foudObject;
    }

    public float BindRange => m_collider.radius;

    public GameObject CenterObject
    {
        get
        {
            if(m_centerObject == null) {
                return gameObject;
            }

            return m_centerObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var iBind = other.GetComponent<I_BindPlowlingArea>();
        if(iBind != null)
        {
            iBind.InBind(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var iBind = other.GetComponent<I_BindPlowlingArea>();
        if (iBind != null)
        {
            iBind.OutBind(this);
        }
    }
}
