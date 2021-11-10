using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


/// <summary>
/// ターゲットの管理用のパラメータ
/// </summary>
[Serializable]
class EyeTargetParam
{
    public GameObject target;
    public bool isFind;  //見つけた状態かどうかを返す

    public EyeTargetParam(GameObject target)
    {
        this.target = target;
        this.isFind = false;
    }
}

/// <summary>
/// 視界のパラメータの構造体
/// </summary>
[Serializable]
public class EyeSearchRangeParam
{
    public float range;  //索敵範囲(同心円状)
    public float height; //索敵範囲(高さ)
    public float rad;    //索敵範囲(角度)

    public EyeSearchRangeParam()
        :this(20.0f ,3.0f ,30.0f)
	{}

	public EyeSearchRangeParam(float range, float height, float deg)
    {
        this.range = range;
        this.height = height;
        this.rad = deg * Mathf.Deg2Rad;
    }
}


/// <summary>
/// 視界の管理
/// </summary>
public class EyeSearchRange : MonoBehaviour
{
    //範囲に入っているかの対象になるオブジェクト
    [SerializeField]
    List<EyeTargetParam> m_targetParams = new List<EyeTargetParam>();

    [SerializeField]
    GameObject m_centerObject = null;

    [SerializeField]
    EyeSearchRangeParam m_param = new EyeSearchRangeParam();

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    private void Start()
    {
        //NullCheck
        if(m_centerObject == null)
        {
            m_centerObject = transform.gameObject;
        }
    }

    private void Update()
    {
        foreach(var param in m_targetParams)
        {
            if (IsInEyeRange(param.target))
            {  //ターゲットが視界に入っていたら。
                Hit(param);
            }
            else
            {
                param.isFind = false;
            }
        }
    }

    bool IsRange(GameObject target) {
        return IsRange(target.transform.position);
	}

    bool IsRange(Vector3 targetPosition) {
        var toVec = targetPosition - transform.position;
        //長さチェック
        return toVec.magnitude <= m_param.range ? true : false;
    }

    bool IsHeight(GameObject target) {
        return IsHeight(target.transform.position);
	}

    bool IsHeight(Vector3 targetPosition)
    {
        var selfPos = transform.position;

        var subHeight = targetPosition.y - selfPos.y;  //高さの差を求める。
        //高さが小さかったら。
        return Mathf.Abs(subHeight) <= m_param.height ? true : false;
    }

    bool IsRad(GameObject target) {
        return IsRad(target.transform.position);
	}

    bool IsRad(Vector3 targetPosition)
    {
        var forward = transform.transform.forward;
        forward.y = 0.0f;
        var toVec = targetPosition - transform.position;
        toVec.y = 0.0f;

        var newDot = Vector3.Dot(forward.normalized, toVec.normalized);
        var newRad = Mathf.Acos(newDot);
        //索敵範囲に入っていたら。
        return newRad <= m_param.rad ? true : false;
    }

    bool IsRay(GameObject target){
        return IsRay(target.transform.position);
	}

    bool IsRay(Vector3 targetPosition)
    {
        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        var toVec = targetPosition - transform.position;

        const float SphereRange = 0.0f;
        var colliders = Physics.OverlapSphere(transform.position, SphereRange, obstacleLayer);  //オブジェクトに接触時に透けるバグ解消用

        if (!Physics.Linecast(m_centerObject.transform.position, targetPosition, obstacleLayer) && colliders.Length == 0)
        {
            return true;
        }
        //if (!Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer) && colliders.Length == 0)
        //{
        //    return true;
        //}

        return false;

        //return !Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer) ? true : false;
    }

    void Hit(EyeTargetParam targetParam) {
		targetParam.isFind = true;
	}

    /// <summary>
    /// 視界内にいるならtrueを返す
    /// </summary>
    /// <param name="targetPosition">ターゲットのポジション</param>
    /// <returns>視界の中にいるならtrue</returns>
    public bool IsInEyeRange(Vector3 targetPosition)
    {
        //全ての条件がtrueなら視界内に対象がいる。
        if (IsRange(targetPosition) && IsHeight(targetPosition) && IsRad(targetPosition) && IsRay(targetPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 視界内にいるならtrueを返す
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <returns>視界の中にいるならtrue</returns>
    public bool IsInEyeRange(GameObject target)
    {
        if (target == null) { 
            return false; 
        }

        //全ての条件がtrueなら視界内に対象がいる。
        if (IsRange(target) && IsHeight(target) && IsRad(target) && IsRay(target)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// サーチ範囲を一時的に指定して、ターゲットの中に入っているかを判断
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>サーチ範囲ならtrue</returns>
    public bool IsInEyeRange(GameObject target, float range)
    {
        //float beforeRange = m_param.range;
        //m_param.range = range;

        //bool isRange = IsInEyeRange(target);
        //m_param.range = beforeRange;

        return IsInEyeRange(target.transform.position, range);
    }

    /// <summary>
    /// サーチ範囲を一時的に指定して、ターゲットの中に入っているかを判断
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>サーチ範囲ならtrue</returns>
    public bool IsInEyeRange(Vector3 targetPosition, float range)
    {
        float beforeRange = m_param.range;
        m_param.range = range;

        bool isRange = IsInEyeRange(targetPosition);
        m_param.range = beforeRange;

        return isRange;
    }

    /// <summary>
    /// サーチ範囲を一時的に指定して、ターゲットの中に入っているかを判断
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>サーチ範囲ならtrue</returns>
    public bool IsInEyeRange(GameObject target, EyeSearchRangeParam param)
    {
        var beforeParam = m_param;
        m_param = param;

        bool isRange = IsInEyeRange(target);
        m_param = beforeParam;

        return isRange;
    }

    //アクセッサ-------------------------------------------------------------------------

    public void AddTarget(GameObject target) {
		m_targetParams.Add(new EyeTargetParam(target));
	}

    public void SetParam(EyeSearchRangeParam param) {
		m_param = param;
	}

     public EyeSearchRangeParam GetParam() {
		return m_param;
	 }

}
