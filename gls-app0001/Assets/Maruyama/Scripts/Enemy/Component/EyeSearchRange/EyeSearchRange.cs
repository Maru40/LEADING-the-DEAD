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
    public float lenght; //索敵範囲(同心円状)
    public float height; //索敵範囲(高さ)
    public float rad;    //索敵範囲(角度)

    public EyeSearchRangeParam()
        :this(20.0f ,3.0f ,30.0f)
	{}

	public EyeSearchRangeParam(float lenght, float height, float deg)
    {
        this.lenght = lenght;
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
    EyeSearchRangeParam m_param = new EyeSearchRangeParam();

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
        var toVec = target.transform.position - transform.position;
		//長さチェック
		return toVec.magnitude <= m_param.lenght? true : false;
	}

    bool IsHeight(GameObject target) {
		var selfPos = transform.position;
        var targetPos = target.transform.position;

        var subHeight = targetPos.y - selfPos.y;  //高さの差を求める。
		//高さが小さかったら。
		return Mathf.Abs(subHeight) <= m_param.height? true : false;
	}

    bool IsRad(GameObject target) {
		var forward = transform.transform.forward;
        forward.y = 0.0f;
        var toVec = target.transform.position - transform.position;
        toVec.y = 0.0f;

		var newDot = Vector3.Dot(forward.normalized, toVec.normalized);
        var newRad = Mathf.Acos(newDot);
		//索敵範囲に入っていたら。
		return newRad <= m_param.rad? true : false;
	}

    bool IsRay(GameObject target){
        var toVec = target.transform.position - transform.position;
        return !Physics.Raycast(transform.position, toVec, toVec.magnitude) ? true : false;
	}

    void Hit(EyeTargetParam targetParam) {
		targetParam.isFind = true;
        Debug.Log("Hit");
	}


    /// <summary>
/// 視界内にいるならtrueを返す
/// </summary>
/// <param name="target">ターゲット</param>
/// <returns>視界の中にいるならtrue</returns>
    public bool IsInEyeRange(GameObject target)
    {
        //全ての条件がtrueなら視界内に対象がいる。
        if (IsRange(target) && IsHeight(target) && IsRad(target) && IsRay(target)){
            return true;
        }
        else{
            return false;
        }
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
