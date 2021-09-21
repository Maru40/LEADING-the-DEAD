using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
struct ThrongMgrParametor  //群衆Mgrのパラメータ
{
    public float inThrongRange;   //群衆と判断する範囲
    public float outThrongRange;  //群衆から外れたと判断する距離

    public float nearObjectRange; //隣人と判断される距離
}

//他の群衆を使うためのデータ
[Serializable]
public struct ThrongData
{
    public GameObject gameObject;
    public EnemyVelocityMgr velocityMgr;
    public TargetMgr targetMgr;  //ターゲット管理
    public ThrongMgr throngMgr;  //群衆管理
    public RandomPlowlingMove randomPlowlingMove;

    public ThrongData(EnemyVelocityMgr velocityMgr, TargetMgr targetMgr, ThrongMgr throngMgr,
        RandomPlowlingMove randomPlowlingMove)
    {
        this.gameObject = targetMgr.gameObject;
        this.velocityMgr = velocityMgr;
        this.targetMgr = targetMgr;
        this.throngMgr = throngMgr;
        this.randomPlowlingMove = randomPlowlingMove;
    }
}


/// <summary>
/// 群衆行動マネージャ―
/// </summary>
public class ThrongMgr : MonoBehaviour
{

    [SerializeField]
    ThrongMgrParametor m_param = new ThrongMgrParametor();

    //List<ThrongData> m_throngDatas = new List<ThrongData>();  //グループのオブジェクト一覧

    //コンポーネント系-----------------

    [SerializeField]
    EnemyGenerator m_generator = null;

    Rigidbody m_rigid;

    void Start()
    {
        SetSearchGenerator();
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }
    
    //群衆の中心のベクトル
    Vector3 CalcuCenterVector(ThrongData data)
    {
        return data.gameObject.transform.position;
    }

    //dataの進行方向
    Vector3 CalcuDirectVector(ThrongData data)
    {
        return data.velocityMgr.velocity;
    }

