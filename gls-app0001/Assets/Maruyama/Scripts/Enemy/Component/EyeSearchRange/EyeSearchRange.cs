using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���E�̊Ǘ�
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
            ChangeState();  //�X�e�[�g��Ǐ]��Ԃɂ���B
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
