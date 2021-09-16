using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TargetMgr : MonoBehaviour
{
    //�Ō�ɎQ�Ƃ��ꂽ�^�[�Q�b�g
    GameObject m_nowTarget = null;

    //�ǂ̃R���|�[�l���g�̃^�[�Q�b�g�����m�F����B
    Dictionary<Type,GameObject> m_targets = new Dictionary<Type, GameObject>();

    private void Start()
    {
        if(m_targets.Count == 0){
            var target = GameObject.Find("Player");
            m_nowTarget = target;
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�̒ǉ�
    /// </summary>
    /// <param name="type">�ǂ̃R���|�[�l���g�̃^�[�Q�b�g��</param>
    /// <param name="target">�^�[�Q�b�g</param>
    public void AddTarget(Type type,GameObject target)
    {
        m_targets[type] = target;
    }

    /// <summary>
    /// ���ݒǏ]����^�[�Q�b�g�̃Z�b�g
    /// </summary>
    /// <param name="type">�R���|�[�l���g�̃^�C�v</param>
    /// <param name="target">�^�[�Q�b�g</param>
    public void SetNowTarget(Type type,GameObject target)
    {
        m_nowTarget = target;
        m_targets[type] = target;
    }

    /// <summary>
    /// �Ō�ɎQ�Ƃ��ꂽ�^�[�Q�b�g���擾
    /// </summary>
    /// <returns>�^�[�Q�b�g</returns>
    public GameObject GetNowTarget()
    {
        return m_nowTarget;
    }

    public GameObject GetNowTarget(Type type)
    {
        //key�����݂��Ȃ�������
        if (!m_targets.ContainsKey(type)){
            return null;
        }

        m_nowTarget = m_targets[type];
        return m_nowTarget;
    }
}