    //オブジェクト同士の回避
    Vector3 CalcuAvoidVector(ThrongData data)
    {
        //相手から自分自身に向かうベクトル
        var toSelfVec = transform.position - data.gameObject.transform.position;

        if(toSelfVec.magnitude < m_param.nearObjectRange) {  //隣人なら
            return toSelfVec;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 近くの集団を回避する処理
    /// </summary>
    /// <param name="rigid">自身のリジッドボディ</param>
    /// <param name="moveDirect">そのオブジェクトが向かいたい方向</param>
    /// /// <param name="maxSpeed">最大スピード</param>
    public void AvoidNearThrong(EnemyVelocityMgr velcoityMgr, Vector3 moveDirect, float maxSpeed)
    {
        var velocity = velcoityMgr.velocity;

        moveDirect += CalcuThrongVector();
        Vector3 force = UtilityVelocity.CalucSeekVec(velocity, moveDirect, maxSpeed);
        velcoityMgr.AddForce(force);

        var avoidVec = CalcuSumAvoidVector();
        if (avoidVec != Vector3.zero) //回避が必要なら
        {
            Vector3 avoidForce = UtilityVelocity.CalucSeekVec(velocity, CalcuSumAvoidVector(), CalcuAverageSpeed());
            velcoityMgr.AddForce(avoidForce);
        }
    }

    /// <summary>
    /// 集団移動をする処理(まだ未完成)
    /// </summary>
    /// <param name="selfRigid">自分自身のリジッドボディ</param>
    public void ThrongMove(EnemyVelocityMgr velcoityMgr, Vector3 moveDirect, float maxSpeed)
    {
        var throngVec = CalcuThrongVector();
        if(throngVec == Vector3.zero)
        {
            return;
        }

        var force = UtilityVelocity.CalucSeekVec(velcoityMgr.velocity, throngVec, CalcuAverageSpeed());
        velcoityMgr.AddForce(force);
    }

    /// <summary>
    /// ランダムに移動するゾンビを集団に合わせた方向に統合する。
    /// </summary>
    /// <param name="plowlingMove">コンポ―ネントそのもの</param>
    public Vector3 CalcuRandomPlowlingMovePositonIntegrated(RandomPlowlingMove plowlingMove)
    {
        var throngDatas = m_generator.GetThrongDatas();

        int throngSize = 0;
        Vector3 sumPosition = Vector3.zero;
        foreach (var data in throngDatas)
        {
            if(IsRange(data, m_param.inThrongRange))
            {
                sumPosition += data.randomPlowlingMove.GetTargetPosition();
                throngSize++;
            }
        }

        return sumPosition / throngSize;
    }

    /// <summary>
    /// 群衆行動ベクトルの計算(まだ未完成)
    /// </summary>
    /// <returns></returns>
    Vector3 CalcuThrongVector()
    {
        var throngDatas = m_generator.GetThrongDatas();

        Vector3 centerPosition = Vector3.zero;
        Vector3 avoidVec = Vector3.zero;
        Vector3 directVec = Vector3.zero;
        float sumSpeed = 0.0f;

        int throngSize = 0;
        foreach (var data in throngDatas)
        {
            var toVec = data.gameObject.transform.position - transform.position;
            if (toVec.magnitude > m_param.inThrongRange)
            {  //距離が遠かったらcontinue
                continue;
            }

            centerPosition += CalcuCenterVector(data);
            directVec += CalcuDirectVector(data); //群衆の平均方向
            avoidVec += CalcuAvoidVector(data);
            sumSpeed += data.velocityMgr.velocity.magnitude;

            throngSize++;
        }

        if (throngSize == 0)
        {  //存在しなかったらzeroを返す。
            return Vector3.zero;
        }

        centerPosition /= throngSize;
        directVec /= throngSize;
        //directVec = (directVec - transform.position) / 8;

        //var reVec = (centerPosition + avoidVec) - transform.position;
        var reVec = (centerPosition + avoidVec + directVec) - transform.position;

        return reVec;
    }

    Vector3 CalcuSumAvoidVector()
    {
        var throngDatas = m_generator.GetThrongDatas();
        Vector3 avoidVector = Vector3.zero;

        foreach (var data in throngDatas)
        {
            avoidVector += CalcuAvoidVector(data);
        }

        return avoidVector;
    }

    float CalcuAverageSpeed()
    {
        var throngDatas = m_generator.GetThrongDatas();
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

    bool IsRange(ThrongData data ,float range)
    {
        var toVec = data.gameObject.transform.position - transform.position;

        return toVec.magnitude < range ? true : false;
    }

    //アクセッサ-----------------------------------------------

    /// <summary>
    /// 群衆データリストを渡す。
    /// </summary>
    /// <returns>群衆データリスト</returns>
    public List<ThrongData> GetThrongDatas()
    {
        return m_generator.GetThrongDatas();
    }
    


    //null回避--------------------------

    void SetSearchGenerator()
    {
        if(m_generator != null) {  //null出なかったら処理をしない。
            return;
        }

        var generators = FindObjectsOfType<EnemyGenerator>();

        foreach (var generator in generators)
        {
            var createObj = generator.GetCreateObject();
            //生成するオブジェクトがこのオブジェクトと同じなら
            if (createObj.GetType() == gameObject.GetType())
            {
                m_generator = generator;
                break;
            }
        }
    }


    //ボツデータ↓

    //群衆として認識するオブジェクトのセット
    //void SetCalcuThrongList()
    //{
    //    var datas = m_generator.GetThrongDatas();
    //    foreach (var data in datas)
    //    {
    //        if (this.gameObject == data.gameObject)
    //        {  //自分自身ならcontinue
    //            continue;
    //        }

    //        var toVec = data.gameObject.transform.position - transform.position;
    //        if (toVec.magnitude < m_param.inThrongRange)  //群衆になる範囲にいたら
    //        {
    //            if (IsNewData(data))  //新規のデータだったら
    //            {
    //                m_throngDatas.Add(data);
    //            }
    //        }
    //    }
    //}

    //bool IsNewData(ThrongData newData)
    //{
    //    foreach (var data in m_throngDatas)
    //    {
    //        if (data.gameObject == newData.gameObject)
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}
}
