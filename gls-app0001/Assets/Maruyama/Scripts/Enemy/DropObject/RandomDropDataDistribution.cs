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
    List<DropDataDistributionParametor> m_params = new List<DropDataDistributionParametor>();

    public RandomDropDataDistribution(List<DropDataDistributionParametor> parametors)
    {
        this.m_params = parametors;
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
}
