using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy�p�̊��StateNode�N���X
/// </summary>
/// <typeparam name="EnemyType">BaseEnemy�̎q�N���X</typeparam>
public abstract class EnemyStateNodeBase<EnemyType> : NodeBase<EnemyType>
    where EnemyType : class
{
	protected enum EnableChangeType {
		Start,
		Exit,
	}

	//�R���|�[�l���g�̕ύX�֌W�̏����܂Ƃ߂��\����
	protected struct ChangeCompParam
	{
		public Behaviour behaviour;
		public bool isStart;  //�X�^�[�g���ɂǂ����ɂ��邩�H
		public bool isExit;   //�I�����ɂǂ����ɂ��邩�H

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
	/// �J�n�ƏI�����ɐ؂�ւ���R���|�[�l���g�̒ǉ�
	/// </summary>
	/// <param name="behaviour">�؂�ւ���R���|�[�l���g�̃|�C���^</param>
	/// <param name="isStart">�X�^�[�g���ɂǂ����ɐ؂�ւ���</param>
	/// <param name="isExit">�I�����ɂǂ����ɐ؂�ւ��邩</param>
	protected void AddChangeComp(Behaviour behaviour, bool isStart, bool isExit)
	{
		if (behaviour == null) {  //nullptr�Ȃ�ǉ����Ȃ�
			return;
		}

		var param = new ChangeCompParam(behaviour, isStart, isExit);
		m_changeParams.Add(param);
	}

	/// <summary>
	/// �o�^���ꂽ�R���|�[�l���g�̐؂�ւ����s��
	/// </summary>
	/// <param name="type">Start��Exit�̐ؑփ^�C�v</param>
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
