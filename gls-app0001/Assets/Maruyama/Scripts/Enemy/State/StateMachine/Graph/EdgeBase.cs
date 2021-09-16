using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EdgeBase<EnumType, TransitionType>
	where EnumType : Enum
	where TransitionType : class
{
	EnumType m_fromType;
    EnumType m_toType;

	Func<TransitionType,bool> m_isTransitionFunc = null; //�J�ڂ������

	public EdgeBase(EnumType from, EnumType to,
			Func<TransitionType,bool> isTransitionFunc) 
	{
		m_fromType = from;
		m_toType = to;
		m_isTransitionFunc = isTransitionFunc;
	}


	//�A�N�Z�b�T----------------------------------------------------------

	/// <summary>
	/// �J�ڏ�����ݒ�
	/// </summary>
	/// <param name="func">�J�ڏ����̃t�@���N�V����</param>
	public void SetIsTransitionFunc(Func<TransitionType, bool> func){
		m_isTransitionFunc = func;
    }

	/// <summary>
	/// �J�ڂł��邩���f
	/// </summary>
	/// <param name="member">�J�ڗp�̃����o�[</param>
	/// <returns>�J�ڂł���Ȃ�true</returns>
	public bool IsTransition(TransitionType member) {
		return m_isTransitionFunc(member);
	}

	public EnumType GetFromType(){
		return m_fromType;
	}

	public EnumType GetToType() {
		return m_toType;
	}

}
