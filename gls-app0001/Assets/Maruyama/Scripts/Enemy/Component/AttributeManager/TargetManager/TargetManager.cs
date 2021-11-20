using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UniRx;

using FoundType = FoundObject.FoundType;
using MaruUtility;

public class TargetManager : MonoBehaviour
{
    //ターゲットを見失ったときのデータ
    class LostData
    {
        public bool isActive;
        public Vector3 position;
        public FoundType type;

        public LostData(Vector3 position, FoundType type)
            :this(position, type, 0.0f, null)
        { }

        public LostData(Vector3 position, FoundType type, float activeTime, WaitTimer timer)
        {
            this.isActive = true;
            this.position = position;
            this.type = type;

            if (timer)
            {
                timer.AddWaitTimer(GetType(), activeTime, () => isActive = false);
            }
        }
    }

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

    [Header("音オブジェクトのポジションの乱数幅"), SerializeField]
    Vector3 m_soundPositionRandomRange = new Vector3(2.0f, 0.0f, 2.0f);

    [Header("見失ったポジションを保存する時間"), SerializeField]
    float m_lostDataSaveTime = 10.0f;

    //最後に参照されたターゲット
    readonly ReactiveProperty<FoundObject> m_nowTargetReactive = new ReactiveProperty<FoundObject>();
    FoundObject m_nowTarget
    {
        get => m_nowTargetReactive.Value;
        set => m_nowTargetReactive.Value = value;
    }

    //最後にターゲットを発見した場所を記録する。
    LostData m_lostData = null;

    //どのコンポーネントのターゲットかを確認する。
    Dictionary<Type,FoundObject> m_targets = new Dictionary<Type, FoundObject>();

    //対象外にしたいターゲット群 == exclude == 除外
    List<FoundObject> m_excludeTargets = new List<FoundObject>();

    StatusManagerBase m_statusManager;
    WaitTimer m_waitTimer;

    private void Awake()
    {
        m_statusManager = GetComponent<StatusManagerBase>();
        m_waitTimer = GetComponent<WaitTimer>();

        m_excludeTargets.Clear();
    }

    private void Start()
    {
        //nullCheck
        if(m_targets.Count == 0) {

        }

        //FoundObjectならバフを掛ける。
        m_nowTargetReactive.Skip(1)
            .Where(data => data != null)
            .Where(data => IsBuff(data))
            .Subscribe(_ => ChangeBuffParametor(m_buffParam))
            .AddTo(this);

        //それ以外ならバフを掛けない。
        m_nowTargetReactive.Skip(0)
            .Where(data => !IsBuff(data))
            .Subscribe(_ => ChangeBuffParametor(new BuffParametor(1.0f)))
            .AddTo(this);
    }

    private void Update()
    {
        if(m_nowTarget == null)
        {
            //Debug.Log("null");
        }
    }

    bool IsBuff(FoundObject data)
    {
        if(data == null) {
            return false;
        }

        if(data.GetFoundData().type == FoundType.Smell) {
            return true;
        }

        return false;
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
        FoundDataAdjust(type, target);
        m_nowTarget = target;
        m_targets[type] = target;
    }

    //ターゲットの更新を確定
    private void FoundDataAdjust(Type type, FoundObject target)
    {
        if(target == null) {
            return;
        }


    }

    public void AbsoluteSetNowTarget(Type type, FoundObject target)
    {
        FoundDataAdjust(type, target);
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

        //ターゲットが対象外なら更新しない
        if (IsExcludeTarget(target)) {
            return false;
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
    /// 対象外かどうかを判断する。
    /// </summary>
    /// <param name="newTarget"></param>
    /// <returns>対象外ならtrue</returns>
    bool IsExcludeTarget(FoundObject newTarget)
    {
        foreach(var target in m_excludeTargets)
        {
            if(target == newTarget)
            {
                return true;
            }
        }

        return false;
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

        if (m_nowTarget)  //null出なかったら更新する。
        {
            SetLostData();
        }
        
        return m_nowTarget;
    }

    public FoundType? GetNowTargetType()
    {
        if(m_nowTarget == null) {
            return FoundType.None;
        }

        return m_nowTarget.GetFoundData().type;
    }

    public Vector3? GetNowTargetPosition()
    {
        if (m_nowTarget)  //ターゲットがnull出ない。
        {
            //対象のアクティブがfalseなら
            if (m_nowTarget.gameObject.activeSelf == false)
            {
                return GetLostPosition(); //ターゲットをnullにする。
            }
        }
        
        return m_nowTarget ? CalcuFoundPosition() : GetLostPosition();
    }

    Vector3 CalcuFoundPosition()
    {
        var data = m_nowTarget.GetFoundData();
        return data.position;
    }

    void SetLostData()
    {
        //見失ったから大まかな位置にする。
        var data = m_nowTarget.GetFoundData();
        //var targetPosition = m_nowTarget.transform.position;
        var position = data.position;
        
        m_lostData = new LostData(position, data.type, m_lostDataSaveTime, m_waitTimer);
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
    /// 現在のターゲット方向のベクトルを返す。
    /// </summary>
    /// <returns></returns>
    public Vector3? GetToNowTargetVector()
    {
        var positionCheck = GetNowTargetPosition();
        if (positionCheck != null)
        {
            var position = (Vector3)positionCheck;
            return position - transform.position;
        }

        return null;
    }

    /// <summary>
    /// 最後にターゲットを確認できた場所
    /// </summary>
    /// <returns></returns>
    public Vector3? GetLostPosition()
    {
        if (m_lostData == null) {
            return null;
        }
        if (!m_lostData.isActive) {
            return null;
        }

        Debug.Log("GetLostTarget");
        return m_lostData.position;
    }

    /// <summary>
    /// 対象外にするターゲットを増やす
    /// </summary>
    public void AddExcludeTarget(FoundObject target)
    {
        m_excludeTargets.Add(target);
        if(target == m_nowTarget) {
            SetNowTarget(GetType(), null);
        }
    }

    /// <summary>
    /// 現在のターゲットを対象外にする。
    /// </summary>
    public void AddExcludeNowTarget()
    {
        m_excludeTargets.Add(m_nowTarget);
        SetNowTarget(GetType(), null);
    }

    //アクセッサ・プロパティ--------------------------------------------------------

    /// <summary>
    /// ターゲットを持っているかどうか
    /// </summary>
    /// <returns>ターゲットを持っていたらターゲットにする。</returns>
    public bool HasTarget()
    {
        return m_nowTarget == null ? false : true;
    }

    public BuffParametor buffParametor
    {
        get => m_buffParam;
        set => m_buffParam = value;
    }
}
