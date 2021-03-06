using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

[Serializable]
public struct ThrongManagerParametor  //群衆Managerのパラメータ
{
    [Header("群衆と判断する範囲")]
    public float inThrongRange;   //群衆と判断する範囲

    [Header("隣人と判断される距離")]
    public float nearObjectRange; //隣人と判断される距離
    [Header("回避する最大力")]
    public float maxAvoidPower;   //回避する最大力

    public ThrongManagerParametor(float inThrongRange, float nearObjectRange, float maxAvoidPower)
    {
        this.inThrongRange = inThrongRange;
        this.nearObjectRange = nearObjectRange;
        this.maxAvoidPower = maxAvoidPower;
    }
}

//他の群衆を使うためのデータ
[Serializable]
public struct ThrongData
{
    public GameObject gameObject;
    public EnemyVelocityManager velocityMgr;
    public TargetManager targetMgr;  //ターゲット管理
    public ThrongManager throngMgr;  //群衆管理
    public RandomPlowlingMove randomPlowlingMove; //ランダム徘徊
    public DropObjecptManager dropManager;
    public ClearManager_Zombie clearManager;
    public EnemyRespawnManager respawn;  //リスポーン
    public EnemyRotationCtrl rotationController;

    public ThrongData(EnemyVelocityManager velocityMgr, TargetManager targetMgr, ThrongManager throngMgr,
        RandomPlowlingMove randomPlowlingMove, DropObjecptManager dropManager, ClearManager_Zombie clearManager,
        EnemyRespawnManager respawn, EnemyRotationCtrl rotationController)
    {
        this.gameObject = targetMgr.gameObject;
        this.velocityMgr = velocityMgr;
        this.targetMgr = targetMgr;
        this.throngMgr = throngMgr;
        this.randomPlowlingMove = randomPlowlingMove;
        this.dropManager = dropManager;
        this.clearManager = clearManager;
        this.respawn = respawn;
        this.rotationController = rotationController;
    }
}


/// <summary>
/// 群衆行動マネージャ―
/// </summary>
public class ThrongManager : MonoBehaviour
{
    [SerializeField]
    private ThrongManagerParametor m_param = new ThrongManagerParametor();

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    private string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    //コンポーネント系-----------------

    [SerializeField]
    private ThrongGeneratorBase m_generator = null;
    private AllEnemyGeneratorManager m_generatorManager;

    //private EnemyRotationCtrl m_rotationCtrl;
    private EnemyVelocityManager m_velocityManager;

    private void Awake()
    {
        //m_rotationCtrl = GetComponent<EnemyRotationCtrl>();
        m_velocityManager = GetComponent<EnemyVelocityManager>();

        
    }

    private void Start()
    {
        m_generatorManager = FindObjectOfType<AllEnemyGeneratorManager>();
        //SetSearchGenerator();
    }

    private void Update()
    {
        if(m_generator != null)
        {
            ThrongMoveUpdate(m_velocityManager);
        }
    }

    /// <summary>
    /// 近くの集団を回避する処理
    /// </summary>
    /// <param name="rigid">自身のリジッドボディ</param>
    /// <param name="moveDirect">そのオブジェクトが向かいたい方向</param>
    /// <param name="maxSpeed">最大スピード</param>
    /// <param name="truningPower">旋回パワー</param>
    private void ThrongMoveUpdate(EnemyVelocityManager velocityMgr)
    {
        var velocity = velocityMgr.velocity;
        velocity += m_velocityManager.GetForce() * Time.deltaTime;

        var throngForce = CalcuVelocity.CalucSeekVec(velocity, CalcuThrongVector(), CalcuAverageSpeed());
        //throngForce.y = 0;

        if(IsUpperVector()) //将来的に必要かも？
        {
            //var upperForce = CalcuVelocity.CalucSeekVec(velocity, CalcuSumUpperVector(), CalcuAverageSpeed());
            //throngForce += upperForce;
            //throngForce.y += CalcuSumUpperVector().y * 10.0f;
            //Debug.Log("◆◆" + throngForce);
        }

        velocityMgr.AddForce(throngForce);
    }

