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

	Func<TransitionType,bool> m_isTransitionFunc = null; //遷移する条件

	public EdgeBase(EnumType from, EnumType to,
			Func<TransitionType,bool> isTransitionFunc) 
	{
		m_fromType = from;
		m_toType = to;
		m_isTransitionFunc = isTransitionFunc;
	}


	//アクセッサ----------------------------------------------------------

	/// <summary>
	/// 遷移条件を設定
	/// </summary>
	/// <param name="func">遷移条件のファンクション</param>
	public void SetIsTransitionFunc(Func<TransitionType, bool> func){
		m_isTransitionFunc = func;
    }

	/// <summary>
	/// 遷移できるか判断
	/// </summary>
	/// <param name="member">遷移用のメンバー</param>
	/// <returns>遷移できるならtrue</returns>
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
