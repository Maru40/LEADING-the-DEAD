using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TargetManager : MonoBehaviour
{
    //最後に参照されたターゲット
    FoundObject m_nowTarget = null;

    //どのコンポーネントのターゲットかを確認する。
    Dictionary<Type,FoundObject> m_targets = new Dictionary<Type, FoundObject>();

    private void Start()
    {
        //nullCheck
        if(m_targets.Count == 0) {
            var target = GameObject.Find("Player");
            m_nowTarget = target.GetComponent<FoundObject>();
        }
    }

    private void Update()
    {
        //Debug.Log(m_nowTarget);
    }

    /// <summary>
    /// ターゲットの追加
    /// </summary>
    /// <param name="type">どのコンポーネントのターゲットか</param>
    /// <param name="target">ターゲット</param>
    public void AddTarget(Type type, FoundObject target)
    {
        m_targets[type] = target;
    }

    /// <summary>
    /// 現在追従するターゲットのセット
    /// </summary>
    /// <param name="type">コンポーネントのタイプ</param>
    /// <param name="target">ターゲット</param>
    public void SetNowTarget(Type type, FoundObject target)
    {
        //nowTargetがnullでなかったら
        if(m_nowTarget != null) {
            if (!IsTargetUpdate(target)) {  //更新が必要ないなら
                return;  //更新せずに処理を飛ばす。
            }
        }

        //更新
        m_nowTarget = target;
        m_targets[type] = target;
    }

    public void AbsoluteSetNowTarget(Type type, FoundObject target)
    {
        m_nowTarget = target;
        m_targets[type] = target;
    }

    /// <summary>
    /// 更新が必要かどうかを返す。
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <returns>更新が必要ならtrue</returns>
    bool IsTargetUpdate(FoundObject target)
    {
        if(target == null) {
            return true; //ターゲットがnullならtrueを返す。
        }

        var newPriority = target.GetFoundData().priority;
        var nowPriority = m_nowTarget.GetFoundData().priority;

        if (nowPriority < newPriority) //新しい方が優先度が高かったら更新
        {
            return true;
        }

        if(nowPriority == newPriority) //新しい方と優先度が同じなら。
        {
            return IsNearNewTarget(target);  //新しいターゲットが近いなら更新あり。
        }

        return false;  //どちらでも無かったら更新しない。
    }

    /// <summary>
    /// 新しいターゲットの方が近いかどうか
    /// </summary>
    /// <param name="target"></param>
    /// <returns>新しいターゲットが近いならtrue</returns>
    bool IsNearNewTarget(FoundObject newTarget)
    {
        var nowTargetPosition = m_nowTarget.gameObject.transform.position;
        var newTargetPosition = newTarget.gameObject.transform.position;

        var toNowTarget = nowTargetPosition - transform.position;
        var toNewTarget = newTargetPosition - transform.position;

        //現在の方が近いならfalse
        return (toNowTarget.magnitude < toNewTarget.magnitude) ? false : true;
    }

    /// <summary>
    /// 最後に参照されたターゲットを取得
    /// </summary>
    /// <returns>ターゲット</returns>
    public FoundObject GetNowTarget()
    {
        return m_nowTarget;
    }

    public FoundObject.FoundData? GetNowTargetFoundData()
    {
        return m_nowTarget?.GetFoundData();
    }

    public FoundObject GetNowTarget(Type type)
    {
        //keyが存在しなかったら
        if (!m_targets.ContainsKey(type)){
            return null;
        }

        m_nowTarget = m_targets[type];
        return m_nowTarget;
    }

    public FoundObject.FoundData? GetNowTargetFoundData(Type type)
    {
        //keyが存在しなかったら
        if (!m_targets.ContainsKey(type))
        {
            return null;
        }

        m_nowTarget = m_targets[type];
        return m_nowTarget.GetFoundData();
    }

    /// <summary>
    /// 現在のターゲット方向のベクトルを返す
    /// </summary>
    /// <returns>ターゲット方向のベクトル</returns>
    public Vector3 GetToNowTargetVector()
    {
        return m_nowTarget.transform.position - transform.position;
    }
}
