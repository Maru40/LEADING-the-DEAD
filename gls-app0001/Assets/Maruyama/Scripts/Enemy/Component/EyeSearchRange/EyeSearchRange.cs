using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 視界の管理
/// </summary>
public class EyeSearchRange : MonoBehaviour
{
    [SerializeField]
    GameObject m_target;

    [SerializeField]
    float m_range = 5.0f;

    void Start()
    {
        if(m_target == null)
        {
            m_target = GameObject.Find("Player");
        }
    }

    void Update()
    {
        if (IsFind())
        {
            ChangeState();  //ステートを追従状態にする。
        }
    }

    void ChangeState()
    {

    }

    bool IsFind()
    {
        var toVec = m_target.transform.localPosition - transform.localPosition;

        return toVec.magnitude <= m_range ? true : false;
    }
}
