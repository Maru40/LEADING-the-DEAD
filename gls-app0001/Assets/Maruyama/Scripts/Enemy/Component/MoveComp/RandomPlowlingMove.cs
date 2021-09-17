using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomに徘徊するコンポーネント
/// </summary>
public class RandomPlowlingMove : MonoBehaviour
{
    //徘徊する場所を決める半径
    [SerializeField]
    float m_randomPositionRadius = 15.0f;

    //移動スピード
    [SerializeField]
    float m_speed = 3.0f;                                             

    //目的地に着いたと判断される,目的地との相対距離
    //小さすぎると判断できなくなるため注意
    [SerializeField]
    float m_targetNearRange = 0.3f;

    //目的地にたどり着いたときのスピード
    //[SerializeField]
    //float m_arrivalSpeed = 0.3f;

    //目的地についたとき、立ち止まる最大の時間
    [SerializeField]
    float m_maxWaitCalucRouteTime = 3.0f;

    //目的の場所
    Vector3 m_targetPosition;

    Rigidbody m_ridgid;
    WaitTimer m_waitTimer;
    
    void Start()
    {
        //コンポーネントの取得
        m_ridgid = GetComponent<Rigidbody>();
        m_waitTimer = GetComponent<WaitTimer>();

        //シード値
        Random.InitState(System.DateTime.Now.Millisecond);

        SetRandomTargetPosition();
    }
    
    void Update()
    {
        MoveProcess();
    }

    void MoveProcess()
    {
        //待機状態なら処理をしない。
        if (m_waitTimer.IsWait(GetType())){
            return;
        }

        //加える力の計算
        var velocity = m_ridgid.velocity;
        var toVec = m_targetPosition - transform.position;

        var newForce = UtilityVelocity.CalucArriveVec(velocity, toVec, m_speed);
        m_ridgid.AddForce(newForce);

        if (IsRouteEnd())
        {
            RouteEndProcess();
        }
    }

    /// <summary>
    /// ルートの終了を判断。
    /// </summary>
    /// <returns>目的地についたらtrue</returns>
    bool IsRouteEnd()
    {
        var toVec = m_targetPosition - transform.position;
        float range = toVec.magnitude;

        return range <= m_targetNearRange ? true : false;
    }

    /// <summary>
    /// 目的地にたどり着いた時に行う処理
    /// </summary>
    void RouteEndProcess()
    {
        if (m_waitTimer.IsWait(GetType())){
            return;
        }

        SetRandomTargetPosition();
        m_ridgid.velocity = Vector3.zero;  //速度のリセット

        //待機状態の設定
        var waitTime = Random.value * m_maxWaitCalucRouteTime;
        m_waitTimer.AddWaitTimer(GetType(), waitTime);
    }

    /// <summary>
    /// ランダムな目的地を設定
    /// </summary>
    void SetRandomTargetPosition()
    {
        m_targetPosition = CalucRandomTargetPosition();
    }

    /// <summary>
    /// ランダムな方向を計算して返す。
    /// </summary>
    /// <returns>ランダムな方向</returns>
    Vector3 CalucRandomTargetPosition()
    {
        float directX = CalucRandomDirect();
        float directZ = CalucRandomDirect();

        float x = Random.value * m_randomPositionRadius * directX;
        float y = transform.position.y;
        float z = Random.value * m_randomPositionRadius * directZ;

        var toVec = new Vector3(x,y,z);
        var newPosition = transform.position + toVec;

        return newPosition;
    }

    /// <summary>
    /// ランダムに1か-1にして返す。
    /// </summary>
    /// <returns>1,-1のどちらか</returns>
    int CalucRandomDirect()
    {
        float halfValue = 0.5f;
        float random = Random.value;

        return random < halfValue ? 1 : -1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SetRandomTargetPosition();
    }

    private void OnCollisionStay(Collision collision)
    {
        SetRandomTargetPosition();
    }


    //現在使用していない
    //bool IsRayHit(Vector3 position)
    //{
    //    RaycastHit hitData;
    //    var direction = position - transform.position;
    //    //var direction = transform.position - position;
    //    float range = direction.magnitude;

    //    if (Physics.Raycast(transform.position, direction, out hitData, range))
    //    //if (Physics.Raycast(position, direction, out hitData, range))
    //    {
    //        Debug.Log(transform.position - hitData.transform.position);
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

}
