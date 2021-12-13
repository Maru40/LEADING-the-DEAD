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
    public int NumCreate => m_numCreate;
    public int GetNumAliveZombie() //生存しているゾンビの総数
    {
        int count = 0;
        foreach (var data in m_datas)
        {
            if (data.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }

    [SerializeField]
    protected Vector3 m_centerPosition = new Vector3();  //生成するときの中心点
    [SerializeField]
    protected Vector3 m_maxRandomRange = new Vector3();  //ランダムに生成する時の最大距離

    [SerializeField]
    protected bool m_isInCameraCreate = false;  //カメラの範囲内に生成するかどうか
    public bool IsInCameraCreate {  //カメラの範囲内に生成するかどうかのアクセッサ
        get => m_isInCameraCreate;
        set => m_isInCameraCreate = value;
    }

    [Header("障害物として扱う名前群"),SerializeField]
    string[] m_obstacleLayerStrings = new string[] { "L_Obstacle" };

    //配布するデータの構造体
    //[Header("ドロップアイテムを配布するデータ群"), SerializeField]
    //List<DropDataDistributionParametor> m_distributionParams = new List<DropDataDistributionParametor>();
    ////データを配布する処理をまとめたクラス
    //RandomDropDataDistribution m_distribution;

    //生成したゾンビを持つ
    protected List<ThrongData> m_datas = new List<ThrongData>();
    static List<ThrongData> sm_allDatas = new List<ThrongData>();

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

        //m_distribution = new RandomDropDataDistribution(m_distributionParams);

        CreateObjects();
        //DropDistribution();
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
    //public void DropDistribution()
    //{
    //    m_distribution.Distribution(m_datas);
    //}

    //public void DropDistribution(List<DropData> dropDatas)
    //{
    //    m_distribution.Distribution(m_datas ,dropDatas);
    //}

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
            data.gameObject.SetActive(true);
            data.respawn.Respawn();
        }
    }

    //アクセッサ---------------------------------------------------------------------------

    static public void AllDestroy()
    {
        foreach(var data in sm_allDatas)
        {
            Destroy(data.gameObject);
        }
    }

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
