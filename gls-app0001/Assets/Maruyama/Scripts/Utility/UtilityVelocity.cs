using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityVelocity
{

	/// <summary>
	/// �ő呬�x����
	/// </summary>
	/// <param name="velocity">�����������x�N�g��</param>
	/// <param name="maxSpeed">�������x</param>
	/// <returns>�������ꂽ�͈͂̃x�N�g����Ԃ��B</returns>
	static public Vector3 MaxSpeedVecCheck(Vector3 velocity, float maxSpeed) {
		var speed = velocity.magnitude;

		speed = Mathf.Min(speed, maxSpeed);
		return velocity.normalized * speed;
	}

	/// <summary>
	/// �����I�ɒǂ������邽�߂̃x�N�g�����v�Z���ĕԂ��֐�
	/// </summary>
	/// <param name="velocity">���݂̑��x</param>
	/// <param name="toVec">�^�[�Q�b�g�����̃x�N�g��</param>
	/// <param name="maxSpeed">�ő呬�x</param>
	/// <returns>�u�^�[�Q�b�g�̕����̃x�N�g���v- �u���݂̑��x�v</returns>
	static public Vector3 CalucSeekVec(Vector3 velocity, Vector3 toVec, float maxSpeed) {
		Vector3 desiredVelocity = toVec.normalized * maxSpeed;  //��]�̃x�N�g��
		return (desiredVelocity - velocity);
	}

	/// <summary>
	/// �����x�N�g����Ԃ�(�߂Â��Ə������Ȃ�x�N�g��)
	/// </summary>
	/// <param name="velocity">���݂̑��x</param>
	/// <param name="toVec">�^�[�Q�b�g�����̃x�N�g��</param>
	/// <param name="maxSpeed">�ő呬�x</param>
	/// <param name="decl"></param>
	/// <returns>�����x�N�g����Ԃ�(�߂Â��Ə������Ȃ�x�N�g��)��Ԃ�</returns>
	static public Vector3 CalucArriveVec(Vector3 velocity, Vector3 toVec, float maxSpeed, float decl = 3.0f) {
		float dist = toVec.magnitude;
		if (dist > 0) {
			const float DecelerationTweaker = 0.3f;  //�����l

			//�w�肳�ꂽ�����ŖڕW�ɓ��B���鎮
			float speed = dist / (decl * DecelerationTweaker);
			speed = Mathf.Min(speed, maxSpeed);
			Vector3 desiredVelocity = toVec * speed / dist; //��]�̃x�N�g��
			Vector3 steerVec = desiredVelocity - velocity;  //�X�e�A�����O�x�N�g��

			return steerVec;
		}

		return new Vector3(0, 0, 0);
	}

	/// <summary>
	/// �߂��ɂ���Ƃ���Arrive��,�����ɂ���Ƃ���Seek�Œǂ�������֐�
	/// </summary>
	/// <param name="velocity">���݂̑��x</param>
	/// <param name="toVec">�^�[�Q�b�g�����̃x�N�g��</param>
	/// <param name="maxSpeed">�ő呬�x</param>
	/// <param name="nearRange">�v�Z��؂�ւ��鋗��</param>
	/// <param name="decl"></param>
	/// <returns>�v�Z���ꂽ�x�N�g��</returns>
	static public Vector3 CalucNearArriveFarSeek(Vector3 velocity, Vector3 toVec,
		float maxSpeed, float nearRange, float decl = 3.0f)
	{
		float range = toVec.magnitude;
		if (range <= nearRange) {  //�߂��ɂ�����
			return CalucArriveVec(velocity, toVec, maxSpeed, decl);
		}
		else {  //�����ɂ�����
			return CalucSeekVec(velocity, toVec, maxSpeed);
		}
	}

}
