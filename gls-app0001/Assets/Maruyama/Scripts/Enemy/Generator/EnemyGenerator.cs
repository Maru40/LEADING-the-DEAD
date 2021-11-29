using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

/// <summary>
/// 一定距離離れた場所に生成したいデータ構造体
/// </summary>
[Serializable]
public struct OutOfTargetData
{
    public GameObject target;  //ターゲット
    public float range;  //どれだけ離れた距離か

    public OutOfTargetData(GameObject target)
        :this(target, 15.0f)
    {}

    public OutOfTargetData(GameObject target, float range)
    {
        this.target = target;
        this.range = range;
    }
}


public class EnemyGenerator : GeneratorBase
{
    [Serializable]
    public struct Parameotor
    {
        public GameObject createObject;
        public int numCreate;
        public Vector3 centerPosition;
        public Vector3 maxRandomRange;
        public bool isInCameraCreate;

        public Parameotor(GameObject createObject, int numCreate, Vector3 centerPosition, Vector3 maxRandomRange)
        {
            this.createObject = createObject;
            this.numCreate = numCreate;
            this.centerPosition = centerPosition;
            this.maxRandomRange = maxRandomRange;
            this.isInCameraCreate = false;
        }
    }

    [Header("セレクト時のみ範囲を表示するかどうか"),SerializeField]
    bool m_isSelectDrawGizmos = false;
    [Header("生成範囲表示カラー"),SerializeField]
    Color m_gizmosColor = new Color(1.0f, 0, 0, 0.3f);

    //近くに生成したくないオブジェクト群
    [Header("近くに生成したくないオブジェクト群"), SerializeField]
    List<OutOfTargetData> m_outOfTargteDatas =  new List<OutOfTargetData>();

    [SerializeField]
    protected GameObject m_createObject = null;

    [SerializeField]
    protected int m_numCreate = 30;

    [SerializeField]
    protected Vector3 m_centerPosition = new Vector3();  //生成するときの中心点
    [SerializeField]
    protected Vector3 m_maxRandomRange = new Vector3();  //ランダムに生成する時の最大距離

    [SerializeField]
    protected bool m_isInCameraCreate = false;  //カメラの範囲内に生成するかどうか

    [Header("障害物として扱う名前群"),SerializeField]
    string[] m_obstacleLayerStrings = new string[] { "L_Obstacle" };

    //配布するデータの構造体
    [Header("ドロップアイテムを配布するデータ群"), SerializeField]
    List<DropDataDistributionParametor> m_distributionParams = new List<DropDataDistributionParametor>();
    //データを配布する処理をまとめたクラス
    RandomDropDataDistribution m_distribution;

    //生成したゾンビを持つ
    protected List<ThrongData> m_datas = new List<ThrongData>();
    static List<ThrongData> sm_allDatas = new List<ThrongData>();

    [Header("クリアに貢献できる時間"), SerializeField]
    float m_clearServicesTime = 5.0f;
    HashSet<GameObject> m_clearServices = new HashSet<GameObject>();  //クリアに貢献したオブジェクト
    List<GameObject> m_removeClearServices = new List<GameObject>();  //削除申請の出されたクリア貢献オブジェクト
    Dictionary<GameObject, GameTimer> m_clearServicesTimerDictionary = new Dictionary<GameObject, GameTimer>();

    protected virtual void Awake()
    {
        sm_allDatas.Clear();
    }

    protected virtual void Start()
    {
        //nullCheck
        if(m_outOfTargteDatas.Count == 0)
        {
            var barricade = GameObject.Find("Barricade");
            m_outOfTargteDatas.Add(new OutOfTargetData(barricade));
        }

        m_distribution = new RandomDropDataDistribution(m_distributionParams);

        CreateObjects();
        DropDistribution();
    }

    private void Update()
    {
        //m_clearServicesTimer.UpdateTimer();

        UpdateClearServicesTimers();
        RemoveClearServices();
        //Debug.Log("△" + m_clearServices.Count);
        //Debug.Log("■" + m_clearServicesTimers.Count);
        Debug.Log("〇" + GetNumAllClearServices());
    }

    void UpdateClearServicesTimers()
    {
        System.Action removeAction = null;

        foreach (var pairs in m_clearServicesTimerDictionary)
        {
            var timer = pairs.Value;
            timer.UpdateTimer();
            if (timer.IsTimeUp) {
                removeAction += () => m_clearServicesTimerDictionary.Remove(pairs.Key);
            }
        }

        removeAction?.Invoke();
    }

    //貢献した数の削除
    void RemoveClearServices()
    {        
        foreach (var remove in m_removeClearServices)
        {
            m_clearServices.Remove(remove);
        }
    }

    void CreateObjects()
    {
        for (int i = 0; i < m_numCreate; i++)
        {
            var createPosition = CalcuRandomPosition();
            CreateObject(createPosition);
        }
    }

    void CreateObject(Vector3 createPosition)
    {
        var obj = Instantiate(m_createObject, createPosition, Quaternion.identity, transform);
        CreateObjectAdjust(obj);  //調整

        var newData = new ThrongData(obj.GetComponent<EnemyVelocityMgr>(),
            obj.GetComponent<TargetManager>(),
            obj.GetComponent<ThrongManager>(),
            obj.GetComponent<RandomPlowlingMove>(),
            obj.GetComponent<DropObjecptManager>(),
            obj.GetComponent<ClearManager_Zombie>(),
            obj.GetComponent<EnemyRespawnManager>()
        );

        m_datas.Add(newData);
        sm_allDatas.Add(newData);
    }

