using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TargetManager : MonoBehaviour
{
    //�Ō�ɎQ�Ƃ��ꂽ�^�[�Q�b�g
    FoundObject m_nowTarget = null;

    //�ǂ̃R���|�[�l���g�̃^�[�Q�b�g�����m�F����B
    Dictionary<Type,FoundObject> m_targets = new Dictionary<Type, FoundObject>();

    private void Start()
    {
        if(m_targets.Count == 0) {
            var target = GameObject.Find("Player");
            m_nowTarget = target.GetComponent<FoundObject>();
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�̒ǉ�
    /// </summary>
    /// <param name="type">�ǂ̃R���|�[�l���g�̃^�[�Q�b�g��</param>
    /// <param name="target">�^�[�Q�b�g</param>
    public void AddTarget(Type type, FoundObject target)
    {
        m_targets[type] = target;
    }

    /// <summary>
    /// ���ݒǏ]����^�[�Q�b�g�̃Z�b�g
    /// </summary>
    /// <param name="type">�R���|�[�l���g�̃^�C�v</param>
    /// <param name="target">�^�[�Q�b�g</param>
    public void SetNowTarget(Type type, FoundObject target)
    {
        m_nowTarget = target;
        m_targets[type] = target;
    }

    /// <summary>
    /// �Ō�ɎQ�Ƃ��ꂽ�^�[�Q�b�g���擾
    /// </summary>
    /// <returns>�^�[�Q�b�g</returns>
    public FoundObject GetNowTarget()
    {
        return m_nowTarget;
    }

    public FoundObject.FoundData? GetNowTargetFoundData()
    {
        return m_nowTarget?.GetFoundData();
    }

    public FoundObject GetNowTarget(Type type)
    {
        //key�����݂��Ȃ�������
        if (!m_targets.ContainsKey(type)){
            return null;
        }

        m_nowTarget = m_targets[type];
        return m_nowTarget;
    }

    public FoundObject.FoundData? GetNowTargetFoundData(Type type)
    {
        //key�����݂��Ȃ�������
        if (!m_targets.ContainsKey(type))
        {
            return null;
        }

        m_nowTarget = m_targets[type];
        return m_nowTarget.GetFoundData();
    }

    /// <summary>
    /// ���݂̃^�[�Q�b�g�����̃x�N�g����Ԃ�
    /// </summary>
    /// <returns>�^�[�Q�b�g�����̃x�N�g��</returns>
    public Vector3 GetToNowTargetVector()
    {
        return m_nowTarget.transform.position - transform.position;
    }
}
