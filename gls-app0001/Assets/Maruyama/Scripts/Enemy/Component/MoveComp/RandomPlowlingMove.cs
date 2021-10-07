using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using MaruUtility;

using System;

/// <summary>
/// Randomに徘徊するコンポーネント
/// </summary>
public class RandomPlowlingMove : MonoBehaviour
{
    [Serializable]
    public struct Parametor 
    {
        public float randomPositionRadius;  //徘徊する場所を決める半径
        public float maxSpeed;              //最大スピード
        public float turningPower;          //旋回する力
        public float targetNearRange;       //目的地に着いたと判断される,目的地との相対距離(小さすぎると判断できなくなるため注意)
        public float maxWaitCalcuRouteTime; //目的地についたとき、立ち止まる最大の時間
        public float inThrongRange;         //集団と認識する範囲

        public Parametor(float randomPositionRadius, float maxSpeed, float turningPower,
            float targetNearRange, float maxWaitCalcuRouteTime, float inThrongRange)
        {
            this.randomPositionRadius = randomPositionRadius;
            this.maxSpeed = maxSpeed;
            this.turningPower = turningPower;
            this.targetNearRange = targetNearRange;
            this.maxWaitCalcuRouteTime = maxWaitCalcuRouteTime;
            this.inThrongRange = inThrongRange;
        }
    }


    //member変数-------------------------------------------------------------------------------------------

    [SerializeField]
    Parametor m_param = new Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);
    float m_firstRandomPositionRadius;
    float m_firstInThrongRange;

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    GameObject m_centerObject = null;

    Vector3 m_targetPosition;    //目的の場所

    EnemyVelocityMgr m_velocityMgr;
    WaitTimer m_waitTimer;
    ThrongManager m_throngMgr;
    EnemyRotationCtrl m_rotationCtrl;

    void Awake()
    {
        //コンポーネントの取得
        m_velocityMgr = GetComponent<EnemyVelocityMgr>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_throngMgr = GetComponent<ThrongManager>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_centerObject = gameObject;

        //シード値
        Random.InitState(System.DateTime.Now.Millisecond);

        m_firstRandomPositionRadius = m_param.randomPositionRadius;
        m_firstInThrongRange = m_param.inThrongRange;

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
        Vector3 force = CalcuVelocity.CalucSeekVec(m_velocityMgr.velocity, toVec, m_param.maxSpeed);
        m_velocityMgr.AddForce(force * m_param.turningPower);

        m_rotationCtrl.SetDirect(m_velocityMgr.velocity);

        ThrongProcess();  //集団行動系

        if (IsRouteEnd())
        {
            RouteEndProcess();
        }
    }

    /// <summary>
    /// 集団行動の処理
    /// </summary>
    void ThrongProcess()
    {
        if (m_throngMgr) //もし集団行動するなら...
        {
            if (m_throngMgr.enabled)
            {
                var newTargetPosition = m_throngMgr.CalcuRandomPlowlingMovePositonIntegrated(this);  //ランダムな方向を集団に合わせる。
                SetTargetPositon(newTargetPosition);
            }
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

        return range <= m_param.targetNearRange ? true : false;
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
        var waitTime = UnityEngine.Random.value * m_param.maxWaitCalcuRouteTime;
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
        if(m_centerObject == null) {
            ResetCenterObject();
        }

        float directX = CalucRandomDirect();
        float directZ = CalucRandomDirect();

        float x = Random.value * m_param.randomPositionRadius * directX;
        float y = transform.position.y;
        float z = Random.value * m_param.randomPositionRadius * directZ;

        var toVec = new Vector3(x,y,z);
        var newPosition = m_centerObject.transform.position + toVec;

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

    public void ResetCenterObject()
    {
        m_centerObject = this.gameObject;
        m_param.randomPositionRadius = m_firstRandomPositionRadius;
        m_throngMgr.enabled = true;
        //m_param.inThrongRange = m_firstInThrongRange;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsObstract(collision.gameObject))
        {
            SetRandomTargetPosition();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsObstract(collision.gameObject))
        {
            SetRandomTargetPosition();
        }
    }

    bool IsObstract(GameObject gameObj)
    {
        foreach(var str in m_rayObstacleLayerStrings)
        {
            int obstacleLayer = LayerMask.NameToLayer(str);
            if(gameObj.layer == obstacleLayer) {
                return true;
            }
        }

        return false;
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

    public void SetInThrongRange(float range)
    {
        m_param.inThrongRange = range;
    }
    public  float GetInThrongRange()
    {
        return m_param.inThrongRange;
    }

    public void SetCenterObject(GameObject centerObject)
    {
        m_centerObject = centerObject;
        SetRandomTargetPosition();
    }
    public GameObject GetCenterObject()
    {
        return m_centerObject;
    }

    public void SetParametor(Parametor parametor)
    {
        m_param = parametor;
    }

    public void AddParametor(Parametor parametor)
    {
        m_param.randomPositionRadius += parametor.randomPositionRadius;
        m_param.maxSpeed += parametor.maxSpeed;
        m_param.turningPower += parametor.turningPower;
        m_param.targetNearRange += parametor.targetNearRange;
        m_param.maxWaitCalcuRouteTime += parametor.maxWaitCalcuRouteTime;
        m_param.inThrongRange += parametor.inThrongRange;
    }

    

}
