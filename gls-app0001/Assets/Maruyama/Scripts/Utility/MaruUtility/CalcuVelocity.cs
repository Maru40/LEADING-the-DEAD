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
		/// 当たった壁に対して反射ベクトルを求める。
		/// </summary>
		/// <param name="direct">現在の進行方向</param>
		/// <param name="other">あたったコリジョン</param>
		/// <returns>反射ベクトル</returns>
		static public Vector3 Reflection(Vector3 moveDirect, Collision other)
        {
			var direct = moveDirect;
			foreach(var contact in other.contacts)
            {
				var newDot = Mathf.Abs(Vector3.Dot(direct, contact.normal));
				direct += 2.0f * (newDot * contact.normal);
			}

			return direct;
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
			if(targetVelocityManager == null)
            {
				return CalucSeekVec(velocity, toVec, maxSpeed);
            }
			var targetObj = targetVelocityManager.gameObject;
			var targetVelocity = targetVelocityManager.velocity;

			//先読み時間は、逃げる側と追いかける側の距離に比例し、エージェントの速度に反比例する。
			var aheadTime = toVec.magnitude / (maxSpeed + targetVelocity.magnitude);
			var desiredPosition = targetObj.transform.position + (targetVelocity * aheadTime * turningPower); //目的のポジション
			var desiredVec = desiredPosition - selfObj.transform.position; //希望のベクトル

			return CalucSeekVec(velocity, desiredVec, maxSpeed);
        }

		/// <summary>
		/// 条件付き予測フォース
		/// </summary>
		/// <param name="velocity">現在の速度</param>
		/// <param name="toVec">行きたい方向</param>
		/// <param name="maxSpeed">最大スピード</param>
		/// <param name="selfObj">自分自身</param>
		/// <param name="targetVelocityManager">速度管理</param>
		/// <param name="relativeHeading">ターゲットとのフォワードの差(dot)</param>
		/// <param name="subPursuitTargetForward">曲がってよい差</param>
		/// <param name="turningPower">曲がる力</param>
		/// <returns></returns>
		static public Vector3 CalcuConditionPursuitForce(Vector3 velocity, Vector3 toVec, float maxSpeed,
			GameObject selfObj, Rigidbody targetVelocityManager,
			float relativeHeading, float subPursuitTargetForward,
			float turningPower = 1.0f)
		{	
			//前方にいて、かつ、自分と相手のフォワードの差が開いていなかったら、通常Seek
			//Dotは差が開いている程、値が小さくなる。
			if (IsFront(selfObj.transform.forward,toVec) && IsMinSubForward(relativeHeading, subPursuitTargetForward))
			{
				return CalucSeekVec(velocity, toVec, maxSpeed);
			}
			else
			{   //フォワードの差が開いていたら、予測Seek
				Debug.Log("◆予測Seek");
				return CalcuPursuitForce(
					velocity, toVec, maxSpeed, selfObj, targetVelocityManager, turningPower);
			}
		}

		/// <summary>
		/// ターゲットが正面にいるかどうか
		/// </summary>
		/// <param name="selfForward">自分自身のフォワード</param>
		/// <param name="toTargetVec">ターゲットの方向</param>
		/// <returns>正面ならtrue</returns>
		static bool IsFront(Vector3 selfForward ,Vector3 toTargetVec)
		{
			return Vector3.Dot(toTargetVec, selfForward) > 0 ? true : false;
		}

		/// <summary>
		/// 自分と相手のフォワードの差が開いていなかったら
		/// </summary>
		/// <param name="relativeHeading">自分と相手のフォワードのdot</param>
		/// <param name="sub">基準となる差</param>
		/// <returns>差が開いていなかったらtrue</returns>
		static bool IsMinSubForward(float relativeHeading, float sub)
		{
			return Mathf.Abs(relativeHeading) > sub ? true : false;
		}

		/// <summary>
		/// 指定した角度より大きくないかどうか
		/// </summary>
		/// <param name="velocity">現在の速度</param>
		/// <param name="toVec">行きたい方向</param>
		/// <param name="turningDegree">曲がる最大角度</param>
		/// <returns>角度差が小さいならtrueを返す</returns>
		static public bool IsTurningVector(Vector3 velocity, Vector3 toVec, float turningDegree)
        {
			var newDot = Vector3.Dot(velocity.normalized, toVec.normalized);
			var rad = Mathf.Acos(newDot);
			var turningRad = turningDegree * Mathf.Deg2Rad;

			return rad < turningRad ? true : false;
        }

		/// <summary>
		/// 角度差を返す。
		/// </summary>
		/// <param name="velocity">現在の速度</param>
		/// <param name="toVec">行きたい方向</param>
		/// <param name="turningDegree">曲がりたい角度</param>
		/// <returns>角度の差</returns>
		static public float CalcuSubDotRad(Vector3 velocity, Vector3 toVec, float turningDegree)
        {
			var newDot = Vector3.Dot(velocity.normalized, toVec.normalized);
			var rad = Mathf.Acos(newDot);
			var turningRad = turningDegree * Mathf.Deg2Rad;

			return rad - turningRad;
		}

		/// <summary>
		/// 角度内のベクターに変換する。
		/// </summary>
		/// <param name="velocity">現在の速度</param>
		/// <param name="toVec">行きたい方向</param>
		/// <param name="turningDegree">曲がれる角度</param>
		/// <param name="axis">dotの基準となるベクトル</param>
		/// <returns></returns>
		static public Vector3 CalcuInTurningVector(Vector3 velocity, Vector3 toVec, float turningDegree, Vector3 axis)
        {
			if(IsTurningVector(velocity, toVec, turningDegree)) {
				//Debug.Log("曲がれる");
				return toVec;
			}

			//回転方向を時計回りか反時計回りによって変える。
			var angle = Vector3.SignedAngle(velocity.normalized, toVec.normalized, axis);  
			var subRad = CalcuSubDotRad(velocity, toVec, turningDegree) * Mathf.Sign(angle); 

			var quat = Quaternion.AngleAxis(subRad, axis);
			//var inverseQuat = Quaternion.Inverse(quat);

			var newVec = quat * toVec;
            //Debug.Log("toVec:  " + toVec);
            //Debug.Log("newVec: " + newVec);

			return newVec;
        }
    }
}


