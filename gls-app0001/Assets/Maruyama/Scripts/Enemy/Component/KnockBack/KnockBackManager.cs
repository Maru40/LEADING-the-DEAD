using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;
using UniRx;

public class KnockBackManager : MonoBehaviour
{
    [Serializable]
    struct Parametor
    {
        public float lenght;
        public float speed;

        static public Parametor operator *(Parametor param, float power)
        {
            param.lenght = param.lenght * power;
            param.speed = param.speed * power;

            return param;
        }
    }

    EnemyVelocityMgr m_velocityManager;

    //吹き飛ばしの基礎値
    [SerializeField]
    Parametor m_baseParam = new Parametor();
    Parametor m_param = new Parametor();
    float m_elapsedLength = 0.0f;

    Vector3 m_direct = Vector3.zero;

    /// <summary>
    /// ノックバックしたかどうか
    /// </summary>
    readonly ReactiveProperty<bool> m_isKnockBackReactive = new ReactiveProperty<bool>();
    public IObservable<bool> IsKnockBackReactive => m_isKnockBackReactive;
    public bool IsKnockBack
    {
        get => m_isKnockBackReactive.Value;
        private set => m_isKnockBackReactive.Value = value;
    }

    private void Awake()
    {
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
    }

    private void Update()
    {
        if (IsKnockBack)
        {
            MoveProcess();
        }
    }

    void MoveProcess()
    {
        var direct = m_direct;
        direct.y = 0;

        var rate = 1.0f;// - m_elapsedLength / m_param.lenght;
        var speed = m_param.speed * rate;
        var moveVec = direct.normalized * speed * Time.deltaTime;
        transform.position += moveVec;

        m_elapsedLength += moveVec.magnitude;
        if (m_elapsedLength > m_param.lenght)
        {
            IsKnockBack = false;
        }
    }

    public void KnockBack(AttributeObject.DamageData data)
    {
        var other = data.obj;
        if(other == null || enabled == false) { 
            return; 
        }
        
        //飛ばしたい方向
        m_direct = transform.position - other.transform.position;
        m_param = m_baseParam;

        IsKnockBack = true;
        m_elapsedLength = 0.0f;

        m_velocityManager?.ResetAll();
    }
}
