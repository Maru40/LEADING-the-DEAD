﻿using System.Collections;
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


public class EnemyGenerator : MonoBehaviour
{
    //近くに生成したくないオブジェクト群
    [SerializeField]
    List<OutOfTargetData> m_outOfTargteDatas =  new List<OutOfTargetData>();

    [SerializeField]
    protected GameObject m_createObject = null;

    [SerializeField]
    protected int m_numCreate = 30;

    [SerializeField]
    protected Vector3 m_centerPosition = new Vector3();  //生成するときの中心点
    [SerializeField]
    protected Vector3 m_maxRandomRange = new Vector3();  //ランダムに生成する時の最大距離

    //配布するデータの構造体
    [SerializeField]
    List<DropDataDistributionParametor> m_distributionParams = new List<DropDataDistributionParametor>();
    //データを配布する処理をまとめたクラス
    RandomDropDataDistribution m_distribution;

    //生成したゾンビを持つ
    protected List<ThrongData> m_datas = new List<ThrongData>();

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

        m_datas.Add(new ThrongData(obj.GetComponent<EnemyVelocityMgr>(),
            obj.GetComponent<TargetManager>(),
            obj.GetComponent<ThrongManager>(),
            obj.GetComponent<RandomPlowlingMove>(),
            obj.GetComponent<DropObjecptManager>()
        )); ;
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
    public Vector3 CalcuRandomPosition()
    {
        return RandomPosition.OutCameraAndOutRangeOfTargets(
            m_outOfTargteDatas,
            Camera.main, m_maxRandomRange, m_centerPosition);
    }

    /// <summary>
    /// DropItem情報を配布をする処理
    /// </summary>
    public void DropDistribution()
    {
        m_distribution.Distribution(m_datas);
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
