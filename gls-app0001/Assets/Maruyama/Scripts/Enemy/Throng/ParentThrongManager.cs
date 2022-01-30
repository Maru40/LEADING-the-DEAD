using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;
using System.Linq;

//親集団行動管理
public class ParentThrongManager : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float maxSpeed;
        public float nearRange;
        public float range;  //自分のどのくらい離れた位置を追従するか？
        [SerializeField]
        private int sides;
        public float Sides => (float)sides;

        public ThrongManagerParametor throngParam;
    }

    //目的の場所群
    private List<Vector3> m_destinationPositions = new List<Vector3>();

    [SerializeField]
    private Parametor m_param = new Parametor();
    private List<ThrongData> m_throngDatas = new List<ThrongData>();

    [SerializeField]
    private TriggerAction m_triggerAction;
    //private EnemyVelocityManager m_velocityManager;

    private void Awake()
    {
        //m_velocityManager = GetComponent<EnemyVelocityManager>();
    }

    private void Start()
    {
        m_triggerAction.AddEnterAction(TriggerEnter);

        CreateDestinationPosition();
    }

    private void Update()
    {
        if(m_throngDatas.Count != 0)
        {
            ThrongUpdate();
        }
    }

    private void ThrongUpdate()
    {
        foreach(var data in m_throngDatas)
        {
            var velocityManager = data.velocityMgr;

            var velocity = velocityManager.velocity;
            var destinationPosition = transform.position + CalcuDestinationVector(data);
            var toTargetVector = destinationPosition - transform.position;
            var force = CalcuVelocity.CalucNearArriveFarSeek(velocity, toTargetVector, m_param.maxSpeed, m_param.nearRange);
            velocityManager.AddForce(force);

            ThrongMoveUpdate(velocityManager);
        }
    }

    private Vector3 CalcuDestinationVector(ThrongData data)
    {
        var positions = new List<Vector3>();
        var destinationVector = Vector3.zero;
        foreach(var offset in m_destinationPositions)
        {
            var toPosition = (transform.position + offset) - data.gameObject.transform.position;
            positions.Add(toPosition);
        }

        var sortPositions = positions.OrderBy(toPosition => toPosition.magnitude).ToArray();

        return sortPositions[0];
    }

    private void CreateDestinationPosition()
    {
        var sides = m_param.Sides;
        for (int i = 0; i < sides; i++)
        {
            var degree = (360.0f / sides) * i;

            var rotQuat = Quaternion.AngleAxis(degree, Vector3.up);
            var direct = rotQuat * Vector3.forward;

            var position = direct.normalized * m_param.range;
            m_destinationPositions.Add(position);
        }
    }

    /// <summary>
    /// 近くの集団を回避する処理
    /// </summary>
    /// <param name="rigid">自身のリジッドボディ</param>
    /// <param name="moveDirect">そのオブジェクトが向かいたい方向</param>
    /// <param name="maxSpeed">最大スピード</param>
    /// <param name="truningPower">旋回パワー</param>
    private void ThrongMoveUpdate(EnemyVelocityManager velocityManager)
    {
        var velocity = velocityManager.velocity;
        velocity += velocityManager.GetForce() * Time.deltaTime;

        var throngForce = CalcuVelocity.CalucSeekVec(velocity, CalcuThrongVector(), CalcuAverageSpeed());

        velocityManager.AddForce(throngForce);
    }

    /// <summary>
    /// 群衆行動ベクトルの計算(まだ未完成)
    /// </summary>
    /// <returns></returns>
    public Vector3 CalcuThrongVector()
    {
        var throngDatas = m_throngDatas;

        Vector3 centerPosition = Vector3.zero;
        Vector3 avoidVec = Vector3.zero;
        Vector3 directVec = Vector3.zero;
        float sumSpeed = 0.0f;

        int throngSize = 0;
        foreach (var data in throngDatas)
        {
            if (data.gameObject == gameObject)
            {  //自分自身なら処理をしない
                continue;
            }

            //集団距離内でかつ、障害物がない場合
            if (Calculation.IsRange(gameObject, data.gameObject, m_param.throngParam.inThrongRange) && 
                !Obstacle.IsLineCastObstacle(transform.position, data.gameObject.transform.position))
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
        if (data.gameObject == gameObject)
        {  //自分自身なら処理をしない
            return Vector3.zero;
        }

        //相手から自分自身に向かうベクトル
        var toSelfVec = transform.position - data.gameObject.transform.position;

        if (Calculation.IsRange(gameObject, data.gameObject, m_param.throngParam.nearObjectRange))
        {  //隣人なら
            var power = m_param.throngParam.maxAvoidPower - toSelfVec.magnitude;
            var avoidVec = toSelfVec.normalized * power;

            return avoidVec;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 集団の平均スピードを計算して返す。
    /// </summary>
    /// <returns>平均スピード</returns>
    private float CalcuAverageSpeed()
    {
        var throngDatas = m_throngDatas;
        float sumSpeed = 0.0f;
        int throngSize = 0;

        foreach (var data in throngDatas)
        {
            if (Calculation.IsRange(gameObject, data.gameObject, m_param.throngParam.nearObjectRange))
            {
                sumSpeed += data.velocityMgr.velocity.magnitude;
                throngSize++;
            }
        }

        if (throngSize == 0)
        {
            return 0;
        }

        return sumSpeed / throngSize;
    }

    //終了
    public void EndProcess()
    {
        foreach(var data in m_throngDatas)
        {
            data.throngMgr.enabled = true;
            data.gameObject.GetComponent<ChaseTarget>().enabled = true;
            //data.gameObject.GetComponent<StatorBase>().enabled = true;
        }

        m_throngDatas.Clear();
    }

    private void TriggerEnter(Collider other)
    {
        if(IsRegisterGameObject(other.gameObject)) {
            return;
        }

        var throng = other.GetComponent<ThrongManager>();
        if (throng)
        {
            var data = CalcuThrongData(other.gameObject);
            m_throngDatas.Add(data);

            //other.GetComponent<StatorBase>().enabled = false;
            data.gameObject.GetComponent<ChaseTarget>().enabled = false;
            data.throngMgr.enabled = false;
        }
    }

    private void TriggerExit(Collider other)
    {

    }

    /// <summary>
    /// 登録済みのゲームオブジェクトかどうか
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private bool IsRegisterGameObject(GameObject gameObject)
    {
        foreach(var data in m_throngDatas)
        {
            if(data.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    protected ThrongData CalcuThrongData(GameObject obj)
    {
        var newData = new ThrongData(obj.GetComponent<EnemyVelocityManager>(),
            obj.GetComponent<TargetManager>(),
            obj.GetComponent<ThrongManager>(),
            obj.GetComponent<RandomPlowlingMove>(),
            obj.GetComponent<DropObjecptManager>(),
            obj.GetComponent<ClearManager_Zombie>(),
            obj.GetComponent<EnemyRespawnManager>()
        );

        return newData;
    }
}
