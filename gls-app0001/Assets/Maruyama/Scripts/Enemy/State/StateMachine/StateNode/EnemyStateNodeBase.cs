using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy用の基底StateNodeクラス
/// </summary>
/// <typeparam name="EnemyType">BaseEnemyの子クラス</typeparam>
public abstract class EnemyStateNodeBase<EnemyType> : NodeBase<EnemyType>
    where EnemyType : class
{
	protected enum EnableChangeType {
		Start,
		Exit,
	}

	//コンポーネントの変更関係の情報をまとめた構造体
	protected struct ChangeCompParam
	{
		public Behaviour behaviour;
		public bool isStart;  //スタート時にどっちにするか？
		public bool isExit;   //終了時にどっちにするか？

		public ChangeCompParam(Behaviour behaviour, bool isStart,bool isExit)
		{
			this.behaviour = behaviour;
			this.isStart = isStart;
			this.isExit = isExit;
		}
	}

	List<ChangeCompParam> m_changeParams;


	public EnemyStateNodeBase(EnemyType enemy)
        :base(enemy)
    {
		m_changeParams = new List<ChangeCompParam>();
	}

	//protected---------------------------------------------------------

	/// <summary>
	/// 開始と終了時に切り替えるコンポーネントの追加
	/// </summary>
	/// <param name="behaviour">切り替えるコンポーネントのポインタ</param>
	/// <param name="isStart">スタート時にどっちに切り替える</param>
	/// <param name="isExit">終了時にどっちに切り替えるか</param>
	protected void AddChangeComp(Behaviour behaviour, bool isStart, bool isExit)
	{
		if (behaviour == null) {  //nullptrなら追加しない
			return;
		}

		var param = new ChangeCompParam(behaviour, isStart, isExit);
		m_changeParams.Add(param);
	}

	/// <summary>
	/// 登録されたコンポーネントの切り替えを行う
	/// </summary>
	/// <param name="type">StartかExitの切替タイプ</param>
	protected void ChangeComps(EnableChangeType type)
	{
		foreach(var param in m_changeParams)
        {
			bool isEnable = type switch
            {
				EnableChangeType.Start => param.isStart,
				EnableChangeType.Exit => param.isExit,
				_ => false
			};

            param.behaviour.enabled = isEnable;
		}
	}
}
