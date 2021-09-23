using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

class WaitTimerParam
{
    public float time = 1.0f;
    public float timeElapsed = 0;
    public float countSpeed = 1.0f;
    public Action endAction = null;
    public bool isEnd = false;

    public WaitTimerParam(float time, float countSpeed, Action endAction)
    {
        this.time = time;
        this.countSpeed = countSpeed;
        this.endAction = endAction;
    }

    /// <summary>
    /// �^�C���I�����ɂ��鏈��
    /// </summary>
    /// <param name="isEndAction">�I���֐����Ăяo�����ǂ���</param>
    public void EndTimer(bool isEndAction = true)
    {
        timeElapsed = time;
        isEnd = true;

        if (isEndAction){
            endAction?.Invoke();
        }
        endAction = null;
    }
}

public class WaitTimer : MonoBehaviour
{
    Dictionary<Type,WaitTimerParam> m_params = new Dictionary<Type, WaitTimerParam>();

    void Start()
    {
        m_params = new Dictionary<Type,WaitTimerParam>();
    }
    
    void Update()
    {
        //�p�����[�^��Update
        foreach (var keyValuePair in m_params)
        {
            var param = keyValuePair.Value;

            if (param.isEnd){  //�I�����Ă�����������ɓ����B
                continue;
            }

            param.timeElapsed += param.countSpeed * Time.deltaTime;

            if(param.timeElapsed > param.time)
            {
                param.EndTimer();
            }
        }
    }

    /// <summary>
    /// �^�C�}�[�̐ݒu
    /// </summary>
    /// <param name="type">�ǉ������N���X�̃^�C�v</param>
    /// <param name="time">�҂���</param>
    /// <param name="endAction">�I�����ɌĂԃA�N�V����</param>
    /// <param name="countSpeed">�J�E���g�X�s�[�h</param>
    public void AddWaitTimer(Type type ,float time, Action endAction = null, float countSpeed = 1.0f)
    {
        var newParam = new WaitTimerParam(time, countSpeed, endAction);
        m_params[type] = newParam;
    }

    /// <summary>
    /// �ҋ@��Ԃ��ǂ���
    /// </summary>
    /// <param name="type">�I�u�W�F�N�g�̃^�C�v</param>
    /// <returns>�ҋ@��ԂȂ�true</returns>
    public bool IsWait(Type type)
    {
        //�L�[�����݂���Ȃ�
        if (m_params.ContainsKey(type)) {
            return !m_params[type].isEnd;  //�I����ԂłȂ��Ȃ�true(�ҋ@���)
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// �����I�ɑҋ@��Ԃ�����
    /// </summary>
    /// <param name="type">�I�u�W�F�N�g�^�C�v</param>
    /// <param name="isEndAction">�I�����ɌĂяo���֐����ĂԂ��ǂ���</param>
    public void AbsoluteEndTimer(Type type, bool isEndAction)
    {        
        //�L�[�����݂���Ȃ�
        if (m_params.ContainsKey(type))
        {
            m_params[type].EndTimer(isEndAction);  //�ҋ@��ԋ����I��
        }
    }
}
