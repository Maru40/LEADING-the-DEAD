﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

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

    bool m_isKnockBack = false;

    private void Awake()
    {
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
    }

    private void Update()
    {
        if (m_isKnockBack)
        {
            MoveProcess();
        }
    }

    void MoveProcess()
    {
        Debug.Log("ノックバック中");

        var direct = m_direct;
        direct.y = 0;

        var rate = 1.0f;// - m_elapsedLength / m_param.lenght;
        var speed = m_param.speed * rate;
        var moveVec = direct.normalized * speed * Time.deltaTime;
        //var moveVec = direct.normalized * (m_param.lenght - m_elapsedLength);
        transform.position += moveVec;

        m_elapsedLength += moveVec.magnitude;
        if (m_elapsedLength > m_param.lenght)
        {
            m_isKnockBack = false;
        }
    }

    public void KnockBack(AttributeObject.DamageData data)
    {
        var other = data.obj;
        if(other == null) { 
            return; 
        }
        
        //飛ばしたい方向
        m_direct = transform.position - other.transform.position;
        m_param = m_baseParam;

        m_isKnockBack = true;
        m_elapsedLength = 0.0f;
    }
}