    /// <summary>
    /// 調整が必要なオブジェクトを渡して、調整
    /// </summary>
    /// <param name="obj">調整したいオブジェクト</param>
    protected virtual void CreateObjectAdjust(GameObject obj)
    {
        var throng = obj.GetComponent<ThrongManager>();
        if (throng)
        {
            throng.SetGenerator(this);
        }

        var respawn = obj.GetComponent<EnemyRespawnManager>();
        if (respawn)
        {
            respawn.SetGenerator(this);
        }
    }

    /// <summary>
    /// ターゲットから離れた場所を、ランダムに返す。
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <returns>ランダムな位置</returns>
    public Vector3 CalcuRandomPosition()
    {
        //return RandomPosition.CalcuPosition(m_maxRandomRange, m_centerPosition);

        if(m_isInCameraCreate)
        {
            return RandomPosition.OutObstacleAndOutRangeOfTargets(
                m_outOfTargteDatas,
                m_maxRandomRange, m_centerPosition, m_obstacleLayerStrings);
        }
        else
        {
            return RandomPosition.OutCameraAndOutObstacleAndOutRangeOfTargets(
               m_outOfTargteDatas,
               Camera.main, m_maxRandomRange, m_centerPosition, m_obstacleLayerStrings);
        }
    }

    /// <summary>
    /// DropItem情報を配布をする処理
    /// </summary>
    public void DropDistribution()
    {
        m_distribution.Distribution(m_datas);
    }

    public void DropDistribution(List<DropData> dropDatas)
    {
        m_distribution.Distribution(m_datas ,dropDatas);
    }

    /// <summary>
    /// ゲームクリア時に呼んで欲しい関数
    /// </summary>
    public virtual void ClearProcess()
    {
        foreach(var data in m_datas)
        {
            data.clearManager.ClearProcess();
        }
    }

    /// <summary>
    /// 全部リスポーン
    /// </summary>
    public void RepawnPositoinAll()
    {
        foreach(var data in m_datas)
        {
            data.respawn.RespawnPosition();
        }
    }

    //アクセッサ---------------------------------------------------------------------------

    /// <summary>
    /// 生成するオブジェクトと渡されたオブジェクトが同じprefabなら
    /// </summary>
    /// <param name="gameObj">比較対象のオブジェクト</param>
    /// <returns>同じならtrue</returns>
    public bool IsEqualCreateObject(GameObject gameObj)
    {
        return m_createObject.GetType() == gameObj.GetType() ? true : false;
    }

    public List<ThrongData> GetThrongDatas()
    {
        return sm_allDatas;
    }

    public GameObject GetCreateObject()
    {
        return m_createObject;
    }

    /// <summary>
    /// ランダムに生成するときの最大距離
    /// </summary>
    /// <returns>最大距離</returns>
    public Vector3 GetMaxRandomRange()
    {
        return m_maxRandomRange;
    }

    public Vector3 GetCenterPosition()
    {
        return m_centerPosition;
    }

    public void AddClearServices(AttributeObject.DamageData data)
    {
        AddClearServices(data.obj);
    }

    /// <summary>
    /// クリアに貢献したオブジェクトの追加
    /// </summary>
    /// <param name="gameObj"></param>
    public void AddClearServices(GameObject gameObj)
    {
        if (m_clearServices.Add(gameObj))  //追加されたら
        {
            //一定時間後に削除をする。
            m_clearServicesTimerDictionary[gameObj] = new GameTimer(m_clearServicesTime, () => {
                EndClearServices(gameObj);
            });
        }
        else
        {
            //時間リセット
            var timer = m_clearServicesTimerDictionary[gameObj];
            timer.ResetTimer(m_clearServicesTime, () => {
                EndClearServices(gameObj);
            });
        }
    }

    /// <summary>
    /// 貢献終了時
    /// </summary>
    void EndClearServices(GameObject gameObj)
    {
        m_removeClearServices.Add(gameObj);
    }

    /// <summary>
    /// クリアに貢献したゾンビを全て取得
    /// </summary>
    /// <returns></returns>
    static public int GetNumAllClearServices()
    {
        var generators = GameObject.FindObjectsOfType<EnemyGenerator>();

        int count = 0;
        foreach(var generator in generators)
        {
            count += generator.NumClearServices;
        }

        return count;
    }

    /// <summary>
    /// クリアに貢献した数。
    /// </summary>
    int NumClearServices => m_clearServices.Count;

    //Gizmos-------------------------------------------------------------------------------------------

    private void OnDrawGizmosSelected()
    {
        //セレクト時のみ表示だったら
        if (m_isSelectDrawGizmos)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmos()
    {
        //セレクト時のみ表示で無かったら
        if (!m_isSelectDrawGizmos)
        {
            DrawGizmos();
        }
    }

    /// <summary>
    /// 生成範囲表示用
    /// </summary>
    void DrawGizmos()
    {
        Gizmos.color = m_gizmosColor;
        var maxRandomRange = m_maxRandomRange * 2.0f;
        Gizmos.DrawCube(m_centerPosition, maxRandomRange);
    }
}
