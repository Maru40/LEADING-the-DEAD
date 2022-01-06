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
    public struct Parametor : I_Random<Parametor>
    {
        [Header("徘徊する場所を決める半径")]
        public float randomPositionRadius; 
        [Header("最大スピード")]
        public float maxSpeed;             
        [Header("旋回する力")]
        public float turningPower;          
        [Header("目的地に着いたと判断される,目的地との相対距離(小さすぎると判断できなくなるため注意)")]
        public float targetNearRange;       
        [Header("目的地についたとき、立ち止まる最大の時間")]
        public float maxWaitCalcuRouteTime; 
        [Header("集団と認識する範囲")]
        public float inThrongRange;        

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

        public void Random(RandomRange<Parametor> range)
        {
            if (range.isActive == false) { return; }

            randomPositionRadius = UnityEngine.Random.Range(range.min.randomPositionRadius, range.max.randomPositionRadius);
            maxSpeed = UnityEngine.Random.Range(range.min.maxSpeed, range.max.maxSpeed);
            turningPower = UnityEngine.Random.Range(range.min.turningPower, range.max.turningPower);
            maxWaitCalcuRouteTime = UnityEngine.Random.Range(range.min.maxWaitCalcuRouteTime, range.max.maxWaitCalcuRouteTime);
            inThrongRange = UnityEngine.Random.Range(range.min.inThrongRange, range.max.inThrongRange);
        }
    }


    //member変数-------------------------------------------------------------------------------------------

    [SerializeField]
    private Parametor m_param = new Parametor(15.0f, 2.5f, 2.0f, 0.3f, 3.0f, 1.0f);
    private float m_firstRandomPositionRadius;
    //private float m_firstInThrongRange;

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    private string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    private GameObject m_centerObject = null;

    private Vector3 m_targetPosition;    //目的の場所

    private EnemyVelocityManager m_velocityMgr;
    private WaitTimer m_waitTimer;
    private ThrongManager m_throngMgr;
    private EnemyRotationCtrl m_rotationCtrl;
    private StatusManagerBase m_statusManager;

    [SerializeField]
    private float m_collisionStayTime = 2.0f;
    private float m_collisionStayTimerElapsed = 0.0f;

    private void Awake()
    {
        //コンポーネントの取得
        m_velocityMgr = GetComponent<EnemyVelocityManager>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_throngMgr = GetComponent<ThrongManager>();
        m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_statusManager = GetComponent<StatusManagerBase>();
        m_centerObject = gameObject;

        //シード値
        Random.InitState(System.DateTime.Now.Millisecond);

        m_firstRandomPositionRadius = m_param.randomPositionRadius;
        //m_firstInThrongRange = m_param.inThrongRange;

        SetRandomTargetPosition();
    }

    private void Update()
    {
        MoveProcess();
    }

    private void MoveProcess()
    {
        //待機状態なら処理をしない。
        if (m_waitTimer.IsWait(GetType())){
            return;
        }

        //加える力の計算
        var toVec = m_targetPosition - transform.position;
        var maxSpeed = m_param.maxSpeed * m_statusManager.GetBuffParametor().SpeedBuffMultiply;
        Vector3 force = CalcuVelocity.CalucSeekVec(m_velocityMgr.velocity, toVec, maxSpeed);
        m_velocityMgr.AddForce(force * m_param.turningPower);

        ThrongProcess();  //集団行動系

        if (IsRouteEnd())
        {
            RouteEndProcess();
        }

        Rotation();
    }

    private void Rotation()
    {
        if (m_waitTimer.IsWait(GetType())) {
            return;
        }

        if (m_velocityMgr.velocity != Vector3.zero)
        {
            m_rotationCtrl.SetDirect(m_velocityMgr.velocity);
        }
    }

    /// <summary>
    /// 集団行動の処理
    /// </summary>
    private void ThrongProcess()
    {
        if (m_throngMgr) //もし集団行動するなら...
        {
            if (m_throngMgr.enabled)
            {
                var newTargetPosition = m_throngMgr.CalcuRandomPlowlingMovePositonIntegrated(this);  //ランダムな方向を集団に合わせる。
                var toVec = newTargetPosition - transform.position;

                if (!IsRayHitObstacle(toVec))  //障害物が無かったら。
                {
                    SetTargetPositon(newTargetPosition);
                }
            }
        }
    }

    /// <summary>
    /// ルートの終了を判断。
    /// </summary>
    /// <returns>目的地についたらtrue</returns>
    private bool IsRouteEnd()
    {
        var selfPosition = transform.position;
        selfPosition.y = 0.0f;
        m_targetPosition.y = 0.0f;

        var toVec = m_targetPosition - selfPosition;
        float range = toVec.magnitude;

        return range <= m_param.targetNearRange ? true : false;
    }

    /// <summary>
    /// 目的地にたどり着いた時に行う処理
    /// </summary>
    private void RouteEndProcess()
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
    private void SetRandomTargetPosition()
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
    private Vector3 CalucRandomTargetPosition()
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
    private int CalucRandomDirect()
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

        SetRandomTargetPosition();
        //m_param.inThrongRange = m_firstInThrongRange;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enabled == false) {
            return;
        }

        if (IsObstract(collision.gameObject))
        {
            //m_targetPosition = -m_targetPosition;
            var toVec = m_targetPosition - transform.position;
            var reflectionVec = CalcuVelocity.Reflection(toVec, collision);

            m_targetPosition = reflectionVec;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsObstract(collision.gameObject))
        {
            m_collisionStayTimerElapsed += Time.deltaTime;

            if(m_collisionStayTimerElapsed >= m_collisionStayTime)
            {
                m_collisionStayTimerElapsed = 0.0f;
                SetRandomTargetPosition();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsObstract(collision.gameObject))
        {
            m_collisionStayTimerElapsed = 0.0f;
        }
    }

    private bool IsObstract(GameObject gameObj)
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

    /// <summary>
    /// Rayを飛ばして障害物があるかどうか？
    /// </summary>
    /// <param name="toVec">Rayを飛ばしたい方向</param>
    /// <returns>当たったらtrue</returns>
    private bool IsRayHitObstacle(Vector3 toVec)
    {
        //var toVec = m_targetPosition - transform.position;
        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        if (Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer))
        {
            return true;  //障害物がなかったら
        }
        else
        {
            return false;
        }
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

    public void SetRandomPositionRadius(float radius)
    {
        m_param.randomPositionRadius = radius;
    }
    public float GetRandomPositionRadius()
    {
        return m_param.randomPositionRadius;
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
