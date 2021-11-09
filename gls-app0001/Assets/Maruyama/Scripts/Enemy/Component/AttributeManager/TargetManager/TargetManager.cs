using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

public class TargetManager : MonoBehaviour
{
    [Serializable]
    public struct BuffParametor
    {
        public float speed;

        public BuffParametor(float speed)
        {
            this.speed = speed;
        }
    }

    [SerializeField]
    BuffParametor m_buffParam = new BuffParametor(1.1f);

    //最後に参照されたターゲット
    readonly ReactiveProperty<FoundObject> m_nowTargetReactive = new ReactiveProperty<FoundObject>();
    FoundObject m_nowTarget
    {
        get => m_nowTargetReactive.Value;
        set => m_nowTargetReactive.Value = value;
    }

    //最後にターゲットを発見した場所を記録する。
    Vector3 m_lostPosition = Vector3.zero;

    //どのコンポーネントのターゲットかを確認する。
    Dictionary<Type,FoundObject> m_targets = new Dictionary<Type, FoundObject>();

    StatusManagerBase m_statusManager;

    private void Awake()
    {
        m_statusManager = GetComponent<StatusManagerBase>();
    }

    private void Start()
    {
        //nullCheck
        if(m_targets.Count == 0) {
            //var target = GameObject.Find("Player");
            //m_nowTarget = target.GetComponent<FoundObject>();
        }

        //FoundObjectならバフを掛ける。
        m_nowTargetReactive.Skip(1)
            .Where(data => data != null)
            .Where(data => data.GetFoundData().type == FoundObject.FoundType.SoundObject)
            .Subscribe(_ => ChangeBuffParametor(m_buffParam))
            .AddTo(this);

        //それ以外ならバフを掛けない。
        m_nowTargetReactive.Skip(1)
            .Where(data => data != null)
            .Where(data => data.GetFoundData().type != FoundObject.FoundType.SoundObject)
            .Subscribe(_ => ChangeBuffParametor(new BuffParametor(1.0f)))
            .AddTo(this);
    }

    private void Update()
    {
        //Debug.Log(m_nowTarget);
    }

    /// <summary>
    /// ターゲットに対するバフの切替
    /// </summary>
    /// <param name="parametor">バフデータ</param>
    void ChangeBuffParametor(BuffParametor parametor)
    {
        var buffParametor = m_statusManager.GetBuffParametor();
        buffParametor.targetParam = parametor;

        m_statusManager.SetBuffParametor(buffParametor);
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
        if (m_nowTarget)  //ターゲットがnull出ない。
        {
            //対象のアクティブがfalseなら
            if (m_nowTarget.gameObject.activeSelf == false)
            {
                m_nowTarget = null; //ターゲットをnullにする。
            }
        }

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

    /// <summary>
    /// 最後にターゲットを確認できた場所
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLostPosition()
    {
        return m_lostPosition;
    }

    //アクセッサ・プロパティ--------------------------------------------------------

    public BuffParametor buffParametor
    {
        get => m_buffParam;
        set => m_buffParam = value;
    }
}
