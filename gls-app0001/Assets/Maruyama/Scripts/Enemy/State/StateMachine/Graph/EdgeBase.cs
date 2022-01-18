using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EdgeBase<EnumType, TransitionType>
	where EnumType : Enum
	where TransitionType : struct
{
	private EnumType m_fromType;
	private EnumType m_toType;

	public delegate bool IsTransitionFunc(ref TransitionType member);

	private IsTransitionFunc m_isTransitionFunc = null; //遷移する条件
	private int m_priority = 0;  //優先度

	public EdgeBase(EnumType from, EnumType to,
			IsTransitionFunc isTransitionFunc,
			int priority = 0) 
	{
		m_fromType = from;
		m_toType = to;
		m_isTransitionFunc = isTransitionFunc;
		m_priority = priority;
	}

	//アクセッサ----------------------------------------------------------

	/// <summary>
	/// 遷移条件を設定
	/// </summary>
	/// <param name="func">遷移条件のファンクション</param>
	public void SetIsTransitionFunc(IsTransitionFunc func){
		m_isTransitionFunc = func;
    }

	/// <summary>
	/// 遷移できるか判断
	/// </summary>
	/// <param name="member">遷移用のメンバー</param>
	/// <returns>遷移できるならtrue</returns>
	public bool IsTransition(ref TransitionType member) {
		return m_isTransitionFunc.Invoke(ref member);
	}

	public EnumType GetFromType(){
		return m_fromType;
	}

	public EnumType GetToType() {
		return m_toType;
	}

	/// <summary>
	/// 優先度
	/// </summary>
	public int Priority
	{
		get => m_priority;
		set => m_priority = value;
    }
}
