using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using System;

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

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    //目的の場所
    Vector3 m_targetPosition;

    //Rigidbody m_rigid;
    EnemyVelocityMgr m_velocityMgr;
    WaitTimer m_waitTimer;
    ThrongMgr m_throngMgr;
    
    void Start()
    {
        //コンポーネントの取得
        //m_rigid = GetComponent<Rigidbody>();
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_throngMgr = GetComponent<ThrongMgr>();

        //シード値
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

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
        var toVec = m_targetPosition - transform.position;
        m_throngMgr.AvoidNearThrong(m_velocityMgr, toVec, m_speed);

        var newTargetPosition = m_throngMgr.CalcuRandomPlowlingMovePositonIntegrated(this);  //ランダムな方向を集団に合わせる。
        SetTargetPositon(newTargetPosition);

        //var newForce = UtilityVelocity.CalucArriveVec(velocity, toVec, m_speed);
        //m_rigid.AddForce(newForce);

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
        m_velocityMgr.ResetVelocity();  //速度のリセット

        //待機状態の設定
        var waitTime = UnityEngine.Random.value * m_maxWaitCalucRouteTime;
        m_waitTimer.AddWaitTimer(GetType(), waitTime);
    }

    /// <summary>
    /// ランダムな目的地を設定
    /// </summary>
    void SetRandomTargetPosition()
    {
        int numLoop = 100; //無限ループを防ぐため100を限度にする。
        for(int i = 0; i < numLoop; i++)
        {
            m_targetPosition = CalucRandomTargetPosition();

            var toVec = m_targetPosition - transform.position;
            int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
            if (!Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer)) {
                break;  //障害物がなかったら
            }
        }
    }

    /// <summary>
    /// ランダムな方向を計算して返す。
    /// </summary>
    /// <returns>ランダムな方向</returns>
    Vector3 CalucRandomTargetPosition()
    {
        float directX = CalucRandomDirect();
        float directZ = CalucRandomDirect();

        float x = UnityEngine.Random.value * m_randomPositionRadius * directX;
        float y = transform.position.y;
        float z = UnityEngine.Random.value * m_randomPositionRadius * directZ;

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
        if(collision.gameObject.tag == "T_Wall")
        {
            SetRandomTargetPosition();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "T_Wall")
        {
            SetRandomTargetPosition();
        }
    }

    void ReverseTargetPosition()
    {
        m_targetPosition = -m_targetPosition;
    }


    //アクセッサ-----------------------------------------------------

    public void SetTargetPositon(Vector3 position)
    {
        m_targetPosition = position;
    }
    public Vector3 GetTargetPosition()
    {
        return m_targetPosition;
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
