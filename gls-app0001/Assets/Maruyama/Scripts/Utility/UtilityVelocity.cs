using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityVelocity
{

	/// <summary>
	/// 最大速度制限
	/// </summary>
	/// <param name="velocity">制限したいベクトル</param>
	/// <param name="maxSpeed">制限速度</param>
	/// <returns>制限された範囲のベクトルを返す。</returns>
	static public Vector3 MaxSpeedVecCheck(Vector3 velocity, float maxSpeed) {
		var speed = velocity.magnitude;

		speed = Mathf.Min(speed, maxSpeed);
		return velocity.normalized * speed;
	}

	/// <summary>
	/// 直線的に追いかけるためのベクトルを計算して返す関数
	/// </summary>
	/// <param name="velocity">現在の速度</param>
	/// <param name="toVec">ターゲット方向のベクトル</param>
	/// <param name="maxSpeed">最大速度</param>
	/// <returns>「ターゲットの方向のベクトル」- 「現在の速度」</returns>
	static public Vector3 CalucSeekVec(Vector3 velocity, Vector3 toVec, float maxSpeed) {
		Vector3 desiredVelocity = toVec.normalized * maxSpeed;  //希望のベクトル
		return (desiredVelocity - velocity);
	}

	/// <summary>
	/// 到着ベクトルを返す(近づくと小さくなるベクトル)
	/// </summary>
	/// <param name="velocity">現在の速度</param>
	/// <param name="toVec">ターゲット方向のベクトル</param>
	/// <param name="maxSpeed">最大速度</param>
	/// <param name="decl"></param>
	/// <returns>到着ベクトルを返す(近づくと小さくなるベクトル)を返す</returns>
	static public Vector3 CalucArriveVec(Vector3 velocity, Vector3 toVec, float maxSpeed, float decl = 3.0f) {
		float dist = toVec.magnitude;
		if (dist > 0) {
			const float DecelerationTweaker = 0.3f;  //減速値

			//指定された減速で目標に到達する式
			float speed = dist / (decl * DecelerationTweaker);
			speed = Mathf.Min(speed, maxSpeed);
			Vector3 desiredVelocity = toVec * speed / dist; //希望のベクトル
			Vector3 steerVec = desiredVelocity - velocity;  //ステアリングベクトル

			return steerVec;
		}

		return new Vector3(0, 0, 0);
	}

	/// <summary>
	/// 近くにいるときはArriveで,遠くにいるときはSeekで追いかける関数
	/// </summary>
	/// <param name="velocity">現在の速度</param>
	/// <param name="toVec">ターゲット方向のベクトル</param>
	/// <param name="maxSpeed">最大速度</param>
	/// <param name="nearRange">計算を切り替える距離</param>
	/// <param name="decl"></param>
	/// <returns>計算されたベクトル</returns>
	static public Vector3 CalucNearArriveFarSeek(Vector3 velocity, Vector3 toVec,
		float maxSpeed, float nearRange, float decl = 3.0f)
	{
		float range = toVec.magnitude;
		if (range <= nearRange) {  //近くにいたら
			return CalucArriveVec(velocity, toVec, maxSpeed, decl);
		}
		else {  //遠くにいたら
			return CalucSeekVec(velocity, toVec, maxSpeed);
		}
	}

}