    /// <summary>
    /// ランダムに移動するゾンビを集団に合わせた方向に統合する。
    /// </summary>
    /// <param name="plowlingMove">コンポ―ネントそのもの</param>
    public Vector3 CalcuRandomPlowlingMovePositonIntegrated(RandomPlowlingMove plowlingMove)
    {
        var throngDatas = GetAllThrongDatas();

        int throngSize = 0;
        Vector3 sumPosition = Vector3.zero;
        foreach (var data in throngDatas)
        {
            if (IsRange(data, m_param.inThrongRange))
            {
                sumPosition += data.randomPlowlingMove.GetTargetPosition();
                throngSize++;
            }
        }

        return sumPosition / throngSize;
    }

    /// <summary>
    /// 群衆の中心のベクトル
    /// </summary>
    /// <param name="data">集団データ</param>
    /// <returns>群衆の中心データ</returns>
    private Vector3 CalcuCenterVector(ThrongData data)
    {
        return data.gameObject.transform.position;
    }

    /// <summary>
    /// dataの進行方向
    /// </summary>
    /// <param name="data">集団データ</param>
    /// <returns>dataの進行方向</returns>
    private Vector3 CalcuDirectVector(ThrongData data)
    {
        return data.velocityMgr.velocity;
    }

    /// <summary>
    /// オブジェクト同士の回避
    /// </summary>
    /// <param name="data">集団データ</param>
    /// <returns>オブジェクト同士の回避ベクトル</returns>
    private Vector3 CalcuAvoidVector(ThrongData data)
    {
        if(data.gameObject == gameObject) {  //自分自身なら処理をしない
            return Vector3.zero;
        }

        //相手から自分自身に向かうベクトル
        var toSelfVec = transform.position - data.gameObject.transform.position;

        if(IsRange(data, m_param.nearObjectRange)) {  //隣人なら
            var power = m_param.maxAvoidPower - toSelfVec.magnitude;
            var avoidVec = toSelfVec.normalized * power;

            return avoidVec;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 上昇するベクトル
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcuUpperVector(ThrongData data)
    {
        const float nearRange = 1.0f;
        Vector3 upperVector = Vector3.up;  //将来的にはインスペクタからUpperベクトルを制御できるようにする。
        
        if(data.gameObject == gameObject) {
            return Vector3.zero;
        }

        if (data.gameObject.transform.position.y >= transform.position.y) //自分より上にいたら
        {
            return Vector3.zero;
        }

        if (IsRange(data, nearRange)) //近くにいる場合
        {
            var toTargetVector = data.gameObject.transform.position - transform.position;
            if (UtilityMath.IsFront(transform.forward, toTargetVector)) //正面にいる場合
            {
                return upperVector;
            }
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 群衆行動ベクトルの計算(まだ未完成)
    /// </summary>
    /// <returns></returns>
    public Vector3 CalcuThrongVector()
    {
        var throngDatas = GetAllThrongDatas();

        Vector3 centerPosition = Vector3.zero;
        Vector3 avoidVec = Vector3.zero;
        Vector3 directVec = Vector3.zero;
        float sumSpeed = 0.0f;

        int throngSize = 0;
        foreach (var data in throngDatas)
        {
            if (data.gameObject == gameObject) {  //自分自身なら処理をしない
                continue;
            }

            //集団距離内でかつ、障害物がない場合
            if (IsRange(data,m_param.inThrongRange) && !IsRayHit(data)) 
            {
                centerPosition += CalcuCenterVector(data);
                directVec += CalcuDirectVector(data); //群衆の平均方向
                avoidVec += CalcuAvoidVector(data);

                sumSpeed += data.velocityMgr.velocity.magnitude;
                
                throngSize++;
            }
        }

        if (throngSize == 0)
        {  //存在しなかったらzeroを返す。
            return Vector3.zero;
        }

        centerPosition /= throngSize;
        directVec /= throngSize;

        var reVec = (centerPosition + directVec + avoidVec) - transform.position;

        return reVec;
    }

    private Vector3 CalcuSumUpperVector()
    {
        Vector3 sumUpperVec = Vector3.zero;
        
        foreach(var data in GetAllThrongDatas())
        {
            if(data.gameObject == gameObject) {
                continue;
            }

            sumUpperVec += CalcuUpperVector(data);
        }

        return sumUpperVec;
    }

    /// <summary>
    /// 近い距離のゾンビを避けるVector
    /// </summary>
    /// <returns>避けるベクトルの合計</returns>
    public Vector3 CalcuSumAvoidVector()
    {
        var throngDatas = GetAllThrongDatas();
        Vector3 avoidVector = Vector3.zero;

        foreach (var data in throngDatas)
        {
            if (data.gameObject == gameObject) {  //自分自身なら処理をしない
                continue;
            }

            avoidVector += CalcuAvoidVector(data);
        }

        return avoidVector;
    }

    /// <summary>
    /// 集団の平均スピードを計算して返す。
    /// </summary>
    /// <returns>平均スピード</returns>
    private float CalcuAverageSpeed()
    {
        var throngDatas = GetAllThrongDatas();
        float sumSpeed = 0.0f;
        int throngSize = 0;

        foreach (var data in throngDatas)
        {
            if (IsRange(data, m_param.nearObjectRange))
            {
                sumSpeed += data.velocityMgr.velocity.magnitude;
                throngSize++;
            }
        }

        if(throngSize == 0)
        {
            return 0;
        }

        return sumSpeed / throngSize;
    }

    /// <summary>
    /// 指定した距離より短かったらtrue
    /// </summary>
    /// <param name="data">対象のデータ</param>
    /// <param name="range">距離</param>
    /// <returns>短かったらtrue</returns>
    private bool IsRange(ThrongData data ,float range)
    {
        var toVec = data.gameObject.transform.position - transform.position;

        return toVec.magnitude < range ? true : false;
    }

    /// <summary>
    /// 障害物がヒットしたらtrue
    /// </summary>
    /// <param name="data">相手のデータ</param>
    /// <returns>ヒットしたらtrue</returns>
    private bool IsRayHit(ThrongData data)
    {
        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        var toVec = data.gameObject.transform.position - transform.position;
        return Physics.Raycast(transform.position, toVec, toVec.magnitude, obstacleLayer);
    }

    /// <summary>
    /// 上昇ベクトルが必要かどうか
    /// </summary>
    /// <returns></returns>
    private bool IsUpperVector()
    {
        foreach (var data in GetAllThrongDatas())
        {
            if (data.gameObject == gameObject) {
                continue;
            }

            var upperVec = CalcuUpperVector(data);
            if(upperVec != Vector3.zero) {
                return true;
            }
        }

        return false;
    }

    //アクセッサ-----------------------------------------------

    /// <summary>
    /// 群衆データリストを渡す。
    /// </summary>
    /// <returns>群衆データリスト</returns>
    public List<ThrongData> GetAllThrongDatas()
    {
        return m_generatorManager.GetAllThrongDatas();
    }

    public void SetInThrongRange(float range)
    {
        m_param.inThrongRange = range;
    }
    public float GetInThrongRange()
    {
        return m_param.inThrongRange;
    }

    public void SetNearObjectRange(float range)
    {
        m_param.nearObjectRange = range;
    }
    public float GetNearObjectRange()
    {
        return m_param.nearObjectRange;
    }

    public void SetParametor(ThrongManagerParametor param)
    {
        m_param = param;
    }
    public ThrongManagerParametor GetParametor()
    {
        return m_param;
    }
    
    public void SetGenerator(ThrongGeneratorBase generator)
    {
        m_generator = generator;
    }
}
