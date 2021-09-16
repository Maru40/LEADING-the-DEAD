using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


/// <summary>
/// �^�[�Q�b�g�̊Ǘ��p�̃p�����[�^
/// </summary>
[Serializable]
class EyeTargetParam
{
    public GameObject target;
    public bool isFind;  //��������Ԃ��ǂ�����Ԃ�

    public EyeTargetParam(GameObject target)
    {
        this.target = target;
        this.isFind = false;
    }
}

/// <summary>
/// ���E�̃p�����[�^�̍\����
/// </summary>
[Serializable]
public class EyeSearchRangeParam
{
    public float lenght; //���G�͈�(���S�~��)
    public float height; //���G�͈�(����)
    public float rad;    //���G�͈�(�p�x)

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
/// ���E�̊Ǘ�
/// </summary>
public class EyeSearchRange : MonoBehaviour
{
    //�͈͂ɓ����Ă��邩�̑ΏۂɂȂ�I�u�W�F�N�g
    [SerializeField]
    List<EyeTargetParam> m_targetParams = new List<EyeTargetParam>();

    [SerializeField]
    EyeSearchRangeParam m_param = new EyeSearchRangeParam();

    private void Update()
    {
        foreach(var param in m_targetParams)
        {
            if (IsInEyeRange(param.target))
            {  //�^�[�Q�b�g�����E�ɓ����Ă�����B
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
		//�����`�F�b�N
		return toVec.magnitude <= m_param.lenght? true : false;
	}

    bool IsHeight(GameObject target) {
		var selfPos = transform.position;
        var targetPos = target.transform.position;

        var subHeight = targetPos.y - selfPos.y;  //�����̍������߂�B
		//������������������B
		return Mathf.Abs(subHeight) <= m_param.height? true : false;
	}

    bool IsRad(GameObject target) {
		var forward = transform.transform.forward;
        forward.y = 0.0f;
        var toVec = target.transform.position - transform.position;
        toVec.y = 0.0f;

		var newDot = Vector3.Dot(forward.normalized, toVec.normalized);
        var newRad = Mathf.Acos(newDot);
		//���G�͈͂ɓ����Ă�����B
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
/// ���E���ɂ���Ȃ�true��Ԃ�
/// </summary>
/// <param name="target">�^�[�Q�b�g</param>
/// <returns>���E�̒��ɂ���Ȃ�true</returns>
    public bool IsInEyeRange(GameObject target)
    {
        //�S�Ă̏�����true�Ȃ王�E���ɑΏۂ�����B
        if (IsRange(target) && IsHeight(target) && IsRad(target) && IsRay(target)){
            return true;
        }
        else{
            return false;
        }
    }

    //�A�N�Z�b�T-------------------------------------------------------------------------

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
