using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_createObject = null;

    [SerializeField]
    protected int m_numCreate = 30;

    [SerializeField]
    protected Vector3 m_centerPosition = new Vector3();  //生成するときの中心点
    [SerializeField]
    protected Vector3 m_maxRandomRange = new Vector3();  //ランダムに生成する時の最大距離

    [SerializeField]
    protected GameObject m_target = null;
    [SerializeField]
    protected float m_outRange = 15.0f;

    //生成したゾンビを持つ
    protected List<ThrongData> m_datas = new List<ThrongData>();

    protected virtual void Start()
    {
        if(m_target == null)
        {
            m_target = GameObject.Find("Player");
        }

        CreateObjects();
    }

    void CreateObjects()
    {
        var random = new System.Random(System.DateTime.Now.Millisecond);

        for (int i = 0; i < m_numCreate; i++)
        {
            var createPosition = CalcuRandomPosition(m_target);
            CreateObject(createPosition);
        }
    }

    void CreateObject(Vector3 createPosition)
    {
        var obj = Instantiate(m_createObject, createPosition, Quaternion.identity, transform);
        CreateObjectAdjust(obj);  //調整
        
        m_datas.Add(new ThrongData(obj.GetComponent<EnemyVelocityMgr>(),
            obj.GetComponent<TargetManager>(),
            obj.GetComponent<ThrongManager>(),
            obj.GetComponent<RandomPlowlingMove>()
        ));
    }

    /// <summary>
    /// 調整が必要なオブジェクトを渡して、調整
    /// </summary>
    /// <param name="obj">調整したいオブジェクト</param>
    protected virtual void CreateObjectAdjust(GameObject obj) { }


    /// <summary>
    /// ターゲットから離れた場所を、ランダムに返す。
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <returns>ランダムな位置</returns>
    public Vector3 CalcuRandomPosition(GameObject target)
    {
        return UtilityRandomPosition.OutRangeOfTarget(target, m_outRange, m_maxRandomRange, m_centerPosition);
    }

    //アクセッサ---------------------------------------

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
        return m_datas;
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
}
