using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TargetMgr : MonoBehaviour
{
    //最後に参照されたターゲット
    GameObject m_nowTarget = null;

    //どのコンポーネントのターゲットかを確認する。
    Dictionary<Type,GameObject> m_targets = new Dictionary<Type, GameObject>();

    private void Start()
    {
        if(m_targets.Count == 0){
            var target = GameObject.Find("Player");
            m_nowTarget = target;
        }
    }

    /// <summary>
    /// ターゲットの追加
    /// </summary>
    /// <param name="type">どのコンポーネントのターゲットか</param>
    /// <param name="target">ターゲット</param>
    public void AddTarget(Type type,GameObject target)
    {
        m_targets[type] = target;
    }

    /// <summary>
    /// 現在追従するターゲットのセット
    /// </summary>
    /// <param name="type">コンポーネントのタイプ</param>
    /// <param name="target">ターゲット</param>
    public void SetNowTarget(Type type,GameObject target)
    {
        m_nowTarget = target;
        m_targets[type] = target;
    }

    /// <summary>
    /// 最後に参照されたターゲットを取得
    /// </summary>
    /// <returns>ターゲット</returns>
    public GameObject GetNowTarget()
    {
        return m_nowTarget;
    }

    public GameObject GetNowTarget(Type type)
    {
        //keyが存在しなかったら
        if (!m_targets.ContainsKey(type)){
            return null;
        }

        m_nowTarget = m_targets[type];
        return m_nowTarget;
    }
}
