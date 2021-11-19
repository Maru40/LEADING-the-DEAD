﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellManaer : MonoBehaviour
{
    [Header("正体に気づく距離"), SerializeField]
    float m_nearRange = 0.5f;

    [Header("近くで待機する時間"),SerializeField]
    float m_nearWaitTime = 1.0f;

    TargetManager m_targetManager;
    I_Smell m_smell;
    WaitTimer m_waitTimer;
    EnemyVelocityMgr m_velocityManager;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_smell = GetComponent<I_Smell>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
    }

    private void Update()
    {
        if (IsUpdate())
        {
            var meat = m_targetManager.GetNowTarget().GetComponent<MeatManager>();
            if (meat)
            {
                MeatCheck();
                return;
            }

            var bloodPuddle = m_targetManager.GetNowTarget().GetComponent<BloodPuddleManager>();
            if (bloodPuddle)
            {
                BloodPuddleCheck();
                return;
            }
        }
    }

    /// <summary>
    /// アップデート処理が必要かどうか
    /// </summary>
    /// <returns></returns>
    bool IsUpdate()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットが無かったら処理をしない。
            return false; 
        }

        var type = m_targetManager.GetNowTargetType();
        //タイプが匂いタイプなら。
        return type == FoundObject.FoundType.Smell ? true : false;
    }

    bool IsTargetNear(float nearRange)
    {
        var positionCheck = m_targetManager.GetToNowTargetVector();
        if (positionCheck == null) {
            return false;
        }
        var toTargetVec = (Vector3)positionCheck;

        //正体に気づく距離まで来たら。
        return toTargetVec.magnitude < nearRange ? true : false;  
    }

    /// <summary>
    /// 肉のチェック
    /// </summary>
    void MeatCheck()
    {
        if(IsTargetNear(m_nearRange))
        {
            //食べるモーション再生
        }
    }

    /// <summary>
    /// 血の液体チェック
    /// </summary>
    void BloodPuddleCheck()
    {
        if (IsTargetNear(m_nearRange))  //正体に気づく距離まで来たら。
        {
            enabled = false;
            m_velocityManager.ResetAll();
            m_velocityManager.enabled = false;

            m_waitTimer.AddWaitTimer(GetType(), m_nearWaitTime, () => {
                enabled = true;
                m_velocityManager.enabled = true;
                m_velocityManager.ResetAll();
                m_targetManager.AddExcludeNowTarget();  //ターゲット対象から外す。
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var foundObject = other.GetComponent<FoundObject>();
        if(foundObject) 
        {
            //対象が匂いタイプだったら
            if(foundObject.GetFoundData().type == FoundObject.FoundType.Smell)
            {
                m_smell?.SmellFind(foundObject);
            }
        }
    }
}