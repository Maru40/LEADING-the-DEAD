using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility 
{
	public class CalcuVelocity
	{

		/// <summary>
		/// 最大速度制限
		/// </summary>
		/// <param name="velocity">制限したいベクトル</param>
		/// <param name="maxSpeed">制限速度</param>
		/// <returns>制限された範囲のベクトルを返す。</returns>
		static public Vector3 MaxSpeedVecCheck(Vector3 velocity, float maxSpeed)
		{
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
		/// <param name="maxTurningDegree">最大旋回角度</param>
		/// <returns>「ターゲットの方向のベクトル」- 「現在の速度」</returns>
		static public Vector3 CalucSeekVec(Vector3 velocity, Vector3 toVec, float maxSpeed)
		{
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
		static public Vector3 CalucArriveVec(Vector3 velocity, Vector3 toVec, float maxSpeed, float decl = 3.0f)
		{
			float dist = toVec.magnitude;
			if (dist > 0)
			{
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
			if (range <= nearRange)
			{  //近くにいたら
				return CalucArriveVec(velocity, toVec, maxSpeed, decl);
			}
			else
			{  //遠くにいたら
				return CalucSeekVec(velocity, toVec, maxSpeed);
			}
		}

		/// <summary>
		/// 敵の動きを先読みして、方向を決めるベクトルを返す
		/// </summary>
		/// <param name="velocity">現在の速度</param>
		/// <param name="toVec">行きたい方向</param>
		/// <param name="maxSpeed">最大speed</param>
		/// <param name="targetVelocityManager">ターゲットのvelocity管理</param>
		/// <returns>先読みしたベクトル</returns>
		static public Vector3 CalcuPursuitForce(Vector3 velocity, Vector3 toVec, float maxSpeed, 
			GameObject selfObj, Rigidbody targetVelocityManager, float turningPower = 1.0f)
        {
			var targetObj = targetVelocityManager.gameObject;
			var targetVelocity = targetVelocityManager.velocity;

			//先読み時間は、逃げる側と追いかける側の距離に比例し、エージェントの速度に反比例する。
			var aheadTime = toVec.magnitude / (maxSpeed + targetVelocity.magnitude);
			var desiredPosition = targetObj.transform.position + (targetVelocity * aheadTime * turningPower); //目的のポジション
			var desiredVec = desiredPosition - selfObj.transform.position; //希望のベクトル

			return CalucSeekVec(velocity, desiredVec, maxSpeed);
        }

		//指定した角度より大きくないかどうか
		static public bool IsTurningVector(Vector3 velocity, Vector3 toVec, float turningDegree)
        {
			var newDot = Vector3.Dot(velocity.normalized, toVec.normalized);
			var rad = Mathf.Acos(newDot);
			var turningRad = turningDegree * Mathf.Deg2Rad;

			return rad < turningRad ? true : false;
        }

		//角度差を返す。
		static public float CalcuSubDotRad(Vector3 velocity, Vector3 toVec, float turningDegree)
        {
			var newDot = Vector3.Dot(velocity.normalized, toVec.normalized);
			var rad = Mathf.Acos(newDot);
			var turningRad = turningDegree * Mathf.Deg2Rad;

			return rad - turningRad;
		}

		//角度内のベクターに変換する。
		static public Vector3 CalcuInTurningVector(Vector3 velocity, Vector3 toVec, float turningDegree, Vector3 axis)
        {
			if(IsTurningVector(velocity, toVec, turningDegree)) {
				Debug.Log("曲がれる");
				return toVec;
			}

			//回転方向を時計回りか反時計回りによって変える。
			var angle = Vector3.SignedAngle(velocity.normalized, toVec.normalized, axis);  
			var subRad = CalcuSubDotRad(velocity, toVec, turningDegree) * Mathf.Sign(angle); 

			var quat = Quaternion.AngleAxis(subRad, axis);
			//var inverseQuat = Quaternion.Inverse(quat);

			var newVec = quat * toVec;
            Debug.Log("toVec:  " + toVec);
            Debug.Log("newVec: " + newVec);

			return newVec;
        }
    }
}


