using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

[Serializable]
public struct DropDataDistributionParametor
{
    public DropData dropData;
    public float numDistribution;  //配布する数
}

/// <summary>
/// ランダムにドロップデータを配布する処理を行った。
/// </summary>
public class RandomDropDataDistribution
{
    //配布するデータの構造体
    private List<DropDataDistributionParametor> m_params = new List<DropDataDistributionParametor>();

    public RandomDropDataDistribution(List<DropDataDistributionParametor> parametors)
    {
        foreach(var param in parametors)
        {
            //オブジェクトの生成
            var newObj = MonoBehaviour.Instantiate(param.dropData.obj);
            newObj.gameObject.SetActive(false);

            //パラメータの生成。
            var newParam = new DropDataDistributionParametor();
            newParam.dropData = new DropData(newObj, param.dropData.probability);
            newParam.numDistribution = param.numDistribution;
            m_params.Add(newParam);
        }

        //this.m_params = parametors;
    }

    /// <summary>
    /// データの配布
    /// </summary>
    /// <param name="originDatas"></param>
    public void Distribution(List<ThrongData> originDatas)
    {
        foreach (var param in m_params)
        {
            var datas = new List<ThrongData>(originDatas);

            for (int i = 0; i < param.numDistribution; i++)
            {
                int index = MyRandom.RandomValue(0, datas.Count);

                if (datas[index].dropManager)
                {
                    datas[index].dropManager.AddData(param.dropData);
                    datas.RemoveAt(index);
                }
            }
        }
    }

    /// <summary>
    /// データの配布
    /// </summary>
    /// <param name="throngOriginDatas"></param>
    /// <param name="dropOriginDatas"></param>
    public void Distribution(List<ThrongData> throngOriginDatas, List<DropData> dropOriginDatas)
    {
        var dropDatas = new List<DropData>(dropOriginDatas);
        var throngDatas = new List<ThrongData>(throngOriginDatas);
        foreach (var dropData in dropDatas)
        {
            int index = MyRandom.RandomValue(0, throngDatas.Count);

            if (throngDatas[index].dropManager)
            {
                throngDatas[index].dropManager.AddData(dropData);
                throngDatas.RemoveAt(index);
            }
        } 
    }
}
